using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using TMPro;

public class FirebaseAuth : MonoBehaviour
{
    SingletonPattern singletonPattern;
    public GameObject WelcomeInterface;
    public GameObject MainInterface;
    UserData userData;

    [DllImport("__Internal")]
    public static extern void SignInWithGoogle(string objectName, string callback, string fallback);

    [System.Serializable]
    public class UserData
    {
        public string userId;
        public string userName;
    }

    private void Start()
    {
        singletonPattern = SingletonPattern.Instance;
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
        if (singletonPattern.GetDatabase() != null)
        {
            // Llama a GetData 
            singletonPattern.GetDatabase().GetData();
            // Espera hasta que los datos estén cargados
            yield return new WaitUntil(() => singletonPattern.GetDatabase().GetDataUserInfo() != null);
            // Verificar si los datos del usuario no son nulos
            if (singletonPattern.GetDatabase().GetDataUserInfo() != null){
                // Desactivar el objeto de inicio de sesión 
                WelcomeInterface.SetActive(false);
                // Activar el objeto de la escena principal
                MainInterface.SetActive(true);
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

        // Iniciar la corutina LoadData
        StartCoroutine(LoadData());
    }

    public void OnSignInFailure(string error)
    {
        Debug.LogError("Error from Unity: " + error);
    }

}


