using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject player; // Referencia al jugador
    public Vector3 lateralOffset = new Vector3(-50, 10, -20); // Desplazamiento para la cámara lateral
    public Vector3 thirdPersonOffset = new Vector3(0, 15, 35);
    public float smoothSpeed = 0.0025f; // Factor de suavizado (ajústalo según sea necesario)
    public float rotationSmoothSpeed = 0.01f; // Factor de suavizado para la rotación
    private Quaternion targetRotation; // Rotación objetivo de la cámara
    SingletonPattern singletonPattern;

    void Start()
    {
        singletonPattern = SingletonPattern.Instance;
    }

     void LateUpdate()
    {
        // Calcula la posición deseada de la cámara según el modo
        Vector3 desiredPosition = player.transform.position + (singletonPattern.GetIsInWater() ? thirdPersonOffset : lateralOffset);
        // Vector3 desiredPosition = player.transform.position + (true ? thirdPersonOffset : lateralOffset);

        // Interpola suavemente entre la posición actual y la deseada
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Actualiza la posición de la cámara
        transform.position = smoothedPosition;

        // Calcula la rotación deseada de la cámara según el modo
        if (singletonPattern.GetIsInWater())
        // if (true)
        {
            // Cámara en tercera persona: mira hacia el jugador desde atrás
            targetRotation = Quaternion.LookRotation(player.transform.position - transform.position, Vector3.up);
        }
        else
        {
            // Cámara lateral: mantén una rotación fija
            targetRotation = Quaternion.Euler(0, 90, 0);
        }

        // Interpola suavemente entre la rotación actual y la deseada
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSmoothSpeed);
    }
}