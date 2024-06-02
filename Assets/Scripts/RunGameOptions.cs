using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class RunGameOptions : MonoBehaviour
{
    SingletonPattern singletonPattern;

    private void Start()
    {
        singletonPattern = SingletonPattern.Instance;
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



