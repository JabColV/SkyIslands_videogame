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
    private Animator anim;
    public Transform player;
    // To store the singleton pattern instance
    SingletonPattern singletonPattern;
    // Start is called before the first frame update
    void Start()
    {
        singletonPattern = SingletonPattern.Instance;
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Colision con el tiburón");
        }
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

    void SharkAtack(){
        transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
    }
    // Update is called once per frame
    void Update()
    {
        // Debug.Log("Player: " + singletonPattern.GetPlayer());
        if (Vector3.Distance(transform.position, player.position) > 5.0f){
            SharkMovement();
        }
        else
        {
            Debug.Log("Atacando");
            SharkAtack();
        }
    }
}