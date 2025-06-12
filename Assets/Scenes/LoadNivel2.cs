using UnityEngine;
using UnityEngine.UI; // Para controlar los botones
using UnityEngine.SceneManagement;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Linq;

public class LoadNivel2 : MonoBehaviour
{
    public Button botonNivel2; // Asigna el botón llamado "Button (1)" desde el Inspector
    private MongoClient client;
    private IMongoDatabase database;
    private IMongoCollection<BsonDocument> collection;

    public string nombreJugador; // Nombre del jugador para consultar en la base de datos

    void Start()
    {
        // Inicializar conexión a la base de datos
        InitializeMongoDBConnection();

        // Obtener el nombre del jugador desde PlayerPrefs (sin redeclarar la variable)
        nombreJugador = PlayerPrefs.GetString("JugadorSeleccionado", "Jugador por defecto");

        // Mostrar el nombre del jugador en la consola
        Debug.Log("Jugador seleccionado: " + nombreJugador);

        // Si se cargó correctamente un nombre distinto al valor predeterminado, puedes realizar otras acciones
        if (nombreJugador != "Jugador por defecto")
        {
            Debug.Log("Nombre del jugador cargado correctamente: " + nombreJugador);
        }
        else
        {
            Debug.LogWarning("No se ha encontrado el nombre del jugador. Usando el valor por defecto.");
        }

        VerificarAccesoNivel2();
    }

    // Inicializa la conexión con MongoDB
    void InitializeMongoDBConnection()
    {
        string connectionString = "mongodb://localhost:27017"; // Conexión a MongoDB
        string databaseName = "TolyMoly"; // Nombre de la base de datos
        string collectionName = "usuarios"; // Nombre de la colección

        try
        {
            client = new MongoClient(connectionString);
            database = client.GetDatabase(databaseName);
            collection = database.GetCollection<BsonDocument>(collectionName);

            Debug.Log("Conexión a MongoDB establecida correctamente.");
        }
        catch (MongoException e)
        {
            Debug.LogError("Error al conectar a MongoDB: " + e.Message);
        }
    }

    // Verifica si el jugador puede jugar el Nivel 2
    void VerificarAccesoNivel2()
    {
        if (string.IsNullOrEmpty(nombreJugador))
        {
            Debug.LogError("Nombre del jugador no ha sido establecido.");
            botonNivel2.interactable = false;
            return;
        }

        Debug.Log($"Buscando información para el jugador: {nombreJugador}");

        // Verifica si el jugador ha completado algún nivel mayor o igual a 1
        if (collection != null)
        {
            try
            {
                var filter = Builders<BsonDocument>.Filter.Eq("nombre", nombreJugador);
                var documento = collection.Find(filter).FirstOrDefault();

                if (documento != null)
                {
                    Debug.Log($"Documento encontrado para el jugador: {documento}");

                    // Verifica si el jugador tiene niveles completados
                    if (documento.Contains("nivelesCompletados"))
                    {
                        var nivelesCompletados = documento["nivelesCompletados"].AsBsonDocument;
                        bool botonHabilitado = nivelesCompletados.Names.Any(nivel =>
                        {
                            int nivelInt;
                            if (int.TryParse(nivel, out nivelInt) && nivelInt >= 1)
                            {
                                return nivelesCompletados[nivel].AsBoolean;
                            }
                            return false;
                        });

                        botonNivel2.interactable = botonHabilitado;

                        Debug.Log(botonHabilitado
                            ? "El jugador ha completado un nivel mayor o igual a 1. Botón habilitado."
                            : "El jugador no ha completado ningún nivel mayor o igual a 1. Botón deshabilitado.");
                    }
                    else
                    {
                        Debug.LogError("El campo 'nivelesCompletados' no existe en el documento.");
                        botonNivel2.interactable = false;
                    }
                }
                else
                {
                    Debug.LogError("No se encontró al jugador en la base de datos.");
                    botonNivel2.interactable = false;
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError("Error al consultar la base de datos: " + e.Message);
                botonNivel2.interactable = false;
            }
        }
        else
        {
            Debug.LogError("No se ha inicializado la colección de la base de datos.");
            botonNivel2.interactable = false;
        }
    }

    // Este método será llamado cuando el jugador haga clic en el botón
    public void CargarNivel2()
    {
        // Aquí cargamos la escena de Nivel 2
        SceneManager.LoadScene("Nivel2");
    }
}
