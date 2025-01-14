using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MenuPause : MonoBehaviour
{
    [SerializeField] private GameObject pauseButton;
    [SerializeField] private GameObject Menu;
    public PlayerController playerController;
    public Collisions collisions;
    // To store the singleton pattern instance
    SingletonPattern singletonPattern;

    // Start is called before the first frame update
    public void Start()
    {
        singletonPattern = SingletonPattern.Instance;
        if (singletonPattern == null)
        {
            Debug.LogError("singletonPattern no está inicializado.");
        }
    }

    // =========================================================================================================

    // Pause the game
    public void Pause()
    {
        Time.timeScale = 0f;
        pauseButton.SetActive(false);
        Menu.SetActive(true);
    }

    // =========================================================================================================

    // Resume the game
    public void Resume()
    {
        Time.timeScale = 1f;
        pauseButton.SetActive(true);
        Menu.SetActive(false);
    }

    // =========================================================================================================

    public void LastMemory()
    {
        StartCoroutine(ReloadData("FirstLevel", false));
    }

    // =========================================================================================================
    // Restart the game
    public void Restart()
    {
        Time.timeScale = 1f;
        // Restaurar el estado de las gemas
        playerController.DiamondDeactivation();
        // Suscribirse al evento sceneLoaded antes de cargar la escena
        SceneManager.sceneLoaded += SceneRestart;
        // Restaurar la escena actual
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void SceneRestart(Scene scene, LoadSceneMode mode)
    {
        // Restaurar las vidas del jugador
        singletonPattern.SetLifes(3);
        // Restaurar las gemas del jugador
        singletonPattern.SetGems(0);
        // Restaurar las monedas del jugador
        singletonPattern.SetCoins(0);
        // Restaurar las gafas del jugador
        singletonPattern.SetHasGoggles(false);
        // Restaurar el estado gana o pierde
        singletonPattern.SetWin(false);
        // Restaurar los tabloes de las escaleras
        singletonPattern.SetHasFirstPlanks(false);
        singletonPattern.SetHasSecondPlanks(false);
        // Actualizar los datos del usuario
        singletonPattern.GetDatabase().UpdateData(new Vector3(-3.700000047683716f, 21.304550170898438f, 171.6999969482422f));
        // Desuscribirse del evento sceneLoaded para evitar múltiples suscripciones
        SceneManager.sceneLoaded -= SceneRestart;
    }

    // =========================================================================================================

    // Load game scene
    public void LoadSceneMain()
    {
        // Iniciar la corutina ReloadData
        StartCoroutine(ReloadData("InitialMenu", true));
    }

    public IEnumerator ReloadData(string sceneName, bool sceneLoaded)
    {
        // Setear el estado de carga de datos
        singletonPattern.SetIsLoaded(false);
        // Obtener los datos del usuario
        singletonPattern.GetDatabase().GetData();

        // Esperar a que los datos se carguen completamente
        yield return new WaitUntil(() => singletonPattern.IsLoaded() == true);

        if (sceneLoaded)
        {
            // Suscribirse al evento sceneLoaded antes de cargar la escena
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        // Ahora cargar la escena
        SceneManager.LoadScene(sceneName);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded llamado para la escena: " + scene.name);
        if (singletonPattern == null)
        {
            Debug.LogError("singletonPattern es null en OnSceneLoaded.");
            return; // Detenemos la ejecución para evitar más errores
        }
        GameObject welcomeInterface = singletonPattern.GetWelcomeInterface();
        if (welcomeInterface == null)
        {
            Debug.LogError("GetWelcomeInterface() devolvió null.");
            return;
        }

        GameObject mainInterface = singletonPattern.GetMainInterface();
        if (mainInterface == null)
        {
            Debug.LogError("GetMainInterface() devolvió null.");
            return;
        }
        if (scene.name == "InitialMenu")
        {
            // Desactivar la interfaz de bienvenida y activar la interfaz principal
            singletonPattern.GetWelcomeInterface().SetActive(false);
            singletonPattern.GetMainInterface().SetActive(true);

            Debug.Log("Escena cargada: " + scene.name);
            // Desuscribirse del evento sceneLoaded para evitar múltiples suscripciones
            Debug.Log("Desuscribiendo del evento sceneLoaded.");
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

}
