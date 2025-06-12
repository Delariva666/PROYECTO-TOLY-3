using UnityEngine;

public class NivelCompletoCollision : MonoBehaviour
{
    private GameManager gameManager;

    // Este método se ejecuta cuando hay una colisión entre este objeto y otro objeto
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Colisión detectada con: " + collision.gameObject.name);
        
        // Verifica si el objeto que colisiona tiene el tag "Player"
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("¡Colisionó con el jugador!");

            // Verifica si el GameObject con el collider tiene la capa correcta
            if (gameObject.layer == LayerMask.NameToLayer("NivelCompleto"))
            {
                Debug.Log("Nivel completado por colisión.");
                
                // Llama al método para completar el nivel
                if (gameManager != null)
                {
                    gameManager.CompletarNivelPorColision(); // Llama al método en GameManager
                }
                else
                {
                    Debug.LogError("El GameManager no está asignado.");
                }
            }
            else
            {
                Debug.Log("La capa no es la correcta para completar el nivel.");
            }
        }
        else
        {
            Debug.Log("El objeto que colisionó no es el jugador.");
        }
    }

    // Inicializa la referencia del GameManager
    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>(); // Busca el GameManager en la escena
        if (gameManager == null)
        {
            Debug.LogError("No se encontró el GameManager en la escena.");
        }
    }
}
