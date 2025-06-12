using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackMenuScript : MonoBehaviour
{ 
        [SerializeField] private string mainMenuSceneName = "Main-Menu";

    public void GoToMainMenu()
    {
        // Verificar si el nombre de la escena es válido
        if (string.IsNullOrEmpty(mainMenuSceneName))
        {
            Debug.LogError("Error: No se ha asignado un nombre para la escena del menú principal.");
            return;
        }

        // Cargar la escena del menú principal
        Debug.Log($"Regresando al menú principal: {mainMenuSceneName}");
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
