using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//should be attatched to enemies as this will serve as the core of the Artificial Intelligence
[RequireComponent (typeof(NavMeshAgent))]
public class NavMeshMovement : MonoBehaviour
{
    ////editorvars
    public GameObject[] waypoints;
    public GameObject weapon;
    [SerializeField] private int speed = 3;

    ////system vars
    GravityBody playerGrav;
    GravityBody myGrav;
    Sight sight;
    NavMeshAgent navMeshAgent;
    GameObject currentWaypoint;
    GameObject previousWaypoint;
    Rigidbody rb;
    Transform player; //the player
    Transform distraction;
    float timer;
    int x; //pointer
    bool travelling;
    bool isDistracted = false;
    bool random;

    void Start()
    {
        //getting components or objects
        myGrav = GetComponent<GravityBody>();
        sight = GetComponent<Sight>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerGrav = player.GetComponent<GravityBody>();

        x = -1; //first waypoint they visit is point 0

        if (waypoints.Length == 0)
        {
            waypoints = GameObject.FindGameObjectsWithTag("Waypoint");
            random = true;
            GetRandomWaypoint();
        }
        //checks to see if the array "waypoints" is empty
        //if it is, then it should gather all the objects with the tag "Waypoint"
        //and add them all to the array, and randomly select between them

        nextWaypoint(); //asigns next waypoint
    }

    void Update() //is dealing with movement
    {
        ///IS CHASING PLAYER
        if (GetComponent<Targeting>().enabled && playerGrav.planet == myGrav.planet) //if can see player [see Targeting.cs]
        {
            if (travelling || timer > 0) 
            {
                travelling = false; //indicates they are not going to set waypoints
                timer = 0; //resets timer
            }

            switch (tag)
            {
                //aplies solely to rushdown enemy types
                case "Enemy/Rushdown": 
                    RushdownMove(); 
                    break;

                //applies solely to rocket enemy types
                case "Enemy/Rocket":
                    RocketMove();
                    break;

                default:
                    if (sight.alertSign.activeInHierarchy)
                        navMeshAgent.SetDestination(player.position); //sets the navmesh direction to be that of the player
                    else
                        navMeshAgent.SetDestination(transform.position);
                    break;
            }
        }
        else
        {
            if (!navMeshAgent.isActiveAndEnabled)
                navMeshAgent.gameObject.SetActive(true);

            if (!travelling && !isDistracted) //if was chasing player or was distracted (targeting.cs is disabled but so is bool var travelling)
            {
                if (weapon != null)
                    weapon.SetActive(false);
                timer += Time.deltaTime; //increase timer in real time
                sight.confusedSign.SetActive(true); //gives confused look
                navMeshAgent.SetDestination(transform.position); //stand still for a second
                if (timer > 2f)
                {
                    sight.confusedSign.SetActive(false); //disables confused look
                    timer = 0; //resets timer

                    x--;
                    nextWaypoint(); //resets waypoint back to one before they saw the player
                }
            }
        }

        ///IS TRAVELING TO WAYPOINTS
        if (navMeshAgent.enabled && travelling && navMeshAgent.remainingDistance < 1f ) //if enemy has reached destination and isnt chasing player
        {
            if (random) //checks to see if the route is randomised 
                GetRandomWaypoint();
            nextWaypoint();
        }
    }

    /// WAYPOINT TRAVELLING PROCEDURES --------------------------------------------------------------------------------------------
    void nextWaypoint()     //sets waypoint to next item in array
    {
        //increases x by one unless its at the end of the array
        //then it resets it
        if (x != waypoints.Length - 1)
            x++;
        else
            x = 0;

        currentWaypoint = waypoints[x]; //sets next destination to x
        navMeshAgent.SetDestination(currentWaypoint.transform.position); //inputs this into the navMeshAgent
        travelling = true;
    }

    public GameObject GetRandomWaypoint()
    {
        x = Random.Range(0, waypoints.Length); //selects a random value in the array
        return waypoints[x]; //returns it
    }

    ///ENEMY TYPE TRAVELLING PROCEDURES --------------------------------------------------------------------------------------------
    void RushdownMove() 
    {
        if (sight.alertSign.activeInHierarchy)
            navMeshAgent.SetDestination(player.position); //sets the navmesh direction to be that of the player
        else
            navMeshAgent.SetDestination(transform.position); //stand still for a second

        ///ATTACK ACTION
        //if is less than 1.2 units away from player
        if (Physics.Raycast(transform.position, (player.transform.position - transform.position), out RaycastHit RayHit, 1.2f))
        {
            navMeshAgent.SetDestination(transform.position);
            weapon.SetActive(true); //begins attacking
        }
        else
        {
            navMeshAgent.SetDestination(player.position);
            weapon.SetActive(false); //otherwise attack ends
        }
    }

    void RocketMove() 
    {
        if (sight.alertSign.activeInHierarchy && navMeshAgent.enabled) 
        {
            //navMeshAgent.SetDestination(transform.position);
            //navMeshAgent.SetDestination((transform.TransformDirection(transform.position.x, player.position.y, transform.position.z)));
            navMeshAgent.SetDestination(player.position);
            weapon.SetActive(true);
        }

        if (Physics.Raycast(transform.position, (player.transform.position - transform.position), out RaycastHit rayHit, 10f) && rayHit.transform.gameObject.tag == "Player")
        {
            if (rb.isKinematic)
                rb.isKinematic = false;
            if (navMeshAgent.enabled)
                navMeshAgent.enabled = false;
            rb.AddForce(transform.forward * -speed);
        }
        else
        {
            if (!navMeshAgent.enabled)
                navMeshAgent.enabled = true;
            if (!rb.isKinematic)
                rb.isKinematic = true;
        }
    }
}
