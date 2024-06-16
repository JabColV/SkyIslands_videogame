using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // To use some properties from the character's rigidbody 
    private Rigidbody rb;
    // To know when the character is jumping
    bool isJumpping = false;
    // To know when the character is in the water
    bool isInWater = false;
    // To know when the character is touching the ground
    bool floorDetected = false;
    // To impulse up the character
    private float jumpForce = 49.0f;
    // To know the speed of the character's movement
    private float movementSpeed = 15.0f;
    // To store the speed of the character's rotation
    public float RotationSpeed = 10.0f;
    // To be able to modify some of the animator's properties
    private Animator anim;
    // To store the movement in x and y axis
    public float x, y, z;
    // To store the islands that the character has collided with
    private HashSet<GameObject> collidedIslands = new HashSet<GameObject>();
    // To store the player's lifes
    public GameObject[] lifes;
    // To store the number of player's lifes
    int lifesNumber;
    // To store the game over menu
    public GameObject gameOverMenu;
    // To store the pause menu script
    public MenuPause menuPause;

    public bool heartActive = false;
    // To store the audio of the character's jump
    public AudioClip jumpAudio;
    // To store the audio of the character's yell
    public AudioClip yellAudio;
    // To store the audio of the game over
    public AudioClip gameoverAudio;
    // To store the audio of the character's splash
    public AudioClip splasAudio;
    // To store the ocean's game object
    public GameObject ocean;
    // To store the box collider of the ocean
    public List<BoxCollider> boxColliders;
    // To store the singleton pattern instance
    SingletonPattern singletonPattern;

    [SerializeField] private float _gravity;
    [SerializeField] private float _fallVelocity;

    // Start is called before the first frame update
    public void Start()
    {
        singletonPattern = SingletonPattern.Instance;
        rb = GetComponent<Rigidbody>();
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
        // Obtener los datos del usuario
        singletonPattern.GetDatabase().GetData();
        // Esperar a que los datos se carguen completamente
        yield return new WaitUntil(() => singletonPattern.IsLoaded() == true);
        // Set the player's position to the current position
        this.transform.position = singletonPattern.GetDatabase().GetDataUserInfo().position;
        // Set the player's lifes to the current lifes
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
            DesactivateLife(3-i);
        }
        // Set this file to the singleton pattern
        singletonPattern.SetPlayerController(this);
    }

    public void DesactivateLife(int indice)
    {
        // Desactivate the last life
        lifes[indice].SetActive(false);
    }

    public void ActivateLife(int indice)
    {
        // Activate the last life
        lifes[indice].SetActive(true);
    }

    public void loseLife()
    {
        lifesNumber = lifesNumber - 1;
        if (lifesNumber == 0)
        {
            singletonPattern.PlaySound(gameoverAudio);
            menuPause.Restart();
        }  
        DesactivateLife(lifesNumber);
        singletonPattern.SetLifes(lifesNumber);
        // Update the database with the new data
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
        //Update the database with the new data
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
            // Add the island to the list of collided islands
            collidedIslands.Add(other.gameObject);
            // set the player's position to the current position
            singletonPattern.SetPlayer(this.gameObject);
            // Update the database with the new data
            singletonPattern.GetDatabase().UpdateData();
        }

        if (other.gameObject.CompareTag("bird") && !collidedIslands.Contains(other.gameObject))
        {
            singletonPattern.PlaySound(yellAudio);
            loseLife();
        }

        if (other.gameObject.CompareTag("Ocean") && !collidedIslands.Contains(other.gameObject))
        {
            singletonPattern.PlaySound(splasAudio);
            isInWater = true;
            loseLife();
        } 
    }

    void Update()
    {
        Debug.Log("Is in water: " + isInWater);
        // Capture user input values for horizontal and vertical movement
        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Foward");

        Vector3 movementDirection = new Vector3(x, 0, z);
        movementDirection.Normalize();
       
        // Move the object forward based on vertical input
        transform.Translate(x * Time.deltaTime * movementSpeed, 0, z * Time.deltaTime * movementSpeed);

        // Update animation parameters with input values between -1 and 1
        anim.SetFloat("VelX", x);
        anim.SetFloat("VelY", z);

        // Define the direction downwards from the object's current position
        Vector3 floor = transform.TransformDirection(Vector3.down);
        // Perform a raycast downwards to detect if the object is on the ground
        if (Physics.Raycast(transform.position, floor, 0.6f))
        {
            // The object is in contact with the ground
            floorDetected = true;
            anim.SetBool("landed", true);
        }
        else
        {
            // The object is not in contact with the ground
            floorDetected = false;
            anim.SetBool("landed", false);
            anim.SetBool("jumped", false);
        }

        // Check if the jump button was pressed
        isJumpping = Input.GetButtonDown("Jump");
        // If jump button is pressed and the object is on the ground, apply a jump force
        if (isJumpping && floorDetected && !isInWater)
        {
            rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
            // Play the jump audio
            singletonPattern.PlaySound(jumpAudio);
        }
        if (isInWater){
            WaterConfiguration();
        }

        
    }

    void WaterConfiguration()
    {
        if (isInWater){
            _gravity = 0.0f;
            rb.useGravity = false;
            Vector3 gravity = -0.2f * Vector3.down; 
            rb.AddForce(gravity, ForceMode.Acceleration);
            y = Input.GetAxis("Vertical");
            Vector3 waterMovement = new Vector3(0, y, 0);

            Vector3 newPosition = transform.position; 
            Debug.Log("New position" + newPosition);
            transform.Translate(waterMovement * Time.deltaTime * movementSpeed, Space.World);
            anim.SetBool("in_water", true);
            float upperLimit = -38.36f; // Límite superior del agua
            
            transform.Rotate(0, x * Time.deltaTime * RotationSpeed, 0);
            
            // Comprobar y ajustar la posición si excede los límites
            if (newPosition.y < upperLimit)
            {
                boxColliders[0].enabled = false;
                boxColliders[1].enabled = true;
            } else if (newPosition.y > upperLimit)
            {
                boxColliders[0].enabled = true;
                boxColliders[1].enabled = false;
                isInWater = false;
            }
        }
        else
        {
            _gravity = 60.0f;
            rb.useGravity = true;
            anim.SetBool("in_water", false);
        }
    }

    void FixedUpdate()
    {
        Vector3 vecGravity = new Vector3(0, -_gravity, 0);
        // Ensure gravity scale is appropriate
        rb.AddForce(vecGravity * rb.mass);

        if (isInWater)
        {
            WaterConfiguration();
        }
    }

}
