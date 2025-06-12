using UnityEngine;
using UnityEngine.UI;
using MongoDB.Bson;
using MongoDB.Driver;

public class Modificar : MonoBehaviour
{
    // Referencias a los InputFields
    public InputField InputBuscarNombre;
    public InputField InputNombreActualizar;
    public InputField InputPuntosActualizar;
    public InputField InputNivelActualizar;
    public InputField InputTiempoActualizar;
    public Text FeedBackTextModificar;

    // Variables para la conexión a MongoDB
    private MongoClient client;
    private IMongoDatabase database;
    private IMongoCollection<BsonDocument> collection;

    // Documento actual (almacena los datos consultados)
    private BsonDocument documentoActual;

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
    }

    // Método para buscar un documento por nombre
    public void BuscarDocumento()
{
    try
    {
        string nombre = InputBuscarNombre.text;

        if (string.IsNullOrWhiteSpace(nombre))
        {
            FeedBackTextModificar.text = "Por favor, ingresa un nombre.";
            return;
        }

        // Filtro para buscar por nombre
        var filtro = Builders<BsonDocument>.Filter.Eq("nombre", nombre);

        // Buscar el documento
        documentoActual = collection.Find(filtro).FirstOrDefault();

        if (documentoActual != null)
        {
            // Mostrar los datos en los InputFields
            InputNombreActualizar.text = documentoActual["nombre"].ToString();
            InputNivelActualizar.text = documentoActual["nivel"].ToString();
            InputTiempoActualizar.text = documentoActual["tiempo_jugado"].ToString();

            FeedBackTextModificar.text = "Documento encontrado. Puedes modificar los datos.";
        }
        else
        {
            FeedBackTextModificar.text = $"No se encontró ningún documento con el nombre '{nombre}'.";
        }
    }
    catch (System.Exception e)
    {
        FeedBackTextModificar.text = "Error al buscar: " + e.Message;
        Debug.LogError("Error al buscar: " + e.Message);
    }
}

    // Método para modificar los datos del documento
public void ModificarDocumento()
{
    try
    {
        if (documentoActual == null)
        {
            FeedBackTextModificar.text = "Primero busca un documento antes de modificar.";
            return;
        }

        // Nuevos valores desde los InputFields
        string nuevoNombre = InputNombreActualizar.text;
        string nivelText = InputNivelActualizar.text;
        string tiempoText = InputTiempoActualizar.text;

        // Validar que los campos no estén vacíos
        if (string.IsNullOrWhiteSpace(nuevoNombre) || 
            string.IsNullOrWhiteSpace(nivelText) || 
            string.IsNullOrWhiteSpace(tiempoText))
        {
            FeedBackTextModificar.text = "Por favor, completa todos los campos.";
            return;
        }

        // Convertir los valores a los tipos correspondientes
        int oleada = int.Parse(nivelText);
        int tiempo = int.Parse(tiempoText);

        // Crear la actualización
        var actualizacion = Builders<BsonDocument>.Update
            .Set("nombre", nuevoNombre)
            .Set("nivel", oleada)
            .Set("tiempo_jugado", tiempo)
            .Set("actualizado_en", BsonDateTime.Create(System.DateTime.UtcNow));

        // Aplicar la actualización
        var resultado = collection.UpdateOne(
            Builders<BsonDocument>.Filter.Eq("_id", documentoActual["_id"]),
            actualizacion
        );

        if (resultado.ModifiedCount > 0)
        {
            FeedBackTextModificar.text = "Documento modificado correctamente.";
            Debug.Log("Documento modificado.");
        }
        else
        {
            FeedBackTextModificar.text = "No se pudo modificar el documento.";
        }
    }
    catch (System.Exception e)
    {
        FeedBackTextModificar.text = "Error al modificar: " + e.Message;
        Debug.LogError("Error al modificar: " + e.Message);
    }
}

}
