using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody rb;
    public int playerHealth;
    bool inHitRange = false;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        gameObject.tag = "Player";
        gameObject.name = "Player";
        playerHealth = 100;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerHealth == 0)
        {
            //restart scene

        }

    }


    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Guard")
        {
            inHitRange = true;

        }
        else
        {
            inHitRange = false;
        }
    }

    void Attack()
    {

        if (inHitRange)
        {
            transform.parent.GetComponent<GuardBoi>().isRetreating = true;
            transform.parent.GetComponent<GuardBoi>().guardHealth -= 10; 
        }

    }
}
