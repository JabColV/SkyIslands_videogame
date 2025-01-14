using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class MainMenu : MonoBehaviour
{
    SingletonPattern singletonPattern;
    public TMP_Text nameText;
    public TMP_Text total_gems;
    public TMP_Text total_coins;
    public TMP_Text total_lifes;

    private void OnEnable()
    {
        // Se obtienen las instancias cuando el menú se activa
        singletonPattern = SingletonPattern.Instance;
        // Cargar datos del usuario cuando se active el menú
        LoadUserData(); 
    }

    private void LoadUserData()
    {
        // Asignar el nombre del usuario a un objeto Text, si FirebaseAuth.Instance.userData no es nulo
        if (singletonPattern.GetDatabase().GetDataUserInfo() != null)
        {
            // Asignar el nombre del usuario a un objeto Text
            nameText.text = "Bienvenido usuario " + singletonPattern.GetDatabase().GetDataUserInfo().name;
            // Asignar el número de gemas del usuario a un objeto Text
            total_gems.text = singletonPattern.GetDatabase().GetDataUserInfo().gemas.ToString();
            // Asignar el número de monedas del usuario a un objeto Text
            total_coins.text = singletonPattern.GetDatabase().GetDataUserInfo().totalCoins.ToString();
            // Asignar el número de vidas del usuario a un objeto Text
            total_lifes.text = singletonPattern.GetDatabase().GetDataUserInfo().vidas.ToString();
            // Restablecer tablones
            // singletonPattern.GetPlayerController().quizLogic.ActivatePlanks(singletonPattern.GetPlayerController().first_planks, singletonPattern.GetHasFirstPlanks());
             
            // if (singletonPattern.GetHasFirstPlanks())
            // {
            //     Debug.Log("Es reeee True");
            // }
            // else
            // {
            //     Debug.Log("Es reeee False");
            // }
            // singletonPattern.GetPlayerController().quizLogic.ActivatePlanks(singletonPattern.GetPlayerController().second_planks, singletonPattern.GetHasSecondPlanks());
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






