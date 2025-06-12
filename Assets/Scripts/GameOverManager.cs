using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    [Tooltip("Arrastra aquí el GameOverPanel desde el Inspector")]
    public GameObject gameOverPanel;

    void Start()
    {
        // Al iniciar, aseguramos que el panel esté oculto
        gameOverPanel.SetActive(false);
    }

    // Llama a este método cuando el jugador muera
    public void ShowGameOver()
    {
        // Pausa el juego
        Time.timeScale = 0f;
        // Muestra el panel
        gameOverPanel.SetActive(true);
    }

    // Botón Reintentar
    public void OnRetry()
    {
        Time.timeScale = 1f; 
        // Recarga la escena actual
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Botón Volver al Menú
    public void OnQuit()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main-Menu"); // cambia por el nombre real de tu menú
    }
}
