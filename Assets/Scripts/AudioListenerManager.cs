using UnityEngine;

public class AudioListenerManager : MonoBehaviour
{
    void Start()
    {
        // Desactivar todos los componentes de AudioListener, excepto uno
        AudioListener[] audioListeners = FindObjectsOfType<AudioListener>();
        for (int i = 1; i < audioListeners.Length; i++)
        {
            audioListeners[i].enabled = false;
        }
    }
}
