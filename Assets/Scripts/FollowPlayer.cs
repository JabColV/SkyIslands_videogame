using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject cameraOne;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            // Activa la cámara uno
            cameraOne.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            cameraOne.SetActive(false);
        }
    }

}
