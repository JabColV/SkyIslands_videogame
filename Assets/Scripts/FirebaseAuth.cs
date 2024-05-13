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

    [DllImport("__Internal")]
    public static extern void SignInWithGoogle(string objectName, string callback, string fallback);

    public void SignIn()
    {
        Debug.Log("Sign in with Google" + gameObject.name);
        SignInWithGoogle(gameObject.name, "OnSignInSuccess", "OnSignInFailure");
    }

    public void OnSignInSuccess(string name)
    {
        Debug.Log("Success: name: " + name);
        nameText.text = "Bienvenido usuario " + name;
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

