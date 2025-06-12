using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int puntosPorEnemigo = 10;
    public int puntos = 0;
    public int nivel = 1;
    public float tiempoNivel = 100f;
    private string nombreDelJugador;
    private DataBaseAccess databaseAccess;

    private bool nivelActivo = true;

    void Start()
    {
        databaseAccess = FindObjectOfType<DataBaseAccess>();

        if (databaseAccess != null)
        {
            // Obtener el nombre del jugador desde PlayerPrefs
            nombreDelJugador = PlayerPrefs.GetString("JugadorSeleccionado", "Jugador por defecto");

            if (string.IsNullOrEmpty(nombreDelJugador) || nombreDelJugador == "Jugador por defecto")
            {
                Debug.LogError("Nombre del jugador no ha sido establecido.");
                return;
            }

            // Obtener los datos del jugador desde la base de datos
            var (cargadosPuntos, cargadosNivel) = databaseAccess.CargarPuntosYNivel(nombreDelJugador);

            // Asignar los datos cargados
            puntos = cargadosPuntos;
            nivel = cargadosNivel > 0 ? cargadosNivel : 1;

            tiempoNivel = 100f;
            databaseAccess.jugando = true;

            // Cargar la escena correspondiente al nivel actual
            CargarEscenaCorrespondiente();
        }
        else
        {
            Debug.LogError("No se encontró el script DataBaseAccess.");
        }
    }

    void Update()
    {
        if (nivelActivo && tiempoNivel > 0)
        {
            tiempoNivel -= Time.deltaTime;
        }
        else if (nivelActivo && tiempoNivel <= 0)
        {
            tiempoNivel = 0;
            Debug.Log("Tiempo agotado. Nivel completado.");
            nivelActivo = false;
            CompletarNivel();
        }

        Debug.Log($"Puntos: {puntos}, Tiempo restante: {tiempoNivel:F1} segundos");
    }

    public void RegistrarEnemigoEliminado()
    {
        if (nivelActivo)
        {
            puntos += puntosPorEnemigo;
        }
    }

    public void CompletarNivel()
    {
        if (nivelActivo)
        {
            nivelActivo = false;
            nivel++; // Incrementa el nivel

            if (databaseAccess != null)
            {
                databaseAccess.jugando = false;
                GuardarProgreso(); // Guarda los datos del progreso
                CargarSeleccionDeNivel(); // Cambia de nivel después de guardar
            }
        }
    }

    public void CompletarNivelPorColision()
    {
        CompletarNivel();
    }

    private void CargarSeleccionDeNivel()
    {
        // Carga la escena de "Seleccionar Nivel"
        SceneManager.LoadScene("SelectorNiveles");
    }

    private void GuardarProgreso()
    {
        if (databaseAccess != null)
        {
            databaseAccess.GuardarDatos(nombreDelJugador, puntos, nivel, tiempoNivel);
            Debug.Log($"Progreso guardado: Puntos: {puntos}, Nivel: {nivel}, Tiempo restante: {tiempoNivel:F1}");
        }
        else
        {
            Debug.LogError("No se encontró el script DataBaseAccess.");
        }
    }

    private void CargarEscenaCorrespondiente()
    {
        // Determina la escena que se debe cargar según el nivel guardado
        string escenaObjetivo = "Nivel" + nivel;
        string escenaActual = SceneManager.GetActiveScene().name;

        if (escenaActual != escenaObjetivo)
        {
            Debug.Log($"Cargando escena del nivel guardado: {escenaObjetivo}");
            SceneManager.LoadScene(escenaObjetivo);
        }
        else
        {
            Debug.Log("Ya estás en la escena correcta.");
        }
    }

    public void ReiniciarNivel()
{
    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
}

}
