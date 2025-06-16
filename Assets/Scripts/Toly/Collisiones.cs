using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Collisiones : MonoBehaviour
{
    [Header("Suelo")]
    public bool isGrounded;
    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask groundLayer;

    [Header("Vida")]
    public int maxHealth = 5;
    private int currentHealth;
    public float recoveryTime = 1.5f;
    private bool canCollide = true;

    [Header("UI")]
    public Slider healthSlider;
    public HeartManager heartManager;

    [Header("Audio")]
    [Tooltip("Arrastra aquí el clip de sonido que se reproduce al morir")]
    public AudioClip deathClip;

    public bool IsDead => currentHealth <= 0;

    private BoxCollider2D col2D;
    private Toly toly;

    private void Awake()
    {
        col2D = GetComponent<BoxCollider2D>();
        toly = GetComponent<Toly>();
        currentHealth = maxHealth;

        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }

        if (heartManager != null)
        {
            Debug.Log("✔️ HeartManager conectado en Awake");
            heartManager.UpdateHearts(currentHealth);

            for (int i = 0; i < heartManager.heartImages.Length; i++)
            {
                if (heartManager.heartImages[i] != null)
                    Debug.Log($"❤️ Corazón {i} asignado: {heartManager.heartImages[i].name}");
                else
                    Debug.LogWarning($"❌ Corazón {i} está vacío (no asignado)");
            }
        }
        else
        {
            Debug.LogWarning("❌ HeartManager no está asignado en el Inspector.");
        }
    }

    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    public bool Grounded()
    {
        Vector2 footLeft  = new Vector2(col2D.bounds.center.x - col2D.bounds.extents.x, col2D.bounds.center.y);
        Vector2 footRight = new Vector2(col2D.bounds.center.x + col2D.bounds.extents.x, col2D.bounds.center.y);

        Debug.DrawRay(footLeft,  Vector2.down * col2D.bounds.extents.y * 1.5f, Color.magenta);
        Debug.DrawRay(footRight, Vector2.down * col2D.bounds.extents.y * 1.5f, Color.magenta);

        return Physics2D.Raycast(footLeft,  Vector2.down, col2D.bounds.extents.y * 1.5f, groundLayer) ||
               Physics2D.Raycast(footRight, Vector2.down, col2D.bounds.extents.y * 1.5f, groundLayer);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject atacante = collision.gameObject;

        if (atacante.layer == LayerMask.NameToLayer("Enemy") && canCollide)
        {
            if (atacante.name == "Enemigos") return; // Evita daño por el contenedor

            StartCoroutine(HandleCollision());
        }
    }

    private IEnumerator HandleCollision()
    {
        canCollide = false;
        currentHealth--;

        Debug.Log($"💥 Daño recibido. Salud restante: {currentHealth}");

        if (healthSlider != null)
            healthSlider.value = currentHealth;

        if (heartManager != null)
        {
            Debug.Log("🫀 Actualizando corazones...");
            heartManager.UpdateHearts(currentHealth);
        }
        else
        {
            Debug.LogWarning("⚠️ No se puede actualizar corazones: heartManager es NULL.");
        }

        toly.Hit();

        if (IsDead)
        {
            Dead();
        }
        else
        {
            yield return new WaitForSeconds(recoveryTime);
            canCollide = true;
        }
    }

    public void Dead()
    {
        if (IsDead)
        {
            // Reproducir sonido de muerte
            if (deathClip != null)
            {
                AudioSource.PlayClipAtPoint(deathClip, transform.position);
            }

            gameObject.layer = LayerMask.NameToLayer("PlayerDead");
            Debug.Log("☠️ El jugador ha muerto.");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            PlayerHit playerHit = collision.GetComponent<PlayerHit>();
            if (playerHit != null)
            {
                playerHit.Hit();
                Debug.Log("⚔️ Enemigo golpeado por el jugador.");
            }
        }
    }
}
