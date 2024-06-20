using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MenuPause : MonoBehaviour
{
    [SerializeField] private GameObject pauseButton;
    [SerializeField] private GameObject Menu;
    public PlayerController playerController;
    // To store the singleton pattern instance
    SingletonPattern singletonPattern;

    // Start is called before the first frame update
    public void Start()
    {
        singletonPattern = SingletonPattern.Instance;
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        pauseButton.SetActive(false);
        Menu.SetActive(true);
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        pauseButton.SetActive(true);
        Menu.SetActive(false);
    }

    public void LastMemory()
    {
        StartCoroutine(ReloadData("FirstLevel", false));
    }

    void SceneLastMemory(Scene scene, LoadSceneMode mode)
    {
        
        
        // Setear el estado de la restauracion
        singletonPattern.SetRestarting(true);
        // Restaurar las gafas del jugador
        singletonPattern.SetHasGoggles(false);
        // Restaurar el estado gana o pierde
        singletonPattern.SetWin(false);
        // Restaurar el estado de las gemas
        playerController.DiamondDeactivation();
        // Actualizar los datos del usuario
        singletonPattern.GetDatabase().UpdateData(playerController.lastIsland);
        // Desuscribirse del evento sceneLoaded para evitar múltiples suscripciones
        SceneManager.sceneLoaded -= SceneRestart;
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        // Para saber el estado de la restauracion
        singletonPattern.SetRestarting(false);
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
        // Actualizar los datos del usuario
        singletonPattern.GetDatabase().UpdateData(new Vector3(-3.700000047683716f, 21.304550170898438f, 171.6999969482422f));
        // Setear el estado de la restauracion
        singletonPattern.SetRestarting(true);
        // Desuscribirse del evento sceneLoaded para evitar múltiples suscripciones
        SceneManager.sceneLoaded -= SceneRestart;
    }

    public IEnumerator ReloadData(string sceneName = "InitialMenu", bool sceneLoaded = true)
    {
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
        if (scene.name == "InitialMenu")
        {
            // Desactivar la interfaz de bienvenida y activar la interfaz principal
            singletonPattern.GetWelcomeInterface().SetActive(false);
            singletonPattern.GetMainInterface().SetActive(true);

            // Desuscribirse del evento sceneLoaded para evitar múltiples suscripciones
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    public void LoadSceneMain()
    {
        // Iniciar la corutina ReloadData
        StartCoroutine(ReloadData());
    }
}
