using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class MainMenu : MonoBehaviour
{
    SingletonPattern singletonPattern;
    public TMP_Text nameText;
    public TMP_Text scoreText;

    // It is executed when the object which has the script is activated
    private void Start()
    {
        // Se obtienen las instancias cuando el menú se activa
        singletonPattern = SingletonPattern.Instance;
        // Carga de datos en la interfaz
        LoadUserData();
    }

    private void LoadUserData()
    {
        // Asignar el nombre del usuario a un objeto Text, si FirebaseAuth.Instance.userData no es nulo
        if (singletonPattern.GetDatabase().GetDataUserInfo() != null)
        {
            // Asignar la cantidad de monedas recogidas a un objeto Text
            scoreText.text = singletonPattern.GetDatabase().GetDataUserInfo().totalCoins.ToString();
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
    }
}






