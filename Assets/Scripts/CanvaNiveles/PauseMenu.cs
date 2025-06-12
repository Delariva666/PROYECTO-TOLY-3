using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
public GameObject canvasPausa; // Referencia al Canvas de Pausa
    public GameObject Canvas; // Referencia al Canvas Principal

    private bool isGamePaused = false; // Estado del juego (pausado o no)

    // Método que activa el menú de pausa
    public void ShowPauseMenu()
    {
        canvasPausa.SetActive(true); // Mostrar el Canvas de Pausa
        Canvas.SetActive(false); // Ocultar el Canvas Principal
        Time.timeScale = 0f; // Detener el tiempo en el juego
        isGamePaused = true; // Cambiar el estado a pausado
    }

    // Método que reanuda el juego
    public void ResumeGame()
    {
        canvasPausa.SetActive(false); // Ocultar el Canvas de Pausa
        Canvas.SetActive(true); // Mostrar el Canvas Principal
        Time.timeScale = 1f; // Reanudar el tiempo en el juego
        isGamePaused = false; // Cambiar el estado a no pausado
    }

    // Método para salir al menú principal
    public void QuitToMainMenu()
    {
        Time.timeScale = 1f; // Asegurarse de que el tiempo esté reanudado antes de cambiar de escena
        SceneManager.LoadScene("Main-Menu"); // Cargar la escena del menú principal
    }
}
