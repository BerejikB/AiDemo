using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseRangeDetect : MonoBehaviour
{
    GameObject GuardBoi;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

      void OnCollisionEnter(Collision collision)
    {          
        if (collision.collider.tag == "Player")
        {
            bool spookOn = true;
            transform.parent.GetComponent<GuardBoi>().Spook(spookOn);
        }
    }


       
    
}
