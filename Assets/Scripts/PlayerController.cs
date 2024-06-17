using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region game variables
    public GameObject[] lifes;
    public GameObject gameOverMenu;
    public GameObject ocean;
    #endregion

    #region bool variables
    bool isJumpping = false;
    bool isInWater = false;
    bool floorDetected = false;
    public bool heartActive = false;
    #endregion

    #region audio variables
    public AudioClip jumpAudio;
    public AudioClip yellAudio;
    public AudioClip gameoverAudio;
    public AudioClip splasAudio;
    #endregion

    private Rigidbody playerRB;
    private float jumpForce = 49.0f;
    private float movementSpeed = 15.0f;
    public float RotationSpeed = 2.0f;
    private Animator anim;
    public float x, y, z;
    private HashSet<GameObject> collidedIslands = new HashSet<GameObject>();
    public MenuPause menuPause;
    int lifesNumber;
    public List<BoxCollider> boxColliders;
    SingletonPattern singletonPattern;
    [SerializeField] private float _gravity;
    [SerializeField] private float _fallVelocity;

    public void Start()
    {
        singletonPattern = SingletonPattern.Instance;
        playerRB = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        boxColliders = new List<BoxCollider>(ocean.GetComponents<BoxCollider>());
        StartCoroutine(LoadStatus());
        _gravity = 60f; // 9.8f;
        if (singletonPattern.IsRestarting())
        {
            transform.position = new Vector3(-3.700000047683716f, 21.304550170898438f, 171.6999969482422f);
            singletonPattern.SetRestarting(false);
        }
    }

    public IEnumerator LoadStatus()
    {
        singletonPattern.SetIsLoaded(false);
        singletonPattern.GetDatabase().GetData();
        yield return new WaitUntil(() => singletonPattern.IsLoaded() == true);
        this.transform.position = singletonPattern.GetDatabase().GetDataUserInfo().position;
        lifesNumber = singletonPattern.GetDatabase().GetDataUserInfo().vidas;

        if (lifesNumber == 0)
        {
            menuPause.Restart();
            lifesNumber = 3;
        }
        int rest = 3 - lifesNumber;
        singletonPattern.SetLifes(lifesNumber);
        for (int i = rest; i > 0; i--)
        {
            DesactivateLife(3 - i);
        }
        singletonPattern.SetPlayer(this.gameObject);
        singletonPattern.SetPlayerController(this);
        // singletonPattern.PlayRepeatedSound(singletonPattern.GetGameAudioSong());
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
            menuPause.Restart();
        }
        DesactivateLife(lifesNumber);
        singletonPattern.SetLifes(lifesNumber);
        singletonPattern.GetDatabase().UpdateData();
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
        singletonPattern.GetDatabase().UpdateData();
    }

    public bool GetHeartActive()
    {
        return heartActive;
    }

    public void SetHeartActive(bool heartActive)
    {
        this.heartActive = heartActive;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Island") && !collidedIslands.Contains(other.gameObject))
        {
            collidedIslands.Add(other.gameObject);
            singletonPattern.SetPlayer(this.gameObject);
            singletonPattern.GetDatabase().UpdateData();
            Debug.Log("Player Controller: " + singletonPattern.GetPlayer());
        }
        if (other.gameObject.CompareTag("bird"))
        {
            singletonPattern.PlaySoundEffect(yellAudio, 1.0f);
            loseLife();
        }

        if (other.gameObject.CompareTag("Ocean"))
        {
            singletonPattern.PlaySoundEffect(splasAudio, 1.0f);
            isInWater = true;
            singletonPattern.SetIsInWater(isInWater);
        }
    }

    void Update()
    {
        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Forward");

        anim.SetFloat("VelX", x);
        anim.SetFloat("VelY", z);

        Vector3 floor = transform.TransformDirection(Vector3.down);
        if (Physics.Raycast(transform.position, floor, 0.6f))
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

        if (isInWater)
        {
            WaterConfiguration();
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

            Vector3 waterMovement = transform.TransformDirection(new Vector3(0, y, 0)) * movementSpeed * Time.deltaTime;
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





// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class PlayerController : MonoBehaviour
// {
//     #region game variables
//     // To store the player's lifes
//     public GameObject[] lifes;
//     // To store the game over menu
//     public GameObject gameOverMenu;
//     // To store the ocean's game object
//     public GameObject ocean;
//     #endregion
//     #region bool variables
//     // To know when the character is jumping
//     bool isJumpping = false;
//     // To know when the character is in the water
//     bool isInWater = false;
//     // To know when the character is touching the ground
//     bool floorDetected = false;
//     // To know when the heart is active
//     public bool heartActive = false;
//     #endregion
//     #region audio variables
//     // To store the audio of the character's jump
//     public AudioClip jumpAudio;
//     // To store the audio of the character's yell
//     public AudioClip yellAudio;
//     // To store the audio of the game over
//     public AudioClip gameoverAudio;
//     // To store the audio of the character's splash
//     public AudioClip splasAudio;
//     #endregion
//     // To use some properties from the character's rigidbody 
//     private Rigidbody playerRB;
//     // To impulse up the character
//     private float jumpForce = 49.0f;
//     // To know the speed of the character's movement
//     private float movementSpeed = 15.0f;
//     // To store the speed of the character's rotation
//     public float RotationSpeed = 2.0f;
//     // To be able to modify some of the animator's properties
//     private Animator anim;
//     // To store the movement in x and y axis
//     public float x, y, z;
//     // To store the islands that the character has collided with
//     private HashSet<GameObject> collidedIslands = new HashSet<GameObject>();
    
//     // To store the pause menu script
//     public MenuPause menuPause;
//     // To store the number of player's lifes
//     int lifesNumber;
    
//     // To store the box collider of the ocean
//     public List<BoxCollider> boxColliders;
//     // To store the singleton pattern instance
//     SingletonPattern singletonPattern;

//     [SerializeField] private float _gravity;
//     [SerializeField] private float _fallVelocity;

//     // Start is called before the first frame update
//     public void Start()
//     {
//         singletonPattern = SingletonPattern.Instance;
//         playerRB = GetComponent<Rigidbody>();
//         anim = GetComponent<Animator>();
//         boxColliders = new List<BoxCollider>(ocean.GetComponents<BoxCollider>());
//         StartCoroutine(LoadStatus());
//         _gravity = 60f; // 9.8f;
//         if (singletonPattern.IsRestarting())
//         {
//             transform.position = new Vector3(-3.700000047683716f, 21.304550170898438f, 171.6999969482422f);
//             singletonPattern.SetRestarting(false);
//         }
//     }

//     public IEnumerator LoadStatus()
//     {
//         singletonPattern.SetIsLoaded(false);
//         // Obtener los datos del usuario
//         singletonPattern.GetDatabase().GetData();
//         // Esperar a que los datos se carguen completamente
//         yield return new WaitUntil(() => singletonPattern.IsLoaded() == true);
//         // Set the player's position to the current position
//         this.transform.position = singletonPattern.GetDatabase().GetDataUserInfo().position;
//         // Set the player's lifes to the current lifes
//         lifesNumber = singletonPattern.GetDatabase().GetDataUserInfo().vidas;

//         if (lifesNumber == 0)
//         {
//             menuPause.Restart();
//             lifesNumber = 3;
//         }
//         int rest = 3 - lifesNumber;
//         singletonPattern.SetLifes(lifesNumber);
//         for (int i = rest; i > 0; i--)
//         {
//             DesactivateLife(3-i);
//         }
//         // set the player's position to the current position
//         singletonPattern.SetPlayer(this.gameObject);
//         // Set this file to the singleton pattern
//         singletonPattern.SetPlayerController(this);
//     }

//     public void DesactivateLife(int indice)
//     {
//         // Desactivate the last life
//         lifes[indice].SetActive(false);
//     }

//     public void ActivateLife(int indice)
//     {
//         // Activate the last life
//         lifes[indice].SetActive(true);
//     }

//     public void loseLife()
//     {
//         lifesNumber = lifesNumber - 1;
//         if (lifesNumber == 0)
//         {
//             singletonPattern.PlaySound(gameoverAudio);
//             menuPause.Restart();
//         }  
//         DesactivateLife(lifesNumber);
//         singletonPattern.SetLifes(lifesNumber);
//         // Update the database with the new data
//         singletonPattern.GetDatabase().UpdateData();
//     }

//     public void winLife()
//     {
//         if (lifesNumber == 3)
//         {
//             return;
//         }
//         heartActive = true;
//         ActivateLife(lifesNumber);
//         lifesNumber = lifesNumber + 1;
//         singletonPattern.SetLifes(lifesNumber);
//         //Update the database with the new data
//         singletonPattern.GetDatabase().UpdateData();
//     }

//     public bool GetHeartActive()
//     {
//         return heartActive;
//     }

//     public void SetHeartActive(bool heartActive)
//     {
//         this.heartActive = heartActive;
//     }

//     private void OnTriggerEnter(Collider other)
//     {
//         if (other.gameObject.CompareTag("Island") && !collidedIslands.Contains(other.gameObject))
//         {
//             // Add the island to the list of collided islands
//             collidedIslands.Add(other.gameObject);
//             // set the player's position to the current position
//             singletonPattern.SetPlayer(this.gameObject);
//             // Update the database with the new data
//             singletonPattern.GetDatabase().UpdateData();
//             Debug.Log("Player Controller: " + singletonPattern.GetPlayer());
//         }
//         if (other.gameObject.CompareTag("bird"))
//         {
//             singletonPattern.PlaySound(yellAudio);
//             loseLife();
//         }

//         if (other.gameObject.CompareTag("Ocean"))
//         {
//             singletonPattern.PlaySound(splasAudio);
//             isInWater = true;
//             singletonPattern.SetIsInWater(isInWater);
//             // loseLife();
//         } 
//     }

//     void Update()
//     {
//         // Capture user input values for horizontal and vertical movement
//         x = Input.GetAxis("Horizontal");
//         z = Input.GetAxis("Forward");

//         // Update animation parameters with input values between -1 and 1
//         anim.SetFloat("VelX", x);
//         anim.SetFloat("VelY", z);

//         // Define the direction downwards from the object's current position
//         Vector3 floor = transform.TransformDirection(Vector3.down);
//         // Perform a raycast downwards to detect if the object is on the ground
//         if (Physics.Raycast(transform.position, floor, 0.6f))
//         {
//             // The object is in contact with the ground
//             floorDetected = true;
//             anim.SetBool("landed", true);
//         }
//         else
//         {
//             // The object is not in contact with the ground
//             floorDetected = false;
//             anim.SetBool("landed", false);
//             anim.SetBool("jumped", false);
//         }
        
//         // Verificar si se presionó el botón de salto
//         if (Input.GetButtonDown("Jump") && floorDetected && !isInWater)
//         {
//             isJumpping = true;
//         }
        
//         if (isInWater){
//             WaterConfiguration();
//         }
//     }

//     // void playerRBWaterConfig()
//     // {
//     //     if (isInWater)
//     //     {
//     //         _gravity = 0.0f;
//     //         playerRB.useGravity = false;
//     //         Vector3 gravity = -0.4f * Vector3.down; 
//     //         playerRB.AddForce(gravity, ForceMode.Acceleration);
//     //         transform.Rotate(0, x * Time.deltaTime * RotationSpeed, 0);
//     //         Vector3 waterMovement = new Vector3(0, y, 0);
//     //         transform.Translate(waterMovement * Time.deltaTime * movementSpeed, Space.World);
//     //     }
//     //     else
//     //     {
//     //         _gravity = 60.0f;
//     //         playerRB.useGravity = true;
//     //     }
//     // }

//     void playerRBWaterConfig()
//     {
//         if (isInWater)
//         {
//             _gravity = 0.0f;
//             playerRB.useGravity = false;

//             // Aplicar fuerza gravitacional reducida
//             Vector3 gravity = -0.4f * Vector3.down; 
//             playerRB.AddForce(gravity, ForceMode.Acceleration);

//             // Rotación usando el Rigidbody
//             Quaternion deltaRotation = Quaternion.Euler(0, x * Time.deltaTime * RotationSpeed, 0);
//             playerRB.MoveRotation(playerRB.rotation * deltaRotation);

//             // Movimiento en el agua usando el Rigidbody
//             Vector3 waterMovement = new Vector3(0, y, 0) * movementSpeed * Time.deltaTime;
//             Vector3 newPosition = playerRB.position + waterMovement;
//             playerRB.MovePosition(newPosition);
//         }
//         else
//         {
//             _gravity = 60.0f;
//             playerRB.useGravity = true;
//         }
//     }


//     void WaterConfiguration()
//     {
//         if (isInWater){
//             y = Input.GetAxis("Vertical");

//             anim.SetBool("in_water", true);
//             float upperLimit = -38.36f; // Límite superior del agua
            
//             Vector3 newPosition = transform.position; 
//             // Comprobar y ajustar la posición si excede los límites
//             if (newPosition.y < upperLimit)
//             {
//                 boxColliders[0].enabled = false;
//                 boxColliders[1].enabled = true;
//             } else if (newPosition.y > upperLimit)
//             {
//                 boxColliders[0].enabled = true;
//                 boxColliders[1].enabled = false;
//                 isInWater = false;
//             }
//         }
//         else
//         {
//             anim.SetBool("in_water", false);
//             singletonPattern.SetIsInWater(isInWater);
//         }
//     }

//     void FixedUpdate()
//     {
//         // Move the object based on the input values
//         Vector3 movementDirection = new Vector3(x, 0, z).normalized;
//         playerRB.MovePosition(transform.position - movementDirection * movementSpeed * Time.deltaTime);

//         // Apply the water configuration
//         playerRBWaterConfig();
        
//         // Aplicar gravedad al objeto
//         Vector3 vecGravity = new Vector3(0, -_gravity, 0);
//         playerRB.AddForce(vecGravity * playerRB.mass);
        
//         // Aplicar la fuerza de salto si el objeto está en el suelo y se presionó el botón de salto
//         if (isJumpping && floorDetected && !isInWater)
//         {
//             playerRB.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
//             singletonPattern.PlaySound(jumpAudio);
//             isJumpping = false; // Reset the jump state after applying the force
//         }
//     }

// }
