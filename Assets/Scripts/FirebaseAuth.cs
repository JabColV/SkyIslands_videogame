using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using TMPro;

public class FirebaseAuth : MonoBehaviour
{
    FirebaseDatabase database;
    public GameObject WelcomeInterface;
    public GameObject MainInterface;
    UserData userData;
    public static FirebaseAuth Instance;

    [DllImport("__Internal")]
    public static extern void SignInWithGoogle(string objectName, string callback, string fallback);

    [System.Serializable]
    public class UserData
    {
        public string userId;
        public string userName;
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
        database = FirebaseDatabase.Instance;

        if (database == null)
        {
            Debug.LogError("FirebaseDatabase no está inicializado correctamente.");
            return;
        }

        if (WelcomeInterface == null)
        {
            Debug.LogError("WelcomeInterface no está asignado.");
        }

        if (MainInterface == null)
        {
            Debug.LogError("MainInterface no está asignado.");
        }
    }

    public UserData GetUserData(){
        return userData;
    }

    public void SignIn()
    {
        SignInWithGoogle(gameObject.name, "OnSignInSuccess", "OnSignInFailure");
    }

    public IEnumerator LoadData()
    {
        if (database != null)
        {
            // Llama a GetData 
            database.GetData();
            Debug.Log("Getting data...");
            
            // Espera hasta que los datos estén cargados
            yield return new WaitUntil(() => database.GetDataUserInfo() != null);

            if (database.GetDataUserInfo() != null){
                // Desactivar el objeto de inicio de sesión 
                WelcomeInterface.SetActive(false);
                // Activar el objeto de la escena principal
                MainInterface.SetActive(true);

                Debug.Log("Data loaded - coins " + database.GetDataUserInfo().totalCoins);
                Debug.Log("Data loaded - name " + database.GetDataUserInfo().name);
                Debug.Log("Data loaded - lives " + database.GetDataUserInfo().vidas);
            }
        }
        else
        {
            Debug.LogError("FirebaseDatabase es nulo en LoadData.");
        }
    }

    public void OnSignInSuccess(string userDataJson)
    {
        // Analizar el objeto JSON para obtener el nombre y el ID del usuario
        userData = JsonUtility.FromJson<UserData>(userDataJson);
        Debug.Log("userData - User ID: " + userData.userId + " User Name: " + userData.userName);

        // Iniciar la corutina LoadData
        StartCoroutine(LoadData());
    }

    public void OnSignInFailure(string error)
    {
        Debug.LogError("Error from Unity: " + error);
    }

}


