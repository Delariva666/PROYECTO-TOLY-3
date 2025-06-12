using UnityEngine;

public class Toly : MonoBehaviour
{
    // 1. Tus referencias existentes
    Mover mover;
    Collisiones collisiones;
    Animaciones animaciones;
    Rigidbody2D rb2D;

    // 2. Nueva referencia al manager de Game Over
    //    Así Unity genera un campo en el Inspector donde arrastras tu objeto GameOverManager
    public GameOverManager gameOverManager;

    private void Awake()
    {
        mover       = GetComponent<Mover>();
        collisiones = GetComponent<Collisiones>();
        animaciones = GetComponent<Animaciones>();
        rb2D        = GetComponent<Rigidbody2D>();

        // 3. Si no asignaste manual el manager en el Inspector, lo busca solo
        if (gameOverManager == null)
            gameOverManager = FindObjectOfType<GameOverManager>();
    }

    public void Hit()
    {
        Debug.Log("Recibió un golpe");
        animaciones.Hurt();
        Dead();
    }

    private void Dead()
    {
        // 4. Sólo entramos aquí si ya estamos "muertos"
        if (collisiones.IsDead)
        {
            // 5. Deshabilitamos el control de movimiento para que no siga andando
            mover.InputMoveEnable = false;
            Debug.Log("Movimiento deshabilitado");

            // 6. Reproducimos la animación de muerte
            animaciones.Death();

            // 7. Pausamos la lógica del juego (física, animaciones de Update, etc.)
            Time.timeScale = 0f;

            // 8. ¡Y lanzamos el menú de Game Over!
            gameOverManager.ShowGameOver();
        }
    }
}
