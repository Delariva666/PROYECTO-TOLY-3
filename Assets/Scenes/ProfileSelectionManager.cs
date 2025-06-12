using MongoDB.Bson;
using MongoDB.Driver;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;


public class ProfileSelectionManager : MonoBehaviour
{
    // Conexión a MongoDB
    private MongoClient client;
    private IMongoDatabase database;
    private IMongoCollection<BsonDocument> profiles;
    public GameObject PanelInsertar; 
    public GameObject PanelEliminar;    // El panel que contiene el InputField y el Button

    // Referencias a los botones de UI
    public Button button1, button2, button3;
    public Text button1Text, button2Text, button3Text;
    public InputField nameInputField; // InputField donde el usuario ingresa su nombre
    public InputField NombreDelet;    // Campo de texto para el dato adicional (por ejemplo, apellido o email)
    public GameObject createProfilePanel; // Panel para crear un perfil
    public GameObject deleteProfilePanel; // Panel para eliminar un perfil

    // Nombre de los perfiles (por ejemplo, profile1, profile2, profile3)
    private string[] profileSlots = { "profile1", "profile2", "profile3" };

    // Referencia al script LoadNivel2
    public LoadNivel2 loadNivel2Script;

    void Start()
    {
        // Conectar a MongoDB
        client = new MongoClient("mongodb://localhost:27017");  // Ajusta la URL a tu MongoDB Atlas o servidor local
        database = client.GetDatabase("TolyMoly");  // Nombre de tu base de datos
        profiles = database.GetCollection<BsonDocument>("usuarios");

        // Llamar a la función para cargar los perfiles existentes
        LoadProfiles();

        // Configurar botones
        button1.onClick.AddListener(() => ProfileButtonClicked(0));
        button2.onClick.AddListener(() => ProfileButtonClicked(1));
        button3.onClick.AddListener(() => ProfileButtonClicked(2));
    }

    // Cargar los perfiles desde la base de datos
    void LoadProfiles()
    {
        for (int i = 0; i < profileSlots.Length; i++)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("_id", profileSlots[i]);
            var profile = profiles.Find(filter).FirstOrDefault();

            if (profile != null && profile.Contains("nombre"))
            {
                // Si el perfil existe y tiene un nombre, mostrar el nombre
                string profileName = profile["nombre"].ToString();
                SetProfileButtonText(i, profileName);
            }
            else
            {
                // Si no existe un perfil o no tiene nombre, mostrar "Vacío"
                SetProfileButtonText(i, "Vacío");
            }
        }
    }

    // Función para configurar el texto de los botones de perfil
    void SetProfileButtonText(int index, string text)
    {
        switch (index)
        {
            case 0:
                button1Text.text = text;
                break;
            case 1:
                button2Text.text = text;
                break;
            case 2:
                button3Text.text = text;
                break;
        }
    }

    // Cuando un jugador hace clic en un botón de perfil
public void ProfileButtonClicked(int profileIndex)
    {
        var filter = Builders<BsonDocument>.Filter.Eq("_id", profileSlots[profileIndex]);
        var profile = profiles.Find(filter).FirstOrDefault();

        if (profile != null && profile.Contains("nombre"))
        {
            // Obtener el nombre del perfil seleccionado
            string profileName = profile["nombre"].ToString();
            Debug.Log("Perfil seleccionado: " + profileName);

            // Guardar el nombre del perfil en PlayerPrefs para que se pueda usar en otra escena
            PlayerPrefs.SetString("JugadorSeleccionado", profileName);
            PlayerPrefs.Save(); // Guardar inmediatamente

            // Confirmar que el nombre fue guardado correctamente
            Debug.Log("Nombre guardado en PlayerPrefs: " + PlayerPrefs.GetString("JugadorSeleccionado"));

            // Cargar la siguiente escena
            SceneManager.LoadScene("SelectorNiveles");
        }
        else
        {
            // Si no tiene nombre, mostrar el panel para crear uno
            ShowCreateProfilePanel(profileIndex);
        }
    }

    // Mostrar el panel para crear un perfil
    void ShowCreateProfilePanel(int profileIndex)
    {
        createProfilePanel.SetActive(true); // Activar el panel de creación
        nameInputField.text = ""; // Limpiar el campo de texto

        // Cuando el jugador ingresa el nombre y lo guarda
        Button saveButton = createProfilePanel.transform.Find("SaveButton").GetComponent<Button>();
        saveButton.onClick.AddListener(() => CreateProfile(profileIndex));
    }

    // Crear un perfil en MongoDB
