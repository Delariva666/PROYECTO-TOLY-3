using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNivel1 : MonoBehaviour
{
    // Este método debe estar vinculado al botón desde el inspector de Unity
    public void CargarNivel1()
    {
        // Asegúrate de que la escena "nivel1" esté incluida en la lista de escenas en Build Settings
        SceneManager.LoadScene("Nivel1");
        Debug.Log("Cargando la escena 'nivel1'");
    }
}
