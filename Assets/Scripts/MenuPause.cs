using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MenuPause : MonoBehaviour
{
    [SerializeField] private GameObject pauseButton;
    [SerializeField] private GameObject Menu;
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

    public void Restart()
    {
        Time.timeScale = 1f;
        // Para saber el estado de la restauracion
        singletonPattern.SetRestarting(false);
        // Suscribirse al evento sceneLoaded antes de cargar la escena
        SceneManager.sceneLoaded += SceneRestart;
        // Restaurar la escena actual
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void SceneRestart(Scene scene, LoadSceneMode mode)
    {
        // Restaurar las vidas del jugador
        singletonPattern.SetLifes(3);
        // Actualizar los datos del usuario
        singletonPattern.GetDatabase().UpdateData();
        // Setear el estado de la restauracion
        singletonPattern.SetRestarting(true);
        // Desuscribirse del evento sceneLoaded para evitar múltiples suscripciones
        SceneManager.sceneLoaded -= SceneRestart;
    }

    public IEnumerator ReloadData()
    {
        Debug.Log("Recargando datos...");
        singletonPattern.SetIsLoaded(false);
        // Obtener los datos del usuario
        singletonPattern.GetDatabase().GetData();

        Debug.Log("Esperando a que los datos se carguen completamente...");
        // Esperar a que los datos se carguen completamente
        yield return new WaitUntil(() => singletonPattern.IsLoaded() == true);

        Debug.Log("Se añade el evento sceneLoaded...");
        // Suscribirse al evento sceneLoaded antes de cargar la escena
        SceneManager.sceneLoaded += OnSceneLoaded;

        Debug.Log("Cargando la escena...");
        // Ahora cargar la escena
        SceneManager.LoadScene("InitialMenu");
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Escena cargada: " + scene.name);
        if (scene.name == "InitialMenu")
        {
            Debug.Log("Interfaz Welcome: " + singletonPattern.GetMainInterface());
            Debug.Log("Desactivando la interfaz de bienvenida " + singletonPattern.GetWelcomeInterface().name);
            singletonPattern.GetWelcomeInterface().SetActive(false);
            Debug.Log("Activando la interfaz principal " + singletonPattern.GetMainInterface().name);
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
