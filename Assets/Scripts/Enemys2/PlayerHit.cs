using System.Collections;
using UnityEngine;

public class PlayerHit : MonoBehaviour
{
    private Animator animator;
    private int hitCount = 0; // Número de golpes recibidos
    private bool isHurt = false; // Bandera para evitar múltiples hits simultáneos
    private GameManager gameManager; // Referencia al GameManager
    private AutoMovement autoMovement; // Control del movimiento enemigo

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>(); // Busca el GameManager en la escena
        autoMovement = GetComponent<AutoMovement>(); // Obtiene el componente de movimiento
    }

    private void Awake()
    {
        animator = GetComponent<Animator>(); // Inicializa el Animator
    }

    // Método llamado cuando el enemigo recibe un golpe
    public void Hit()
    {
        if (isHurt) return; // Evita múltiples golpes simultáneos

        isHurt = true; 
        animator.SetTrigger("HitHurt"); // Activa animación de golpe
        hitCount++; 

        if (hitCount >= 3) // Si el enemigo ha recibido 3 golpes, muere
        {
            EnemyDeath();
        }
        else
        {
            StartCoroutine(PlayHurtAnimation());
        }
    }

    // Animación de "lastimado" por un corto tiempo
    private IEnumerator PlayHurtAnimation()
    {
        float hurtDuration = 0.1f; // Duración del efecto "lastimado"
        yield return new WaitForSeconds(hurtDuration);
        
        animator.SetTrigger("Walk"); // Vuelve a la animación de caminar
        isHurt = false; // Resetea la bandera
    }

    // Método llamado cuando el enemigo muere
    public void EnemyDeath()
    {
        animator.SetTrigger("HitDeath"); // Activa animación de muerte
        gameObject.layer = LayerMask.NameToLayer("OnlyGround"); // Cambia la capa del objeto

        if (gameManager != null)
        {
            gameManager.RegistrarEnemigoEliminado(); // Incrementa la puntuación en el GameManager
        }

        autoMovement.PauseMovement(); // Pausa cualquier movimiento del enemigo
        Destroy(gameObject, 1f); // Destruye el objeto después de 1 segundo
    }
}


