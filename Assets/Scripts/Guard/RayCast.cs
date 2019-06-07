using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCast : MonoBehaviour
{
    private GameObject Player;
    public GameObject GuardBoi;
    bool playerFound;
    string hitFeedback;
    public LineRenderer laserLineRenderer;
    public float laserMaxLength = 5f;

    public float interactionRayLength = 5.0f;


    // Start is called before the first frame update
    void Start()
    {
        GameObject player = GameObject.Find("Player");

    }

    // Update is called once per frame
    void Update()
    {
        InterractRaycast();
        
     
    }
    void InterractRaycast()
    { 
        Vector3 playerPosition = transform.position;
        Vector3 forwardDirection = transform.forward;

        Ray interactionRay = new Ray(playerPosition, forwardDirection);
        RaycastHit interactionRayHit;
        

        Vector3 interactionRayEndpoint = forwardDirection * interactionRayLength;
        Debug.DrawLine(playerPosition, interactionRayEndpoint);
        bool hitFound = Physics.Raycast(interactionRay, out interactionRayHit, interactionRayLength);



        if (hitFound)
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * interactionRayHit.distance, Color.yellow);
            GameObject hitGameobject = interactionRayHit.transform.gameObject;
            string hitFeedback = hitGameobject.name;
            Debug.Log(hitFeedback);
            if (hitFeedback == "Player")
            {
                
                    Debug.Log(hitFeedback);
                    GuardBoi.GetComponent<GuardBoi>().enabled = true;
  
            }
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
            string nothingHitFeedback = "-";
            Debug.Log(nothingHitFeedback);
        }
    }



    Vector3 DetectHit(Vector3 startPos, float distance, Vector3 direction)
    {
        Ray ray = new Ray(startPos, direction);
        RaycastHit hit;
        Vector3 endPos = startPos + (distance * direction);

        if (Physics.Raycast(ray, out hit, distance))
        {
            endPos = hit.point;
            return endPos;
        }

        return endPos;
    }


    //}

}
