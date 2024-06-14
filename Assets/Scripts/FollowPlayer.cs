using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject player;
    private Vector3 offset = new Vector3(0,16,25);
    float Sensitivity = 15f;
    float RotationX = 0f;
    float RotationY = 0f;

    void Start()
    {
        // Cursor.lockState = CursorLockMode.Locked;
    }

    void MouseLook()
    {
        // Get the mouse input values
        float mouseX = Input.GetAxis("Mouse X") * Sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * Sensitivity * -1 * Time.deltaTime;

        // Rotate the object based on the mouse input values
        RotationX -= mouseY;
        RotationY += mouseX;
        transform.localEulerAngles = new Vector3(RotationX, RotationY, 0);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position + offset;
    }

}
