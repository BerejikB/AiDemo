using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GuardBoi : MonoBehaviour
{
    public bool isChasing = false;
    public bool isSpooked = false;
    public bool isRetreating = false;
    public bool standTimerRunning = false;
    public bool isAngry = false;
    bool inHitRange = false;
    public float distanceToPlayer;
    private float reset = 5;
    public float offset = 1;
    public int guardHealth = 100;
    public float wanderSphere;
    public float maxDistance = 90.0f;
    public float stage1Dist = 30;
    public float stage2Dist = 20;
    public float stage3Dist = 10;
    public float stalkDist = 50;
    public float pauseTime = 3;
    public float standTimer;
    public Vector3 wanderPoint;
    public Renderer colorchanger;
    private GameObject Player;
    public GameObject Guardo;
    public GameObject CloseRangeDetect;
    Animator animate;
    private NavMeshAgent navmesh;
    System.Random rnd = new System.Random();
    NavMeshHit navHit;

    void Start()
    {
        animate = GetComponent<Animator>();
        animate.StartPlayback();
        navmesh = GetComponent<NavMeshAgent>();
        Player = GameObject.Find("Player");
        wanderPoint = WanderPointCalc();
    }

    void Update()
    {
        if (guardHealth <= 0)
        {
            Dead();
        }
        ScanForPlayer();
        Player = GameObject.Find("Player");
        distanceToPlayer = Vector3.Distance(Guardo.transform.position, Player.transform.position);
        BehaviourDecision();
    }

    void ChaseOn()
    {
        isChasing = true;
    }
    void Reseto()
    {
        if (isChasing)
        {
            Debug.Log(reset);

            reset -= Time.deltaTime;
            if (reset <= 0)
            {

                reset = 5;
                Debug.Log(reset);
                Debug.Log("RESETTING");
                isChasing = false;
            }
        }

    }
    public void ApplyDamage(int appliedDmg)
    {
        guardHealth -= appliedDmg;
        isSpooked = true;
    }
    void BehaviourDecision()
    {
        if (isRetreating)
        {
            Retreating();
        }
        if (isAngry)
        {
            Angry();
        }
        if (isChasing)
        {
            ChaseStage();
            Chase();
        }
        if (isSpooked)
        {
            Spooked();
        }
        else
        {
            MovementDecision();
            Reseto();
            colorchanger.material.color = Color.blue;
        }
    }
    public Vector3 WanderPointCalc()
    {
        Vector3 randomPoint = (Random.insideUnitSphere * wanderSphere) + transform.position;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randomPoint, out navHit, wanderSphere, -1);
        randomPoint = wanderPoint;
        return new Vector3(navHit.position.x, transform.position.y, navHit.position.z);
    }
    void MovementDecision()
    {


        if (standTimerRunning)
        {
            if (standTimer <= 0)
            {
                standTimerRunning = false;
            }
            else
            {

                Stand();

            }

        }
        else
        {
            int switch_on = rnd.Next(1, 4);

            standTimerRunning = false;
            switch (switch_on)

            {
                case 1:
                    standTimerRunning = true;
                    standTimer = rnd.Next(1, 5);
                    Stand();
                    break;

                default:
                    WanderPointCalc();
                    Wander();
                    break;
            }

        }



    }
    void Wander()
    {

        if (Vector3.Distance(transform.position, wanderPoint) < 1f)
        {
            wanderPoint = WanderPointCalc();
        }

        else
        {

            navmesh.speed = 0.5f;
            navmesh.SetDestination(wanderPoint);
        }
    }
    void Stand()
    {

        if (standTimerRunning)
        {
            standTimer -= Time.deltaTime;
            if (standTimer <= 0)
            {
                standTimerRunning = false;
            }
            else
            {

                return;
            }
        }


    }
    void ChaseStage()
    {
        if (isChasing)
        {
            if (distanceToPlayer >= maxDistance)
            {
                isChasing = false;
            }
            if (distanceToPlayer >= stalkDist)
            {
                Stalk();
            }
            if (distanceToPlayer <= stage1Dist)
            {
                navmesh.speed = 1.5f;
            }
            if (distanceToPlayer <= stage2Dist)
            {
                animate.Play("HumanoidRun");
                navmesh.speed = 3f;
            }
            if (distanceToPlayer <= stage3Dist)
            {
                navmesh.speed = 6f;
            }

        }

    }
    void ScanForPlayer()
    {


        Ray visScan = new Ray(transform.position + (Vector3.up * offset), transform.TransformDirection(Vector3.forward));
        Debug.DrawRay(transform.position + (Vector3.up * offset), transform.TransformDirection(Vector3.forward * maxDistance), Color.yellow);
        RaycastHit hit;


        if (Physics.Raycast(visScan, out hit, maxDistance))
        {


            if (hit.collider.tag == "Player")
            {
                isChasing = true;
                Debug.Log("Player");
                Debug.DrawRay(transform.position + (Vector3.up * offset), transform.TransformDirection(Vector3.forward * maxDistance), Color.red);

            }
            else
            {
                Debug.Log("---");
            }
        }

    }
    void Stalk()
    {

        navmesh.speed = 0.3f;
        navmesh.SetDestination(Player.transform.position);
        colorchanger.material.color = Color.yellow;

    }
    void Retreating()
    {

        animate.Play("HumanoidRun");
        navmesh.speed = 5.5f;
        transform.rotation = Quaternion.LookRotation(transform.position - Player.transform.position);
        Vector3 runTo = transform.position + transform.forward * (maxDistance - 30);
        navmesh.SetDestination(runTo);
        colorchanger.material.color = Color.gray;
        if (distanceToPlayer <= maxDistance)
        {
            isRetreating = false;
            isAngry = true;
            Angry();
        }

    }
    void Angry()
    {
        if (inHitRange)
        {
            Attack();
        }
        else
        {

            colorchanger.material.color = Color.yellow;
            int switch_on = rnd.Next(1, 3);
            switch (switch_on)
            {
                default:
                    ChaseOn();
                    Chase();
                    break;

                case 3:
                    navmesh.speed = 6.0f;
                    navmesh.SetDestination(WanderPointCalc());
                    break;
            }
        }
    }
    void Chase()
    {

        navmesh.SetDestination(Player.transform.position);
        colorchanger.material.color = Color.red;
        if (distanceToPlayer >= 5f)
        {
            Attack();
        }
    }
    public void Spook(bool spookOn)
    {
        if (!isChasing)
        {
            isSpooked = spookOn;
        }

    }
    void Spooked()
    {

        navmesh.speed = 5.5f;
        transform.rotation = Quaternion.LookRotation(transform.position - Player.transform.position);
        Vector3 runTo = transform.position + transform.forward * (maxDistance - 10);
        navmesh.SetDestination(runTo);
        colorchanger.material.color = Color.green;
        if (distanceToPlayer >= maxDistance)
        {
            isSpooked = false;
            Stalk();
        }
    }
    void Attack()
    {
        int switch_on = rnd.Next(1, 2);
        switch (switch_on)
        {
            case 1:
                //insert light attack here;
                Vector3 runTo = transform.position + transform.forward * 4f;
                navmesh.SetDestination(runTo);
                if (inHitRange)
                {
                    //fast attack animation
                    transform.parent.GetComponent<Player>().playerHealth -= 10;
                }
                //transform.rotation = Quaternion.LookRotation(transform.position - Player.transform.position);
                break;

            case 2:
                if (inHitRange)
                {
                    //slow attack animation
                    transform.parent.GetComponent<Player>().playerHealth -= 30;
                }
                isRetreating = true;
                break;
        }


    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Player")
        {
            inHitRange = true;
        }
        else
        {
            inHitRange = false;
        }
    }

    void Dead()
    {
        Destroy(Guardo);
    }
}

