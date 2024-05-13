using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLeft : MonoBehaviour
{
    public float speed = 15.0f;
    private bool movement = true;
    public Transform enemyLeft;
    private float from = 39.9f;
    private float to = -46.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void EnemyMovement (){

        // Verifica si 'enemy' ha sido asignada
        if (enemyLeft){
            if (movement)
            {
                transform.Translate(speed * Time.deltaTime * -Vector3.forward);
                if (enemyLeft.position.x <= to)
                {
                    movement = false;
                    transform.rotation = Quaternion.Euler(0, -90, 0); 
                }
            }
            else
            {
                transform.Translate(speed * Time.deltaTime *  -Vector3.forward);
                if (enemyLeft.position.x >= from)
                {
                    movement = true;
                    transform.rotation = Quaternion.Euler(0, 90, 0); 
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
