using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Collisions : MonoBehaviour
{
    #region audio variables
    public AudioClip yellAudio;
    public AudioClip winAudio;
    public AudioClip splasAudio;
    public AudioClip gemAudio;
    public AudioClip sharkAudio;
    public Vector3 lastIsland;
    #endregion

    public TMP_Text coinsText;
    int coins;
    public AudioClip coinAudio;
    private HashSet<GameObject> collidedIslands = new HashSet<GameObject>();
    private HashSet<GameObject> collidedPlanks = new HashSet<GameObject>();
    SingletonPattern singletonPattern;

    // Start is called before the first frame update
    void Start()
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
        if (other.gameObject.CompareTag("Island") && !collidedIslands.Contains(other.gameObject))
        {
            lastIsland = this.gameObject.transform.position;
            collidedIslands.Add(other.gameObject);
            singletonPattern.SetPlayer(this.gameObject);
            singletonPattern.GetDatabase().UpdateData(Vector3.zero);
        }

        if (other.gameObject.CompareTag("bird"))
        {
            singletonPattern.PlaySoundEffect(yellAudio, 1.0f);
            singletonPattern.GetPlayerController().loseLife();
        }

        if (other.gameObject.CompareTag("Ocean"))
        {
            singletonPattern.PlaySoundEffect(splasAudio, 1.0f);
            singletonPattern.GetPlayerController().isInWater = true;
            singletonPattern.SetIsInWater(singletonPattern.GetPlayerController().isInWater);
        }

        if (other.gameObject.CompareTag("gem"))
        {
            singletonPattern.SetGemGotten(true);
            singletonPattern.PlaySoundEffect(gemAudio, 1.0f);
            Destroy(other.gameObject);
            singletonPattern.GetPlayerController().gemEffects[singletonPattern.GetGems()].SetActive(false);
            singletonPattern.GetPlayerController().box.SetActive(true);
            singletonPattern.GetPlayerController().gems[singletonPattern.GetGems()].SetActive(true);
            if (singletonPattern.GetGems() == 2)
            {
                singletonPattern.GetPlayerController().diamonds[singletonPattern.GetGems()+1].SetActive(true);
                singletonPattern.GetPlayerController().gemEffects[singletonPattern.GetGems()+1].SetActive(true);
            }
            else if (singletonPattern.GetGems() == 3)
            {
                singletonPattern.GetPlayerController().diamonds[singletonPattern.GetGems()+1].SetActive(true);
                singletonPattern.GetPlayerController().gemEffects[singletonPattern.GetGems()+1].SetActive(true);
            }
            //Aumentar el número de gemas
            singletonPattern.GetPlayerController().gemsNumber += 1;
            singletonPattern.SetGems(singletonPattern.GetPlayerController().gemsNumber);
            singletonPattern.GetDatabase().UpdateData(lastIsland);
        }

        if (other.gameObject.CompareTag("portal"))
        {
            lastIsland = this.gameObject.transform.position;
            singletonPattern.PlaySoundEffect(winAudio, 1.0f);
            singletonPattern.SetWin(true);
            singletonPattern.GetDatabase().UpdateData(lastIsland);
            
        }
        if (other.gameObject.CompareTag("pinchos"))
        {
            singletonPattern.PlaySoundEffect(yellAudio, 1.0f);
            singletonPattern.GetPlayerController().loseLife();
        }
        if (other.gameObject.CompareTag("Coin"))
        {
            singletonPattern.PlaySoundEffect(coinAudio, 1.0f);
            Destroy(other.gameObject);
            coins += 1;
            coinsText.text = coins.ToString();
            singletonPattern.SetCoins(coins);
        }
        if (other.gameObject.CompareTag("Shark"))
        {
            singletonPattern.PlaySoundEffect(sharkAudio, 1.0f);
            singletonPattern.GetPlayerController().loseLife();
        }
        // if (other.gameObject.CompareTag("wooden_plank") && !collidedPlanks.Contains(other.gameObject) && (!singletonPattern.GetHasFirstPlanks() || !singletonPattern.GetHasSecondPlanks()))
        if (other.gameObject.CompareTag("wooden_plank") && !collidedPlanks.Contains(other.gameObject))
        {
            //singletonPattern.PlaySoundEffect(sharkAudio, 1.0f);
            // singletonPattern.SetCollisions(this.GetComponent<Collisions>());
            Time.timeScale = 0f;
            collidedPlanks.Add(other.gameObject);
            singletonPattern.GetPanelQuestionInterface().SetActive(true);
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
                coinsText.text = coins.ToString();
                //Actualizar estado de las corazones
                singletonPattern.GetPlayerController().SetHeartActive(false);
            }
        }
        singletonPattern.SetCoins(coins);
    }
}
