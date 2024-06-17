using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using TMPro;

public class FirebaseAuth : MonoBehaviour
{
    SingletonPattern singletonPattern;
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
    }

    public UserData GetUserData(){
        return userData;
    }

    public void ClearData()
    {
        userData = null;
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
            if (singletonPattern.GetDatabase().GetDataUserInfo() != null)
            {
                // Desactivar el objeto de inicio de sesión 
                singletonPattern.GetWelcomeInterface().SetActive(false);
                // Activar el objeto de la escena principal
                singletonPattern.GetMainInterface().SetActive(true);
                // Reproducir la canción de fondo
                singletonPattern.PlayBackgroundMusic(singletonPattern.GetGameAudioSong());
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


