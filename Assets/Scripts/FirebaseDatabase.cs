using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class FirebaseDatabase : MonoBehaviour
{
    StoredUserData dataUser;
    SingletonPattern singletonPattern;

    [DllImport("__Internal")]
    private static extern void GetJSON(string path, string objectName, string callback, string fallback);

    [DllImport("__Internal")]
    private static extern void PostJSON(string path, string value, string objectName, string callback, string fallback);
    
    [DllImport("__Internal")]
    private static extern void UpdateJSON(string path, string value, string objectName, string callback, string fallback);

    [System.Serializable]
    public class StoredUserData
    {
        public string id;
        public string name;
        public int totalCoins;
        public int vidas;
        public int gemas;
        public bool hasGoggles;
        public bool win;
        // public bool hasFirstPlanks;
        // public bool hasSecondPlanks;
        public Vector3 position;

        // public StoredUserData(string id, string name, int totalCoins, int vidas, int gemas, bool hasGoggles, bool win, bool hasFirstPlanks, bool hasSecondPlanks, Vector3 position)
        public StoredUserData(string id, string name, int totalCoins, int vidas, int gemas, bool hasGoggles, bool win, Vector3 position)
        {
            this.id = id;
            this.name = name;
            this.totalCoins = totalCoins;
            this.vidas = vidas;
            this.gemas = gemas;
            this.hasGoggles = hasGoggles;
            this.win = win;
            // this.hasFirstPlanks = hasFirstPlanks;
            // this.hasSecondPlanks = hasSecondPlanks;
            this.position = position;
        }
    }

    public StoredUserData GetDataUserInfo()
    {
        return dataUser;
    }

    private void Start()
    {
        singletonPattern = SingletonPattern.Instance;
    }

    public void ClearData()
    {
        dataUser = null;
    }

    public void GetData()
    {
        if (singletonPattern.GetFirebaseAuth().GetUserData() != null)
        {
            // Crear ruta para obtener los datos del usuario
            string path = "users/" + singletonPattern.GetFirebaseAuth().GetUserData().userId;
            // Llamar a la función GetJSON para obtener los datos del usuario
            GetJSON(path, gameObject.name, "OnSaveSuccessGet", "OnSaveErrorGet");
        }
        else
        {
            Debug.LogError("UserData is null");
        }
    }

    public void CreateData()
    {
        if (singletonPattern.GetFirebaseAuth().GetUserData() != null)
        {
            // Crear una posición inicial para el jugador
            Vector3 initialposition = new Vector3(-3.700000047683716f, 21.304550170898438f, 171.6999969482422f);
            // Crear un nuevo objeto StoredUserData para la serialización
            // dataUser = new StoredUserData(singletonPattern.GetFirebaseAuth().GetUserData().userId, singletonPattern.GetFirebaseAuth().GetUserData().userName, 0, 3, 0, false, false, false, false, initialposition);
            dataUser = new StoredUserData(singletonPattern.GetFirebaseAuth().GetUserData().userId, singletonPattern.GetFirebaseAuth().GetUserData().userName, 0, 3, 0, false, false, initialposition);
            // Crear la ruta para crear los datos del usuario
            string path = "users/" + dataUser.id;
            // Convertir el objeto a una cadena JSON usando JsonUtility
            string value = JsonUtility.ToJson(dataUser);
            // Llamar a la función PostJSON para crear los datos del usuario
            PostJSON(path, value, gameObject.name, "OnSaveSuccessCreate", "OnSaveErrorCreate");
        }
        else
        {
            Debug.LogError("UserData is null");
        }
    }


    public void UpdateData(Vector3 position)
    {
        if (singletonPattern.GetFirebaseAuth().GetUserData() != null && dataUser != null)
        {
            // Sumar las nuevas monedas a las existentes
            int totalCoins = singletonPattern.GetCoins(); 
            // Sumar las nuevas gemas a las existentes
            int totalGems = singletonPattern.GetGems(); 
            // Verificar si el jugador tiene las gafas
            bool hasGoggles = singletonPattern.GetHasGoggles();
            // Verificar si el jugador ha ganado
            bool win = singletonPattern.GetWin();
            // Sumar las restar las vidas
            int totalVidas = singletonPattern.GetLifes();
            // Actualizar los tablones de madera
            // bool hasFP = singletonPattern.GetHasFirstPlanks();
            // bool hasSP = singletonPattern.GetHasSecondPlanks();
            if (position == Vector3.zero)
            {
                position = singletonPattern.GetPlayer().transform.position;
            }
            // Crear un nuevo objeto StoredUserData para la serialización
            // var data = new StoredUserData(singletonPattern.GetFirebaseAuth().GetUserData().userId, singletonPattern.GetFirebaseAuth().GetUserData().userName, totalCoins, totalVidas, totalGems, hasGoggles, win, hasFP, hasSP, position);
            var data = new StoredUserData(singletonPattern.GetFirebaseAuth().GetUserData().userId, singletonPattern.GetFirebaseAuth().GetUserData().userName, totalCoins, totalVidas, totalGems, hasGoggles, win, position);
            // Crear la ruta para actualizar los datos del usuario
            string path = "users/" + data.id;
            // Convertir el objeto a una cadena JSON usando JsonUtility
            string value = JsonUtility.ToJson(data);
            // Llamar a la función UpdateJSON para actualizar los datos del usuario
            UpdateJSON(path, value, gameObject.name, "OnSaveSuccessUpdate", "OnSaveErrorUpdate");
        }
        else
        {
            Debug.LogError("userData is null or dataUser is null");
        }
    }

    void OnSaveSuccessGet(string userDataJson)
    {
        // Verificar si los datos del usuario no son nulos
        if (string.IsNullOrEmpty(userDataJson) || userDataJson == "null")
        {
            // Si los datos del usuario son nulos, crear los datos del usuario
            CreateData();
        }
        else
        {
            // Si los datos del usuario no son nulos, asignar los datos a la variable dataUser
            dataUser = JsonUtility.FromJson<StoredUserData>(userDataJson);
            singletonPattern.SetIsLoaded(true);
        }
    }

    void OnSaveErrorGet(string error)
    {
        Debug.Log("Ha ocurrido un error al tratar de obtener los datos del usuario " + error);
    }

    void OnSaveSuccessCreate(string message)
    {
        Debug.Log("Desde OnSaveSuccessCreate "+ message);
    }

    void OnSaveErrorCreate(string error)
    {
        Debug.LogError(error);
    }

    void OnSaveSuccessUpdate(string message)
    {
        Debug.Log("Desde OnSaveSuccessUpdate "+ message);
    }

    void OnSaveErrorUpdate(string error)
    {
        Debug.Log("Hey! An error has occured " + error);
    }
}
