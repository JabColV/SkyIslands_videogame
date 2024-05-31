using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class MainMenu : MonoBehaviour
{
    FirebaseDatabase database;
    SystemPickingUp systemPickingUp;
    FirebaseAuth firebaseAuth;
    public TMP_Text nameText;
    public TMP_Text scoreText;

    // It is executed when the object which has the script is activated
    private void Start()
    {
        Debug.Log("MainMenu Start");
        // Se obtiene la instancia de FirebaseAuth
        firebaseAuth = FirebaseAuth.Instance;
        // Se obtienen las instancias cuando el menú se activa
        database = FirebaseDatabase.Instance;
        // Se obtiene la instancia de SystemPickingUp
        systemPickingUp = SystemPickingUp.Instance;
        // Carga de datos en la interfaz
        LoadUserData();
    }

    private void LoadUserData()
    {
        // Asignar el nombre del usuario a un objeto Text, si FirebaseAuth.Instance.userData no es nulo
        if (database.dataUser != null)
        {
            // Asignar la cantidad de monedas recogidas a un objeto Text
            scoreText.text = database.dataUser.totalCoins.ToString();
            nameText.text = "Bienvenido usuario " + database.dataUser.name;
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





// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.SceneManagement;
// using TMPro;

// public class MainMenu : MonoBehaviour
// {
//     FirebaseDatabase database;
//     SystemPickingUp systemPickingUp;
//     public TMP_Text nameText;
//     public TMP_Text scoreText;

    
//     void Start()
//     {
//         database = FirebaseDatabase.Instance;
//         systemPickingUp = SystemPickingUp.Instance;
//     }


//     private IEnumerator LoadUserData()
//     {
//         // Llama a GetData y espera hasta que termine
//         database.GetData();
        
//         // Espera hasta que los datos estén cargados
//         yield return new WaitUntil(() => database.getData != null);

//         // Asignar la cantidad de monedas recogidas a un objeto Text
//         scoreText.text = database.getData.totalCoins.ToString();
        
//         // Asignar el nombre del usuario a un objeto Text
//         nameText.text = "Bienvenido usuario " + userData.userName;
//     }

//     public void LoadScene()
//     {
//         SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
//     }
//     // public void LoadSceneInterface()
//     // {
//     //     SceneManager.LoadScene("InitialMenu");
//     // }
// }
