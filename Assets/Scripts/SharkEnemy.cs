using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkEnemy : MonoBehaviour
{
    public float speed = 10.0f;
    private bool movement = true;
    public Transform enemyStart;
    private float fromPos = 142.8f;
    private float toPos = -789.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void SharkMovement (){
        //Debug.Log("Posición del tiburón: " + enemy.position.z);
        //Debug.Log("Rotación del tiburón: " + enemy.rotation);
        if (enemyStart){
            if (movement)
            {
                transform.Translate(speed * Time.deltaTime * Vector3.forward);
                if (enemyStart.position.z <= toPos)
                {
                    movement = false;
                    transform.rotation = Quaternion.Euler(0, 0, 0); 
                }
            }
            else
            {
                transform.Translate(speed * Time.deltaTime * Vector3.forward);
                if (enemyStart.position.z >= fromPos)
                {
                    movement = true;
                    transform.rotation = Quaternion.Euler(0, 180, 0); 
                }
                
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        SharkMovement();
    }
}