public void CreateProfile(int profileIndex)
{
    if (!string.IsNullOrEmpty(nameInputField.text))
    {
        // Comprobar si el perfil ya existe en el índice seleccionado
        var filter = Builders<BsonDocument>.Filter.Eq("_id", profileSlots[profileIndex]);
        var existingProfile = profiles.Find(filter).FirstOrDefault();

        if (existingProfile == null)
        {
            // Si el perfil no existe, crear un nuevo perfil
            var newProfile = new BsonDocument
            {
                { "_id", profileSlots[profileIndex] },  // El ID del perfil
                { "nombre", nameInputField.text },      // Nombre del jugador
                { "puntos", 0 },                        // Puntos iniciales
                { "nivel", 0 },                         // Nivel inicial
                { "tiempo_jugado", 0 },                 // Tiempo jugado inicial
                { "guardado_en", DateTime.UtcNow },    // Fecha de guardado actual
                { "nivelesCompletados", new BsonDocument
                    {
                        { "0", true },  // Nivel 1 completado
                    }
                }
            };

            profiles.InsertOne(newProfile);
            Debug.Log("Perfil creado: " + nameInputField.text);
        }
        else
        {
            // Si el perfil ya existe, intentamos crear un perfil en el siguiente slot
            Debug.LogError("El perfil con ID " + profileSlots[profileIndex] + " ya existe.");

            // Intentamos con el siguiente slot
            int nextIndex = GetNextAvailableProfileSlot(profileIndex);

            if (nextIndex != -1)
            {
                // Llamamos a la función recursivamente para crear el perfil en el siguiente slot disponible
                CreateProfile(nextIndex);
            }
            else
            {
                Debug.LogError("No hay más perfiles disponibles para crear.");
            }
        }

        // Recargar los perfiles
        LoadProfiles();

        // Asignar el nombre a LoadNivel2
        loadNivel2Script.nombreJugador = nameInputField.text;
        
        // Desactivar el panel de creación
        createProfilePanel.SetActive(false);
    }
    else
    {
        Debug.LogError("Por favor ingresa un nombre válido.");
    }
}

// Función para obtener el siguiente perfil disponible
private int GetNextAvailableProfileSlot(int currentIndex)
{
    for (int i = currentIndex + 1; i < profileSlots.Length; i++)
    {
        var filter = Builders<BsonDocument>.Filter.Eq("_id", profileSlots[i]);
        var existingProfile = profiles.Find(filter).FirstOrDefault();

        if (existingProfile == null)
        {
            return i; // Retorna el índice del siguiente perfil disponible
        }
    }

    return -1; // Retorna -1 si no hay más slots disponibles
}



    // Mostrar el panel para eliminar un perfil
// Mostrar el panel para eliminar un perfil
public void ShowDeleteProfilePanel()
{
    deleteProfilePanel.SetActive(true); // Mostrar el panel de eliminación
    NombreDelet.text = ""; // Limpiar el InputField

    // Vincular el botón de confirmación al método de eliminación
    Button deleteButton = deleteProfilePanel.transform.Find("DeleteButton").GetComponent<Button>();
    deleteButton.onClick.AddListener(() => DeleteProfileByName());
}

// Eliminar un perfil basado en el nombre ingresado
public void DeleteProfileByName()
{
    string nameToDelete = NombreDelet.text.Trim(); // Obtener y limpiar el nombre ingresado

    if (!string.IsNullOrEmpty(nameToDelete))
    {
        // Filtro para buscar el perfil con el nombre especificado
        var filter = Builders<BsonDocument>.Filter.Eq("nombre", nameToDelete);

        // Verificar si existe un perfil con ese nombre
        var profileToDelete = profiles.Find(filter).FirstOrDefault();
        if (profileToDelete != null)
        {
            // Eliminar el perfil completo, incluyendo el _id
            profiles.DeleteOne(filter);
            Debug.Log("Perfil eliminado completamente: " + nameToDelete);

            // Recargar perfiles para actualizar la interfaz
            LoadProfiles();

            // Cerrar el panel de eliminación
            deleteProfilePanel.SetActive(false);
        }
        else
        {
            Debug.LogError("No se encontró un perfil con el nombre: " + nameToDelete);
        }
    }
    else
    {
        Debug.LogError("Por favor ingresa un nombre válido para eliminar.");
    }
}
    public void CerrarPanel(){
        PanelEliminar.SetActive(false);
        PanelInsertar.SetActive(false);

    }


}
