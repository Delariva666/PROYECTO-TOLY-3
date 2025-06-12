using System.Collections;
using UnityEngine;

public class PlayerHit : MonoBehaviour
{
    private Animator animator;
    private int hitCount = 0; // Número de golpes recibidos por el enemigo
    private bool isHurt = false; // Bandera para evitar múltiples hits simultáneos
    private GameManager gameManager; // Referencia al GameManager
    private AutoMovement autoMovement; // Movimiento del enemigo

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>(); // Buscar GameManager en escena
        autoMovement = GetComponent<AutoMovement>(); // Obtener script de movimiento enemigo
    }

    private void Awake()
    {
        animator = GetComponent<Animator>(); // Obtener Animator
    }

    // ⚠ NUEVO: Ataque al jugador con animación
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            animator.SetTrigger("AproximateAttack");


            var jugador = other.GetComponent<Toly>();
            if (jugador != null)
            {
                jugador.Hit(); // Aplica daño al jugador
            }
        }
    }

    // Lógica cuando el enemigo recibe un golpe del jugador
    public void Hit()
    {
        if (isHurt) return;

        isHurt = true;
        animator.SetTrigger("HitHurt"); // Animación de recibir golpe
        hitCount++;

        if (hitCount >= 3) // Muere al tercer golpe
        {
            EnemyDeath();
        }
        else
        {
            StartCoroutine(PlayHurtAnimation());
        }
    }

    // Breve pausa visual de estar "lastimado"
    private IEnumerator PlayHurtAnimation()
    {
        yield return new WaitForSeconds(0.1f);
        animator.SetTrigger("Walk"); // Volver a caminar
        isHurt = false;
    }

    // Lógica de muerte del enemigo
    public void EnemyDeath()
    {
        animator.SetTrigger("HitDeath"); // Animación de muerte
        gameObject.layer = LayerMask.NameToLayer("OnlyGround");

        if (gameManager != null)
        {
            gameManager.RegistrarEnemigoEliminado(); // Informar al GameManager
        }

        autoMovement.PauseMovement(); // Detener movimiento
        Destroy(gameObject, 1f); // Destruir después de 1 segundo
    }
}