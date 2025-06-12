using MongoDB.Bson;
using MongoDB.Driver;
using UnityEngine;

public class DataBaseAccess : MonoBehaviour
{
    private MongoClient client;
    private IMongoDatabase database;
    private IMongoCollection<BsonDocument> collection;
    public string nombreJugador;
    public bool jugando = false;

    // Se ejecuta al inicio
    void Start()
    {
        // Cargar el nombre del jugador desde PlayerPrefs
        nombreJugador = PlayerPrefs.GetString("JugadorSeleccionado", "Jugador por defecto");

        if (string.IsNullOrEmpty(nombreJugador) || nombreJugador == "Jugador por defecto")
        {
            Debug.LogError("Nombre del jugador no ha sido establecido.");
            return;
        }

        // Inicializa la conexión con MongoDB
        InitializeMongoDBConnection();
    }

    // Inicializa la conexión con MongoDB
    void InitializeMongoDBConnection()
    {
        string connectionString = "mongodb://localhost:27017";
        string databaseName = "TolyMoly";
        string collectionName = "usuarios";

        try
        {
            client = new MongoClient(connectionString);
            database = client.GetDatabase(databaseName);
            var collectionNames = database.ListCollectionNames().ToList();
            if (collectionNames.Contains(collectionName))
            {
                collection = database.GetCollection<BsonDocument>(collectionName);
                Debug.Log("Conexión a MongoDB establecida correctamente.");
            }
            else
            {
                Debug.LogError($"La colección '{collectionName}' no existe.");
            }

            database.RunCommandAsync((Command<BsonDocument>)"{ping:1}").Wait();
        }
        catch (MongoException e)
        {
            Debug.LogError("Error al conectar a MongoDB: " + e.Message);
        }
    }

    // Guarda los datos del jugador en MongoDB
    public void GuardarDatos(string nombre, int puntos, int nivel, float tiempo)
    {
        if (collection == null)
        {
            Debug.LogError("La colección es nula, no se pueden guardar datos.");
            return;
        }

        var filter = Builders<BsonDocument>.Filter.Eq("nombre", nombre);
        var update = Builders<BsonDocument>.Update
            .Set("puntos", puntos)
            .Set("nivel", nivel)
            .Set("tiempo_jugado", tiempo)
            .Set("guardado_en", System.DateTime.Now.ToUniversalTime())
            .Set($"nivelesCompletados.{nivel}", true); // Marca el nivel como completado

        try
        {
            var result = collection.UpdateOne(filter, update, new UpdateOptions { IsUpsert = true });
            if (result.MatchedCount > 0)
            {
                Debug.Log($"Datos actualizados: {nombre}, Puntos: {puntos}, Nivel: {nivel}, Tiempo: {tiempo:F1}");
            }
            else
            {
                Debug.Log($"Datos insertados: {nombre}, Puntos: {puntos}, Nivel: {nivel}, Tiempo: {tiempo:F1}");
            }
        }
        catch (MongoException e)
        {
            Debug.LogError("Error al guardar datos: " + e.Message);
        }
    }

    // Carga puntos y nivel desde la base de datos
    public (int, int) CargarPuntosYNivel(string nombre)
    {
        if (collection == null)
        {
            Debug.LogWarning("La colección es nula.");
            return (0, 0);
        }

        var filter = Builders<BsonDocument>.Filter.Eq("nombre", nombre);
        try
        {
            var documento = collection.Find(filter).FirstOrDefault();

            if (documento != null)
            {
                int puntos = documento.Contains("puntos") ? documento["puntos"].AsInt32 : 0;
                int nivel = documento.Contains("nivel") ? documento["nivel"].AsInt32 : 0;

                return (puntos, nivel);
            }
            else
            {
                return (0, 0);
            }
        }
        catch (MongoException e)
        {
            Debug.LogError($"Error al cargar datos: {e.Message}");
            return (0, 0);
        }
    }
}
