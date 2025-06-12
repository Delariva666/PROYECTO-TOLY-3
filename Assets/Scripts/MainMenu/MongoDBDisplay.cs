using System;
using System.Collections.Generic;
using UnityEngine;
using MongoDB.Bson;
using MongoDB.Driver;
using UnityEngine.UI;

public class MongoDBDisplay : MonoBehaviour
{
    private MongoClient client;
    private IMongoDatabase database;
    private IMongoCollection<BsonDocument> collection;

    public GameObject rankingEntryPrefab;  // Prefab para la entrada del ranking
    public Transform contentPanel;         // El panel Content donde se añadirán las entradas

    void Start()
    {
        // Establecer conexión a MongoDB
        client = new MongoClient("mongodb://localhost:27017");
        database = client.GetDatabase("TolyMoly");
        collection = database.GetCollection<BsonDocument>("usuarios");

        // Llamar para cargar y mostrar los documentos
        DisplayDocuments();
    }

    void DisplayDocuments()
    {
        // Obtener todos los documentos de la colección
        var documentos = collection.Find(new BsonDocument()).ToList();

        // Limpiar el contenido actual del panel para evitar que se acumulen entradas
        foreach (Transform child in contentPanel)
        {
            Destroy(child.gameObject);
        }

        int index = 1; // Contador para enumerar los jugadores
        foreach (var doc in documentos)
        {
            // Instanciar una nueva entrada del ranking
            GameObject entry = Instantiate(rankingEntryPrefab, contentPanel);

            // Obtener los campos de texto del prefab (asegúrate de tener estos campos en el Prefab)
            Text[] textFields = entry.GetComponentsInChildren<Text>();

            // Asignar la información de cada documento a los campos correspondientes
            textFields[0].text = index.ToString();  // IDJugador
            textFields[1].text = doc.Contains("nombre") ? doc["nombre"].ToString() : "Desconocido";  // Nombre
            textFields[2].text = doc.Contains("puntos") ? doc["puntos"].ToString() : "0";  // Puntos
            textFields[3].text = doc.Contains("nivel") ? doc["nivel"].ToString() : "0";  // Nivel
           // Formatear tiempo_jugado en mm:ss
            if (doc.Contains("tiempo_jugado"))
            {
                double segundos = doc["tiempo_jugado"].ToDouble();
                TimeSpan t = TimeSpan.FromSeconds(segundos);
                textFields[4].text = t.ToString(@"mm\:ss");
            }
            else
            {
                textFields[4].text = "00:00";
            }

            // Incrementar el contador de jugadores
            index++;


            // Incrementar el contador de jugadores
            index++;
        }
    }
}
