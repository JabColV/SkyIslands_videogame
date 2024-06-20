using System.Collections;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float speed = 2.0f;
    public float moveDistance = 2.0f;
    public bool moveUp = true; // Determina la dirección inicial de la plataforma

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float direction = moveUp ? 10.0f : -10.0f;
        float newPosY = startPos.y + Mathf.PingPong(Time.time * speed, moveDistance) * direction - (moveDistance / 2.0f) * direction;
        transform.position = new Vector3(transform.position.x, newPosY, transform.position.z);
    }
    // float speedUp = 20.0f; // Velocidad de subida
    // float speedDown = 2.0f; // Velocidad de bajada
    // float moveDistance = 8.0f; // Distancia que los objetos deben moverse hacia arriba y hacia abajo
    // bool moveUp = true; // Indica si la plataforma debe moverse hacia arriba o hacia abajo
    // public float seg = 2.0f; // Tiempo de espera entre subida y bajada

    // private Vector3 startPos;
    // private bool moving = true;

    // void Start()
    // {
    //     startPos = transform.position;
    //     StartCoroutine(MovePlatform());
    // }

    // IEnumerator MovePlatform()
    // {
    //     while (moving)
    //     {
    //         Vector3 targetPos;

    //         if (moveUp)
    //         {
    //             // yield return new WaitForSeconds(seg); 
    //             // Bajar lentamente
    //             targetPos = startPos;
    //             while (transform.position.y > targetPos.y)
    //             {
    //                 float newY = transform.position.y - speedDown * Time.deltaTime;
    //                 transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    //                 yield return null;
    //             }
    //             moveUp = true;
    //         }
    //         else
    //         {
    //             yield return new WaitForSeconds(seg); 
    //             // Subir lentamente
    //             targetPos = startPos + Vector3.up * moveDistance;
    //             while (transform.position.y < targetPos.y)
    //             {
    //                 float newY = transform.position.y + speedUp * Time.deltaTime;
    //                 transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    //                 yield return null;
    //             }
    //             moveUp = false;
    //         }
    //     }
    // }
}

