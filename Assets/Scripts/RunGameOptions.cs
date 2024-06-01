using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class RunGameOptions : MonoBehaviour
{
    public void LoadSceneMain()
    {
        SceneManager.LoadScene("InitialMenu");
        // firebaseAuth.WelcomeInterface.SetActive(false);
        // firebaseAuth.MainInterface.SetActive(true);
    }
}
