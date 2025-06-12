using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ValidarContra : MonoBehaviour
{
    public GameObject PanelContrasena;    // El panel que contiene el InputField y el Button
    public InputField InputContrasena;    // El InputField donde el usuario escribe la contraseña
    public Text MensajeContra;            // El Text para mostrar mensajes como "Contraseña incorrecta"
    public Button BotonConfirmar;        // El botón para confirmar la contraseña
    public Button OpcionesButton;   // El botón que va a mostrar el panel de contraseña

    private string correctPassword = "1234"; // La contraseña correcta

    void Start()
    {
        // Inicialmente, el panel de contraseña está oculto
        PanelContrasena.SetActive(false);
        
        // Asociar el evento del botón de mostrar el panel al método
        OpcionesButton.onClick.AddListener(MostrarPanelContraseña);
        
        // Asociar el evento del botón "Confirmar" al método que verifica la contraseña
        BotonConfirmar.onClick.AddListener(VerificarContraseña);
    }

    // Método que se llama cuando el botón es presionado para mostrar el panel de la contraseña
    public void MostrarPanelContraseña()
    {
        // Mostrar el panel de la contraseña
        PanelContrasena.SetActive(true);
    }

    // Método para verificar la contraseña cuando se presiona el botón "Confirmar"
    void VerificarContraseña()
    {
        string contraseñaIngresada = InputContrasena.text;

        if (contraseñaIngresada == correctPassword)
        {
            // Contraseña correcta
            MensajeContra.text = "Contraseña correcta!";
            MensajeContra.color = Color.green;
            // Aquí puedes añadir la acción que quieres realizar si la contraseña es correcta
            PanelContrasena.SetActive(false);  // Cerrar el panel de la contraseña
            CargarNuevaEscena();
        }
        else
        {
            // Contraseña incorrecta
            MensajeContra.text = "Contraseña incorrecta, intenta de nuevo.";
            MensajeContra.color = Color.red;
            InputContrasena.text = "";
        }
    }

    void CargarNuevaEscena()
    {
        // Reemplaza "NombreDeTuEscena" con el nombre de la escena a la que deseas ir
        SceneManager.LoadScene("CRUD");  
    }

    public void CerrarPanel(){
        PanelContrasena.SetActive(false);
    }
}
