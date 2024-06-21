using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleMovement : MonoBehaviour
{
    float radius = 40.0f; // Radio del círculo
    public float speed = 2.0f; // Velocidad del tiburón
    private float angle = 0.0f; // Ángulo inicial del tiburón
    public Transform centerPoint; // Punto central del círculo

    void Start()
    {
        // Si no se ha asignado un punto central, usa la posición inicial del tiburón como el centro
        if (centerPoint == null)
        {
            centerPoint = new GameObject("Gem4").transform;
            centerPoint.position = transform.position;
        }
    }

    void Update()
    {
        // Incrementa el ángulo basado en la velocidad y el tiempo
        angle += speed * Time.deltaTime;

        // Calcula la nueva posición del tiburón en el círculo
        float x = Mathf.Cos(angle) * radius;
        float z = Mathf.Sin(angle) * radius;
        Vector3 newPos = new Vector3(x, 0, z) + centerPoint.position;

        // Calcula la dirección en la que el tiburón se está moviendo
        Vector3 direction = newPos - transform.position;
        if (direction != Vector3.zero)
        {
            // Ajusta la rotación del tiburón para que apunte en la dirección del movimiento
            Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * speed);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * speed);
        }

        // Actualiza la posición del tiburón
        transform.position = newPos;
    }
}
