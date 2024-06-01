using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonPattern : MonoBehaviour
{
    public static SingletonPattern Instance;

    #region Own variables
    GameObject player;
    int coins;
    #endregion

    #region External variables
    FirebaseDatabase database;
    FirebaseAuth firebaseAuth;
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
            loadData();
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
    }

    public void loadData(){
        // Asignar las instancias de FirebaseDatabase, FirebaseAuth y SystemPickingUp
        database = this.GetComponent<FirebaseDatabase>();
        firebaseAuth = GameObject.Find("ButtonStart")?.GetComponent<FirebaseAuth>();
    }

    public GameObject GetPlayer()
    {
        return player;
    }

    public int GetCoins()
    {
        return coins;
    }

    public void SetPlayer(GameObject player)
    {
        this.player = player;
    }

    public void SetCoins(int coins)
    {
        this.coins = coins;
    }

    public FirebaseDatabase GetDatabase()
    {
        return database;
    }

    public FirebaseAuth GetFirebaseAuth()
    {
        return firebaseAuth;
    }
}
