using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class FirebaseDatabase : MonoBehaviour
{
    public FirebaseAuth firebaseAuth;
    StoredUserData data;

    [DllImport("__Internal")]
    private static extern void PostJSON(string path, string value, string objectName, string callback, string fallback);

    [System.Serializable]
    public class StoredUserData
    {
        public string id;
        public string name;
        public int totalCoins;
        public int vidas;

        public StoredUserData(string id, string name, int totalCoins, int vidas)
        {
            this.id = id;
            this.name = name;
            this.totalCoins = totalCoins;
            this.vidas = vidas;
        }
    }

    public void SaveData()
    {
        if (firebaseAuth.userData != null)
        {
            // Crear un nuevo objeto StoredUserData para la serialización
            var data = new StoredUserData(firebaseAuth.userData.userId, firebaseAuth.userData.userName, 0, 0);

            string path = "users/" + data.id;

            // Convertir el objeto a una cadena JSON usando JsonUtility
            string value = JsonUtility.ToJson(data);

            PostJSON(path, value, gameObject.name, "OnSaveSuccess", "OnSaveError");
        }
        else
        {
            Debug.LogError("UserData is null");
        }
    }

    void OnSaveSuccess(string message)
    {
        Debug.Log(message);
    }

    void OnSaveError(string error)
    {
        Debug.LogError(error);
    }
}
