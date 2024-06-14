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

    public StoredUserData GetDataUserInfo(){
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
            // Crear un nuevo objeto StoredUserData para la serialización
            dataUser = new StoredUserData(singletonPattern.GetFirebaseAuth().GetUserData().userId, singletonPattern.GetFirebaseAuth().GetUserData().userName, 0, 3, new Vector3(0, 0, 0));
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


    public void UpdateData()
    {
        if (singletonPattern.GetFirebaseAuth().GetUserData() != null && dataUser != null)
        {
            // Sumar las nuevas monedas a las existentes
            int totalCoins = dataUser.totalCoins + singletonPattern.GetCoins(); 
            // Sumar las restar las vidas
            int totalVidas = dataUser.vidas;
            Vector3 position;
            if (singletonPattern.GetPlayer() != null)
            {
                // Obtener la posición actual del jugador
                position = singletonPattern.GetPlayer().transform.position;
            }
            else
            {
                // Si el jugador es nulo, asignar una posición por defecto
                position = new Vector3(-3.7f, 21.30455f, 171.7f);
            }
            // Crear un nuevo objeto StoredUserData para la serialización
            var data = new StoredUserData(singletonPattern.GetFirebaseAuth().GetUserData().userId, singletonPattern.GetFirebaseAuth().GetUserData().userName, totalCoins, totalVidas, position);
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
        Debug.Log("Desde OnSaveSuccessGet "+ userDataJson);
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
