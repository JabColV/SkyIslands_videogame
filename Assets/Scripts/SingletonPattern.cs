using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonPattern : MonoBehaviour
{
    public static SingletonPattern Instance;

    #region Own variables
    GameObject? player;
    public GameObject? WelcomeInterface;
    public GameObject? MainInterface;
    public GameObject panelQuestionInterface;
    bool isLoaded = false;
    // bool isRestarting = false;
    bool isInWater = false;
    bool activeGameAudio = false;
    bool hasGoggles = false;
    bool gemGotten = false;
    bool win = false;
    bool hasFirstPlanks = false;
    bool hasSecondPlanks = false;
    int coins;
    int gems;
    int lifesNumber;
    public AudioClip gameAudioSong;
    public List<AudioSource> audioSources;
    #endregion

    #region External variables
    FirebaseDatabase database;
    FirebaseAuth firebaseAuth;
    PlayerController playerController;
    #endregion

    private void Awake()
    {
        // Verificar si ya existe una instancia de FirebaseAuth
        if (Instance == null)
        {
            // Si no existe, asignar esta instancia a la variable Instance 
            Instance = this;
            // y no destruir el objeto al cargar una nueva escena
            DontDestroyOnLoad(gameObject);
            LoadData();
        }
        else
        {
            if (Instance != this)
            {
                // Si ya existe una instancia de FirebaseAuth, destruir este objeto
                Destroy(gameObject);
            }
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
        if (database == null)
        {
            Debug.LogError("FirebaseDatabase no se encontró en ButtonStart.");
        }

        if (firebaseAuth == null)
        {
            Debug.LogError("FirebaseAuth no se encontró en ButtonStart.");
        }
        if (WelcomeInterface == null)
        {
            Debug.LogError("WelcomeInterface no está asignado.");
        }
        if (MainInterface == null)
        {
            Debug.LogError("MainInterface no está asignado.");
        }
    }

    public void LoadData(){
        // Asignar las instancias de FirebaseDatabase, FirebaseAuth 
        database = this.GetComponent<FirebaseDatabase>();
        firebaseAuth = GameObject.Find("ButtonStart")?.GetComponent<FirebaseAuth>();
        audioSources = new List<AudioSource>(GetComponents<AudioSource>());
    }

    // Método para reproducir música de fondo
    public void PlayBackgroundMusic(AudioClip clip)
    {
        audioSources[0].clip = clip;
        audioSources[0].volume = 0.3f; // Ajusta el volumen específico para la música de fondo
        audioSources[0].loop = true;
        audioSources[0].Play();
    }

    // Método para detener la música de fondo
    public void StopBackgroundMusic()
    {
        audioSources[0].Stop(); 
    }

    // Método para reproducir efectos de sonido
    public void PlaySoundEffect(AudioClip clip, float volume)
    {
        audioSources[1].PlayOneShot(clip, volume);
    }

    // Método para detener efectos de sonido
    public void activeSoundsEffects()
    {
        audioSources[1].enabled = true; 
    }
    // Método para detener efectos de sonido
    public void StopSoundEffects()
    {
        audioSources[1].enabled = false; 
    }

    public GameObject GetPlayer()
    {
        return player;
    }

    public GameObject GetWelcomeInterface()
    {
        return WelcomeInterface;
    }

    public GameObject GetMainInterface()
    {
        return MainInterface;
    }

    public GameObject GetPanelQuestionInterface()
    {
        return panelQuestionInterface;
    }

    public bool GetIsInWater()
    {
        return isInWater;
    }

    public int GetCoins()
    {
        return coins;
    }

    public int GetGems()
    {
        return gems;
    }

    public bool GetHasGoggles()
    {
        return hasGoggles;
    }

    public int GetLifes()
    {
        return lifesNumber;
    }

    public bool GetWin()
    {
        return win;
    }

    public bool GetGemGotten()
    {
        return gemGotten;
    }

    public bool GetHasFirstPlanks()
    {
        return hasFirstPlanks;
    }

    public bool GetHasSecondPlanks()
    {
        return hasSecondPlanks;
    }

    public AudioClip GetGameAudioSong()
    {
        return gameAudioSong;
    }

     public void SetIsInWater(bool isInWater)
    {
        this.isInWater = isInWater;
    }

    public void SetGemGotten(bool gemGotten)
    {
        this.gemGotten = gemGotten;
    }

    public bool IsLoaded()
    {
        return isLoaded;
    }

    public void SetPlayer(GameObject player)
    {
        this.player = player;
    }

    public void SetCoins(int coins)
    {
        this.coins = coins;
    }

    public void SetGems(int gems)
    {
        this.gems = gems;
    }

    public void SetHasGoggles(bool hasGoggles)
    {
        this.hasGoggles = hasGoggles;
    }

    public void SetLifes(int lifes)
    {
        this.lifesNumber = lifes;
    }

    public void SetWin(bool win)
    {
        this.win = win;
    }

    public void SetIsLoaded(bool isLoaded)
    {
        this.isLoaded = isLoaded;
    }

    public void SetHasFirstPlanks(bool fPlanks)
    {
        this.hasFirstPlanks = fPlanks;
    }

    public void SetHasSecondPlanks(bool sPlanks)
    {
        this.hasSecondPlanks = sPlanks;
    }

    public FirebaseDatabase GetDatabase()
    {
        return database;
    }

    public FirebaseAuth GetFirebaseAuth()
    {
        return firebaseAuth;
    }

    public void SetPlayerController(PlayerController playerController)
    {
        this.playerController = playerController;
    }
    
    public void SetPanelQuestionInterface(GameObject panelQuestionInterface)
    {
        this.panelQuestionInterface = panelQuestionInterface;
    }

    public PlayerController GetPlayerController()
    {
        return playerController;
    }

    public void ClearData()
    {
        player = null;
        isLoaded = false;
        coins = 0;
        GetDatabase().ClearData();
        GetFirebaseAuth().ClearData();
    }
}
