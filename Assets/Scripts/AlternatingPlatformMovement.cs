using System.Collections;
using UnityEngine;

public class AlternatingPlatformMovement : MonoBehaviour
{
    public GameObject[] rows; // Array de GameObjects que contienen las filas de objetos puntiagudos
    public float speedUp = 20.0f; // Velocidad de subida
    public float speedDown = 2.0f; // Velocidad de bajada
    public float moveDistance = 8.0f; // Distancia que los objetos deben moverse hacia arriba y hacia abajo
    public float waitTime = 1.0f; // Tiempo de espera en la posición superior e inferior
    public AudioClip knifeAudio;
    SingletonPattern singletonPattern;

    private Vector3[] startPos; // Posiciones iniciales de las filas

    void Start()
    {
        singletonPattern = SingletonPattern.Instance;
        // Guardar las posiciones iniciales
        startPos = new Vector3[rows.Length];
        for (int i = 0; i < rows.Length; i++)
        {
            startPos[i] = rows[i].transform.position;
        }

        // Comenzar las corrutinas para cada fila
        for (int i = 0; i < rows.Length; i++)
        {
            if (i % 2 == 0)
            {
                StartCoroutine(MoveRow(rows[i], true)); // Filas pares comienzan subiendo
            }
            else
            {
                StartCoroutine(MoveRow(rows[i], false)); // Filas impares comienzan bajando
            }
        }
    }

    IEnumerator MoveRow(GameObject row, bool moveUp)
    {
        while (true)
        {
            if (moveUp)
            {
                singletonPattern.PlaySoundEffect(knifeAudio, 1.0f);
                // Subir rápidamente
                Vector3 targetPos = startPos[System.Array.IndexOf(rows, row)] + Vector3.up * moveDistance;
                while (row.transform.position.y < targetPos.y)
                {
                    row.transform.position += Vector3.up * speedUp * Time.deltaTime;
                    yield return null;
                }
                yield return new WaitForSeconds(waitTime); 
                moveUp = false;
            }
            else
            {
                // Bajar lentamente
                Vector3 targetPos = startPos[System.Array.IndexOf(rows, row)];
                while (row.transform.position.y > targetPos.y)
                {
                    row.transform.position += Vector3.down * speedDown * Time.deltaTime;
                    yield return null;
                }
                yield return new WaitForSeconds(waitTime+5.0f); 
                moveUp = true;
            }
        }
    }
}
