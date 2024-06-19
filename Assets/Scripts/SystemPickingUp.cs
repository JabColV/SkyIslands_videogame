using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SystemPickingUp : MonoBehaviour
{
    public TMP_Text coinsText;
    int coins;
    SingletonPattern singletonPattern;
    public AudioClip coinAudio;

    private void Start()
    {
        singletonPattern = SingletonPattern.Instance;
    }
    
    public void SetCoins(int coins)
    {
        this.coins = coins;
        coinsText.text = coins.ToString(); // Asegurarse de que el texto se actualice cuando se cambien las monedas
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Coin"))
        {
            singletonPattern.PlaySoundEffect(coinAudio, 1.0f);
            Destroy(other.gameObject);
            coins += 1;
            coinsText.text = coins.ToString();
            singletonPattern.SetCoins(coins);
        }
    }

    void Update()
    {
        if((coins - 15) >= 0)
        {
            //Gana una vida
            singletonPattern.GetPlayerController().winLife();
            if (singletonPattern.GetPlayerController().GetHeartActive() == true)
            {
                //Reinicio de monedas
                coins -= 15;
                //Actualizar estado de las corazones
                singletonPattern.GetPlayerController().SetHeartActive(false);
            }
        }
        singletonPattern.SetCoins(coins);
    }
}
