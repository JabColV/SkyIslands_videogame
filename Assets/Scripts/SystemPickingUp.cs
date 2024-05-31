using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SystemPickingUp : MonoBehaviour
{
    FirebaseDatabase database;
    public TMP_Text coinsText;
    public int coins;
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
        if (database.GetDataUserInfo() != null)
        {
            coins = database.GetDataUserInfo().totalCoins;
            Debug.Log("Coins - SystemPickingUp: " + coins);
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
