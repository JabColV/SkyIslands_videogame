using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // To use some properties from the character's rigidbody 
    private Rigidbody rb;
    // To know when the character is jumping
    bool isJumpping = false;
    // To know when the character is touching the ground
    bool floorDetected = false;
    // To impulse up the character
    private float jumpForce = 20.0f;
    // To know the speed of the character's movement
    private float movementSpeed = 15.0f;
    // To store the speed of the character's rotation
    public float RotationSpeed = 200.0f;
    // To be able to modify some of the animator's properties
    private Animator anim;
    // To store the movement in x and y axis
    public float x, y;
    // To store the database object
    public FirebaseDatabase database;
    // To store the islands that the character has collided with
    private HashSet<GameObject> collidedIslands = new HashSet<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        // To get the character's rigidbody component 
        rb = GetComponent<Rigidbody>();
        // To get the character's animator 
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Island") && !collidedIslands.Contains(other.gameObject))
        {
            collidedIslands.Add(other.gameObject);
            database.SaveData();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Capture user input values for horizontal and vertical movement
        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("Vertical");

        // Rotate the object based on horizontal input
        transform.Rotate(0, x * Time.deltaTime * RotationSpeed, 0);
        // Move the object forward based on vertical input
        transform.Translate(0, 0, y * Time.deltaTime * movementSpeed);
        
        // Update animation parameters with input values
        anim.SetFloat("VelX", x);
        anim.SetFloat("VelY", y);

        // Define the direction downwards from the object's current position
        Vector3 floor = transform.TransformDirection(Vector3.down);
        // Perform a raycast downwards to detect if the object is on the ground
        if (Physics.Raycast(transform.position, floor, 0.6f))
        {
            // The object is in contact with the ground
            floorDetected = true;
            anim.SetBool("landed",true);
        }
        else
        {
            // The object is not in contact with the ground
            floorDetected = false;
            anim.SetBool("landed",false);
            anim.SetBool("jumped", false);
        }

        // Check if the jump button was pressed
        isJumpping = Input.GetButtonDown("Jump");
        // If jump button is pressed and the object is on the ground, apply a jump force
        if (isJumpping && floorDetected)
        {
            rb.AddForce(new Vector3(0,jumpForce,0), ForceMode.Impulse);
        }
            
    }
}
