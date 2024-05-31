using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SystemPickingUp : MonoBehaviour
{
    FirebaseDatabase database;
    public TMP_Text coinsText;
    public int coins = 0;
    public static SystemPickingUp Instance;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        database = FirebaseDatabase.Instance;
        if (database.dataUser != null)
        {
            coins = database.dataUser.totalCoins;
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Coin"))
        {
            Destroy(other.gameObject);
            coins++;
            coinsText.text = coins.ToString();
        }
    }
}
