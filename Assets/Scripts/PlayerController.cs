using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    #region game variables
    public GameObject[] lifes;
    public GameObject[] gems;
    public GameObject[] diamonds;
    public GameObject[] gemEffects;
    public GameObject FinalPortal;
    public GameObject box;
    public GameObject ocean;
    public GameObject goggles;
    public GameObject gogglesHelp;
    public GameObject Canva1;
    public GameObject Canva2;
    public GameObject Menu;
    #endregion

    #region bool variables
    bool isJumpping = false;
    public bool isInWater = false;
    public bool floorDetected = false;
    public bool heartActive = false;
    bool hasGoggles = false;
    bool isDrowning = false;
    #endregion

    #region audio variables
    public AudioClip jumpAudio;
    public AudioClip gameoverAudio;
    #endregion

    public TMP_Text gogglesExplanation;
    private Rigidbody playerRB;
    private float jumpForce = 49.0f;
    private float movementSpeed = 15.0f;
    public float RotationSpeed = 2.0f;
    private Animator anim;
    public float x, y, z;
    public MenuPause menuPause;
    int lifesNumber;
    public int gemsNumber;
    List<BoxCollider> boxColliders;

    Collisions collisions;
    SingletonPattern singletonPattern;
    [SerializeField] private float _gravity;
    [SerializeField] private float _fallVelocity;

    public void Start()
    {
        singletonPattern = SingletonPattern.Instance;
        collisions = GetComponent<Collisions>();
        playerRB = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        boxColliders = new List<BoxCollider>(ocean.GetComponents<BoxCollider>());
        StartCoroutine(LoadStatus());
        _gravity = 60f; // Gravedad
    }

    public IEnumerator LoadStatus()
    {
        singletonPattern.SetIsLoaded(false);
        singletonPattern.GetDatabase().GetData();
        yield return new WaitUntil(() => singletonPattern.IsLoaded() == true);
        this.transform.position = singletonPattern.GetDatabase().GetDataUserInfo().position;
        lifesNumber = singletonPattern.GetDatabase().GetDataUserInfo().vidas;
        gemsNumber = singletonPattern.GetDatabase().GetDataUserInfo().gemas;
        collisions.SetCoins(singletonPattern.GetDatabase().GetDataUserInfo().totalCoins);
        hasGoggles = singletonPattern.GetDatabase().GetDataUserInfo().hasGoggles;
        singletonPattern.SetHasGoggles(hasGoggles);

        //Life configuration
        if (lifesNumber == 0)
        {
            lifesNumber = 3;
        }
        int rest = 3 - lifesNumber;
        singletonPattern.SetLifes(lifesNumber);
        for (int i = rest; i > 0; i--)
        {
            DesactivateLife(3 - i);
        }

        //Gems configuration
        singletonPattern.SetGems(gemsNumber);
        if (gemsNumber > 0)
        {
            box.SetActive(true);
        }
        for (int i = gemsNumber; i > 0; i--)
        {
            gems[i-1].SetActive(true);
            gemEffects[i-1].SetActive(false);
            diamonds[i-1].SetActive(false);
        }
        if (gemsNumber == 3)
        {
            diamonds[gemsNumber].SetActive(true);
            gemEffects[gemsNumber].SetActive(true);
        }
        else if (gemsNumber == 4)
        {
            diamonds[gemsNumber].SetActive(true);
            gemEffects[gemsNumber].SetActive(true);
        }
        
        singletonPattern.SetPlayer(this.gameObject);
        singletonPattern.SetPlayerController(this.GetComponent<PlayerController>());
    }

    public void DesactivateLife(int indice)
    {
        lifes[indice].SetActive(false);
    }

    public void ActivateLife(int indice)
    {
        lifes[indice].SetActive(true);
    }

    public void loseLife()
    {
        lifesNumber = lifesNumber - 1;
        if (lifesNumber == 0)
        {
            singletonPattern.PlaySoundEffect(gameoverAudio, 1.0f);
            menuPause.LastMemory();
        }
        DesactivateLife(lifesNumber);
        singletonPattern.SetLifes(lifesNumber);
    }

    public void winLife()
    {
        if (lifesNumber == 3)
        {
            return;
        }
        heartActive = true;
        ActivateLife(lifesNumber);
        lifesNumber = lifesNumber + 1;
        singletonPattern.SetLifes(lifesNumber);
    }

    public bool GetHeartActive()
    {
        return heartActive;
    }

    public void SetHeartActive(bool heartActive)
    {
        this.heartActive = heartActive;
    }

    //Desactiva todas las gemas, ya que cuando se reinicia el juego, el jugador no tiene ninguna gema
    public void DiamondDeactivation()
    {
        box.SetActive(false);
        for (int i = 0; i < gemsNumber; i++)
        {
            gems[i].SetActive(false);
        }
    }

    void ShowGogglesHelp()
    {
        if (gemsNumber == 3 && !hasGoggles)
        {
            gogglesExplanation.text = "Presiona la tecla E para usarlas, debes arrojarte al mar y bucar el resto de las gemas, no lo podrás hacer sin las gafas de buceo.";
            gogglesHelp.SetActive(true);
            gogglesHelp.transform.Rotate(Vector3.up, 20.0f * Time.deltaTime);
        }
    }

    void TakeGoggles()
    {
        if (Input.GetKeyDown(KeyCode.E) && gemsNumber == 3 && !hasGoggles)
        {
            gogglesHelp.SetActive(false);
            gogglesExplanation.enabled = false;
            goggles.SetActive(true);
            hasGoggles = true;
            singletonPattern.SetHasGoggles(hasGoggles);
            singletonPattern.GetDatabase().UpdateData(collisions.lastIsland);
        }
    }

    private IEnumerator Drown()
    {
        isDrowning = true;
        anim.SetBool("drowning", true);
        
        yield return new WaitForSeconds(2); 
        loseLife();
        menuPause.LastMemory();
        isDrowning = false; 
        
    }

    void Update()
    {
        if (singletonPattern.GetWin()){
            Canva1.SetActive(false);
            Canva2.SetActive(true);
        }
        else
        {
            x = Input.GetAxis("Horizontal");
            z = Input.GetAxis("Forward");
            anim.SetFloat("VelX", x);
            anim.SetFloat("VelY", z);

            Vector3 floor = transform.TransformDirection(Vector3.down);
            if (Physics.Raycast(transform.position, floor, 1.0f))
            {
                floorDetected = true;
                anim.SetBool("landed", true);
            }
            else
            {
                floorDetected = false;
                anim.SetBool("landed", false);
                anim.SetBool("jumped", false);
            }

            if (Input.GetButtonDown("Jump") && floorDetected && !isInWater)
            {
                isJumpping = true;
            } 
            else
            {
                anim.SetBool("jumped", false);
            }

            if (isInWater)
            {
                WaterConfiguration();
            }

            if (isInWater && !hasGoggles && !isDrowning)
            {
                StartCoroutine(Drown());
            }

            ShowGogglesHelp();
            
            TakeGoggles();

            goggles.SetActive(hasGoggles);

            HasFullGems();
        }
    }

    void HasFullGems()
    {
        if (gemsNumber == 5)
        {
            //Aparece un portal en el agua para finalizar el juego
            FinalPortal.SetActive(true);
        }
    }
    void playerRBWaterConfig()
    {
        if (isInWater)
        {
            _gravity = 0.0f;
            playerRB.useGravity = false;

            Vector3 gravity = -0.4f * Vector3.down;
            playerRB.AddForce(gravity, ForceMode.Acceleration);

            Quaternion deltaRotation = Quaternion.Euler(0, x * Time.deltaTime * RotationSpeed, 0);
            playerRB.MoveRotation(playerRB.rotation * deltaRotation);

            Vector3 waterMovement = transform.TransformDirection(new Vector3(0, y, 0)) * (movementSpeed*3.0f) * Time.deltaTime;
            Vector3 newPosition = playerRB.position + waterMovement;
            playerRB.MovePosition(newPosition);
        }
        else
        {
            _gravity = 60.0f;
            playerRB.useGravity = true;
        }
    }

    void WaterConfiguration()
    {
        if (isInWater)
        {
            y = Input.GetAxis("Vertical");

            anim.SetBool("in_water", true);
            float upperLimit = -38.36f;

            Vector3 newPosition = transform.position;
            if (newPosition.y < upperLimit)
            {
                boxColliders[0].enabled = false;
                boxColliders[1].enabled = true;
            }
            else if (newPosition.y > upperLimit)
            {
                boxColliders[0].enabled = true;
                boxColliders[1].enabled = false;
                isInWater = false; 
            }
        }
        else
        {
            anim.SetBool("in_water", false);
            singletonPattern.SetIsInWater(isInWater);
        }
    }

    void FixedUpdate()
    {
        Vector3 movementDirection = transform.TransformDirection(new Vector3(x, 0, z).normalized);
        playerRB.MovePosition(transform.position + movementDirection * movementSpeed * Time.deltaTime);

        playerRBWaterConfig();

        Vector3 vecGravity = new Vector3(0, -_gravity, 0);
        playerRB.AddForce(vecGravity * playerRB.mass);

        if (isJumpping && floorDetected && !isInWater)
        {
            playerRB.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
            singletonPattern.PlaySoundEffect(jumpAudio, 1.0f);
            isJumpping = false;
        }
    }
}





