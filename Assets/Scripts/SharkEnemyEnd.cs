using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkEnemyEnd : MonoBehaviour
{
    public float speed = 20.5f;
    private bool movement = true;
    public Transform enemyEnd;
    private float fromPos = -789.0f;
    private float toPos = 142.8f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void SharkMovement (){
        //Debug.Log("Posición del tiburón: " + enemy.position.z);
        //Debug.Log("Rotación del tiburón: " + enemy.rotation);
        if (enemyEnd){
            if (movement)
            {
                transform.Translate(speed * Time.deltaTime * Vector3.forward);
                transform.Translate(40*Mathf.Cos(Time.time) * Time.deltaTime * Vector3.left);
                if (enemyEnd.position.z >= toPos)
                {
                    movement = false;
                    transform.rotation = Quaternion.Euler(0, 180, 0); 
                }
            }
            else
            {
                transform.Translate(speed * Time.deltaTime * Vector3.forward);
                transform.Translate(50*Mathf.Cos(Time.time) * Time.deltaTime * Vector3.left);
                if (enemyEnd.position.z <= fromPos)
                {
                    movement = true;
                    transform.rotation = Quaternion.Euler(0, 0, 0); 
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