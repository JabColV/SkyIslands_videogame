using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class MainMenu : MonoBehaviour
{
    SingletonPattern singletonPattern;
    public TMP_Text nameText;
    public TMP_Text scoreText;

    private void OnEnable()
    {
        // Se obtienen las instancias cuando el menú se activa
        singletonPattern = SingletonPattern.Instance;
        // Cargar datos del usuario cuando se active el menú
        LoadUserData(); 
    }

    // private void Start()
    // {
    //     // Reproducir la canción de fondo
    //     singletonPattern.PlayBackgroundMusic(singletonPattern.GetGameAudioSong());
    // }

    private void LoadUserData()
    {
        // Asignar el nombre del usuario a un objeto Text, si FirebaseAuth.Instance.userData no es nulo
        if (singletonPattern.GetDatabase().GetDataUserInfo() != null)
        {
            // Asignar la cantidad de monedas recogidas a un objeto Text
            scoreText.text = singletonPattern.GetDatabase().GetDataUserInfo().totalCoins.ToString();
            // Asignar el nombre del usuario a un objeto Text
            nameText.text = "Bienvenido usuario " + singletonPattern.GetDatabase().GetDataUserInfo().name;
        }
        else
        {
            Debug.LogError("UserData is null");
        }
    }

    public void LoadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Time.timeScale = 1f;
    }
}






