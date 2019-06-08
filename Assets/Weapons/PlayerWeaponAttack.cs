using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponAttack : MonoBehaviour
{
    public int lightDamage = 20;
    public int heavyDamage = 50;
    public int appliedDmg;
    public bool isSwinging;
    public Animator anim;
    public float heavyAttkChargeTime = 0.3f;
    public bool heavycharging;
    GameObject Target;
    int swingHash = Animator.StringToHash("SmackAttack");


    // Start is called before the first frame update
    void Start()
    {
        anim.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        AttackDecision();
    }

   
    private void OnTriggerEnter(Collider other)
    {
        if (isSwinging)
        {
            if (other.gameObject.name == "Guard")
            {
                other.gameObject.GetComponent<GuardBoi>().ApplyDamage(appliedDmg);
            }
        } 

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

        anim.SetTrigger(swingHash);
        appliedDmg = lightDamage;

    }

    void HeavyAttack()
    {

        appliedDmg = heavyDamage;
        heavyAttkChargeTime = 0.3f;
    }


}







