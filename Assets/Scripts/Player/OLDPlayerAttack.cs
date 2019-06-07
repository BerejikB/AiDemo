using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public int lightDamage = 20;
    public int heavyDamage = 50;
    public int appliedDmg;
    public float distance = 1;
    public float targetDist;
    public float heavyAttkChargeTime = 0.3f;
    public bool heavycharging;
    public string hitFeedback;
    GameObject Target;
    RaycastHit hit;
    
   
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {

        AttackDecision();
        Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), distance);
    }

    void AttackDecision()
    {

        if (Input.GetButtonDown("Fire1"))
        {
            LightAttack();
        }

        if (Input.GetButtonDown("Fire2"))
        {

            heavycharging = true;

        }
        if (heavycharging)
        {

            heavyAttkChargeTime -= Time.deltaTime;
            if (heavyAttkChargeTime <= 0 && Input.GetButtonUp("Fire2"))
            {
                HeavyAttack();
                heavycharging = false;

            }
            if (Input.GetButtonUp("Fire2"))
            {
                heavycharging = false;
                heavyAttkChargeTime = 0.3f;
            }


        }
    }


    void LightAttack()
    {
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * distance, Color.red);
               
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), distance))
            {
                // GameObject Target = hit.collider.gameObject;
                if (hit.distance <= distance)
                {
                    appliedDmg = lightDamage;
                    Target = hit.transform.gameObject;
                    Target.SendMessage("ApplyDamage", appliedDmg, SendMessageOptions.DontRequireReceiver);
                    //hit.transform.gameObject.SendMessage("ApplyDamage", appliedDmg, SendMessageOptions.DontRequireReceiver);
                }

            }
        

    }

    void HeavyAttack()
    {


        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * distance, Color.red);
        targetDist = hit.distance;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), distance))
        {
            // GameObject Target = hit.collider.gameObject;
            if (hit.distance <= distance)
            {
                appliedDmg = heavyDamage;
                hit.transform.gameObject.SendMessage("ApplyDamage", appliedDmg, SendMessageOptions.DontRequireReceiver);
            }

            heavyAttkChargeTime = 0.3f;
        }


    }


}




