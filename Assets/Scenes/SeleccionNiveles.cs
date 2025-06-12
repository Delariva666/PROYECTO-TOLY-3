using UnityEngine;
using UnityEngine.SceneManagement;
using MongoDB.Driver;
using MongoDB.Bson;

public class ProgresoNivel : MonoBehaviour
{
    private MongoClient client;
    private IMongoDatabase database;
    private IMongoCollection<BsonDocument> collection;

    public string nombreJugador;
    public int nivelActual;
    public int puntosGanados;

    void Start()
    {
        InitializeMongoDBConnection();
    }

    void InitializeMongoDBConnection()
    {
        string connectionString = "mongodb://localhost:27017";
        string databaseName = "TolyMoly";
        string collectionName = "usuarios";

        client = new MongoClient(connectionString);
        database = client.GetDatabase(databaseName);
        collection = database.GetCollection<BsonDocument>(collectionName);
    }

    public void CompletarNivel()
    {
        var filter = Builders<BsonDocument>.Filter.Eq("nombre", nombreJugador);
        var update = Builders<BsonDocument>.Update
            .Set($"nivelesCompletados.{nivelActual}", true)
            .Inc("puntosTotales", puntosGanados);

        collection.UpdateOne(filter, update, new UpdateOptions { IsUpsert = true });
        Debug.Log($"Nivel {nivelActual} completado con {puntosGanados} puntos.");

        SceneManager.LoadScene("SeleccionNivel");
    }
}
