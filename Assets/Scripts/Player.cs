using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody rb;
    
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        gameObject.tag = "Player";
        gameObject.name = "Player";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
