using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRight : MonoBehaviour
{
    public float speed = 15.0f;
    private bool movement = true;
    public Transform enemyRight;
    private float from = -46.0f;
    private float to = 39.9f;
    // Start is called before the first frame update
    void Start()
    {
    }

    void EnemyMovement()
    {

        // Verifica si 'enemy' ha sido asignada
        if (enemyRight)
        {
            //Debug.Log("enemyRight.position.x: " + enemyRight.position.x);
            if (movement)
            {
                transform.Translate(speed * Time.deltaTime * -Vector3.forward);
                if (enemyRight.position.x >= to)
                {
                    movement = false;
                    transform.rotation = Quaternion.Euler(0, 90, 0);
                }
            }
            else
            {
                transform.Translate(speed * Time.deltaTime * -Vector3.forward);
                if (enemyRight.position.x <= from)
                {
                    movement = true;
                    transform.rotation = Quaternion.Euler(0, -90, 0);
                }
            }

        }
    }
    // Update is called once per frame
    void Update()
    {
        EnemyMovement();
    }
}
