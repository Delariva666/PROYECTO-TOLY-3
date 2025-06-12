using UnityEngine;
using UnityEngine.UI; // Para manejar InputField y Text
using MongoDB.Bson;
using MongoDB.Driver;

public class Eliminar : MonoBehaviour
{
    // Referencias al InputField para ingresar el nombre y al Text para feedback
    public InputField nombreInputEliminar;
    public Text feedbackTextEliminar;

    // Variables para la conexión a MongoDB
    private MongoClient client;
    private IMongoDatabase database;
    private IMongoCollection<BsonDocument> collection;

    void Start()
    {
        InitializeMongoDBConnection();
    }

    void InitializeMongoDBConnection()
    {
        string connectionString = "mongodb://localhost:27017";
        string databaseName = "TolyMoly";
        string collectionName = "usuarios";

        try
        {
            // Inicializar conexión a MongoDB
            client = new MongoClient(connectionString);
            database = client.GetDatabase(databaseName);
            collection = database.GetCollection<BsonDocument>(collectionName);

            // Prueba de conexión
            database.RunCommandAsync((Command<BsonDocument>)"{ping:1}").Wait();
            Debug.Log("Conexión a MongoDB establecida correctamente.");
        }
        catch (MongoException e)
        {
            Debug.LogError("Error al conectar a MongoDB: " + e.Message);
        }
        catch (System.AggregateException e)
        {
            Debug.LogError("Error al verificar conexión a MongoDB: " + e.InnerException?.Message);
        }
    }

    // Método que se llamará desde el botón
    public void EliminarDocumentos()
    {
        try
        {
            // Validar que las referencias no sean nulas
            if (nombreInputEliminar == null || feedbackTextEliminar == null)
            {
                Debug.LogError("Las referencias de los objetos no están asignadas en el Inspector.");
                return;
            }

            if (collection == null)
            {
                feedbackTextEliminar.text = "La conexión a la base de datos no está inicializada.";
                Debug.LogError("La colección no está inicializada. Revisa la conexión a MongoDB.");
                return;
            }

            // Validar que el campo de texto no esté vacío
            if (string.IsNullOrWhiteSpace(nombreInputEliminar.text))
            {
                feedbackTextEliminar.text = "Por favor, ingresa un nombre para eliminar.";
                return;
            }

            // Capturar el nombre del InputField
            string nombre = nombreInputEliminar.text;

            // Construir el filtro para buscar los documentos
            var filtro = Builders<BsonDocument>.Filter.Eq("nombre", nombre);

            // Intentar eliminar los documentos
            var resultado = collection.DeleteMany(filtro);

            if (resultado.DeletedCount > 0)
            {
                feedbackTextEliminar.text = $"Se eliminaron {resultado.DeletedCount} documentos con el nombre '{nombre}'.";
                Debug.Log($"Documentos eliminados: {resultado.DeletedCount} con nombre: {nombre}");
            }
            else
            {
                feedbackTextEliminar.text = $"No se encontró ningún documento con el nombre '{nombre}'.";
                Debug.Log($"No se encontró documento con nombre: {nombre}");
            }

            // Limpiar el campo de entrada
            nombreInputEliminar.text = "";
        }
        catch (System.Exception e)
        {
            feedbackTextEliminar.text = "Error al intentar eliminar: " + e.Message;
            Debug.LogError("Error al eliminar: " + e.Message);
        }
    }
}
