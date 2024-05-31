using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class FirebaseDatabase : MonoBehaviour
{
    FirebaseAuth firebaseAuth;
    SystemPickingUp systemPickingUp;
    public GameObject player;
    StoredUserData dataUser;
    public static FirebaseDatabase Instance;

    [DllImport("__Internal")]
    private static extern void PostJSON(string path, string value, string objectName, string callback, string fallback);
    
    [DllImport("__Internal")]
    private static extern void GetJSON(string path, string objectName, string callback, string fallback);

    [System.Serializable]
    public class StoredUserData
    {
        public string id;
        public string name;
        public int totalCoins;
        public int vidas;
        public Vector3 position;

        public StoredUserData(string id, string name, int totalCoins, int vidas, Vector3 position)
        {
            this.id = id;
            this.name = name;
            this.totalCoins = totalCoins;
            this.vidas = vidas;
            this.position = position;
        }
    }

    public StoredUserData GetDataUserInfo (){
        return dataUser;
    }

    private void Awake()
    {
        // Verificar si ya existe una instancia de FirebaseAuth
        if (Instance == null)
        {
            // Si no existe, asignar esta instancia a la variable Instance 
            Instance = this;
            // y no destruir el objeto al cargar una nueva escena
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // Si ya existe una instancia de FirebaseAuth, destruir este objeto
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        firebaseAuth = FirebaseAuth.Instance;
        systemPickingUp = SystemPickingUp.Instance;
    }

    public void SaveData()
    {
        if (firebaseAuth.GetUserData() != null && dataUser != null)
        {
            // Sumar las nuevas monedas a las existentes
            int totalCoins = dataUser.totalCoins + systemPickingUp.coins;   

            // Sumar las restar las vidas
            int totalVidas = dataUser.vidas;

            // Crear un nuevo objeto StoredUserData para la serialización
            var data = new StoredUserData(firebaseAuth.GetUserData().userId, firebaseAuth.GetUserData().userName, totalCoins, totalVidas, player.transform.position);

            string path = "users/" + data.id;

            // Convertir el objeto a una cadena JSON usando JsonUtility
            string value = JsonUtility.ToJson(data);

            PostJSON(path, value, gameObject.name, "OnSaveSuccessPost", "OnSaveErrorPost");
        }
        else
        {
            Debug.LogError("UserData is null");
        }
    }

    public void GetData()
    {
        if (firebaseAuth.GetUserData() != null)
        {
            // Crear ruta para obtener los datos del usuario
            string path = "users/" + firebaseAuth.GetUserData().userId;
            // Llamar a la función GetJSON para obtener los datos del usuario
            GetJSON(path, gameObject.name, "OnSaveSuccessGet", "OnSaveErrorGet");
        }
        else
        {
            Debug.LogError("UserData is null");
        }
    }

    void OnSaveSuccessPost(string message)
    {
        Debug.Log("Desde OnSaveSuccessPost "+message);
    }

    void OnSaveErrorPost(string error)
    {
        Debug.LogError(error);
    }

    void OnSaveSuccessGet(string userDataJson)
    {
        if (userDataJson == null)
        {
            dataUser = new StoredUserData(firebaseAuth.GetUserData().userId, firebaseAuth.GetUserData().userName, 0, 0, new Vector3(0, 0, 0));
        }
        Debug.Log("Desde OnSaveSuccessGet "+ userDataJson);
        dataUser = JsonUtility.FromJson<StoredUserData>(userDataJson);
        Debug.Log("database - Data loaded - coins " + dataUser.totalCoins);
        Debug.Log("database - Data loaded - name " + dataUser.name);
        Debug.Log("database - Data loaded - lives " + dataUser.vidas);
    }

    void OnSaveErrorGet(string error)
    {
        Debug.Log("Ha ocurrido un error al tratar de obtener los datos del usuario " + error);
    }
}
