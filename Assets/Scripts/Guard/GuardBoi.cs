using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GuardBoi : MonoBehaviour
{
    public bool isChasing = false;
    public bool isSpooked = false;
    public bool standTimerRunning = false;
    public float distanceToPlayer;
    private float reset = 5;
    public float offset = 1;
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

    void Start ()
    {
        animate = GetComponent<Animator>();
        navmesh = GetComponent<NavMeshAgent>();

        Player = GameObject.Find("Player");
        wanderPoint = WanderPointCalc();
    }

    void Update()
    {
        ScanForPlayer();
        Player = GameObject.Find("Player");
        distanceToPlayer = Vector3.Distance(Guardo.transform.position, Player.transform.position);
        if (isChasing)
        {
            ChaseStage();
            navmesh.SetDestination(Player.transform.position);
            colorchanger.material.color = Color.red;
        }
        if (isSpooked)
        {
            Spooked();
        }
        else 
        {

            MovementDecision();
            Reset();
            colorchanger.material.color = Color.blue;
        }
    }

    void ChaseOn()
    {
        isChasing = true;
        

    }
   
    void Reset()
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
            int switch_on = rnd.Next(1,4);
            
            standTimerRunning = false;
            switch (switch_on)

            {
                case 1:
                    standTimerRunning = true;
                    standTimer = rnd.Next(1,5);
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
            animate.Play("HumanoidWalk");
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
                animate.Play("HumanoidIdle");
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
                Debug.DrawRay(transform.position + (Vector3.up * offset), transform.TransformDirection(Vector3.forward * maxDistance),  Color.red);
                
            }
            else
            {
                Debug.Log("---");
            }
        }

    }
    void Stalk()
    {
        animate.Play("HumanoidCrouch");
        navmesh.speed = 0.3f;
        navmesh.SetDestination(Player.transform.position);
        colorchanger.material.color = Color.yellow;

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
        animate.Play("HumanoidRun");
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
}

