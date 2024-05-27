using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using TMPro;

public class FirebaseAuth : MonoBehaviour
{
    public TMP_Text nameText;
    public GameObject objToDeactivate;
    public GameObject objToActivate;
    public UserData userData;

    [DllImport("__Internal")]
    public static extern void SignInWithGoogle(string objectName, string callback, string fallback);

    [System.Serializable]
    public class UserData
    {
        public string userId;
        public string userName;
    }

    public void SignIn()
    {
        SignInWithGoogle(gameObject.name, "OnSignInSuccess", "OnSignInFailure");
    }

    public void OnSignInSuccess(string userDataJson)
    {
        // Analizar el objeto JSON para obtener el nombre y el ID del usuario
        userData = JsonUtility.FromJson<UserData>(userDataJson);

        // Asignar el nombre del usuario a un objeto Text
        nameText.text = "Bienvenido usuario " + userData.userName;

        // Desactivar el objeto de inicio de sesión y activar el objeto de la aplicación.
        objToDeactivate.SetActive(false);
        objToActivate.SetActive(true);
    }

    public void OnSignInFailure(string error)
    {
        Debug.LogError("Error from Unity: " + error);
        // Aquí se puede agregar el código para manejar el error de inicio de sesión.
    }

}


