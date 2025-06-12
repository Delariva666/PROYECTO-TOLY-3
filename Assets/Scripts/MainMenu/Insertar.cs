using UnityEngine;
using UnityEngine.UI; // Para manejar InputField y Text
using MongoDB.Bson;
using MongoDB.Driver;

public class Insertar : MonoBehaviour
{
    // Referencias a los campos del Canvas
    public InputField nombreInput;
    public InputField puntosInput;
    public InputField nivelInput;
    public InputField tiempoInput;
    public Text feedbackText;

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
            Debug.LogError("Error al verificar conexión a MongoDB: " + e.InnerException.Message);
        }
    }

    // Método que se llamará desde el botón
    public void InsertarDocumento()
    {
        try
        {
            // Validar los datos del formulario
            if (string.IsNullOrWhiteSpace(nombreInput.text) || 
                string.IsNullOrWhiteSpace(puntosInput.text) || 
                string.IsNullOrWhiteSpace(nivelInput.text) || 
                string.IsNullOrWhiteSpace(tiempoInput.text))
            {
                feedbackText.text = "Por favor, completa todos los campos.";
                return;
            }

            // Capturar los datos del formulario
            string nombre = nombreInput.text;
            int puntos = int.Parse(puntosInput.text);
            int nivel = int.Parse(nivelInput.text);
            float tiempo = float.Parse(tiempoInput.text);

            // Verificar si el nombre ya existe en la base de datos
            var filter = Builders<BsonDocument>.Filter.Eq("nombre", nombre);
            var existingDocument = collection.Find(filter).FirstOrDefault();

            if (existingDocument != null)
            {
                feedbackText.text = "El nombre ya está registrado. Por favor elige otro.";
                return;
            }

            // Crear el documento
            var document = new BsonDocument
            {
                { "_id", ObjectId.GenerateNewId() },
                { "nombre", nombre },
                { "puntos", puntos },
                { "nivel", nivel },
                { "tiempo_jugado", tiempo },
                { "guardado_en", System.DateTime.Now.ToUniversalTime() }
            };

            // Insertar en la colección
            collection.InsertOne(document);

            // Mostrar retroalimentación
            feedbackText.text = "Documento insertado correctamente.";
            Debug.Log("Documento insertado: " + document.ToString());

            // Limpiar los campos del formulario
            nombreInput.text = "";
            puntosInput.text = "";
            nivelInput.text = "";
            tiempoInput.text = "";
        }
        catch (System.Exception e)
        {
            feedbackText.text = "Error al insertar los datos: " + e.Message;
            Debug.LogError("Error al insertar: " + e.Message);
        }
    }
}