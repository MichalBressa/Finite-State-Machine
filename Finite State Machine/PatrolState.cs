using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : StateBase
{
    float minimumDetectionAngle = -50.0f;
    float maximumDetectionAngle = 50.0f;

    Pathfinding pathfinding;

    bool aboutToGetFirstWaypoints = true;

    Transform player;
    Vector3 target;

    public int currentWaypointIndex = 0;
    public int currentPatrolWaypoint = 1;

    [SerializeField]
     float detectionMeter = 0f;

    public GameObject[] patrolWaypoints;
    Vector3[] pathfindingWaypoints;

    Enemy enemy;
    float speed = 2.0f;

    private void Start()
    {
        pathfinding = Pathfinding.GetInstance();
        
        player = GameObject.FindGameObjectWithTag("Player").transform;
        enemy = GetComponentInParent<Enemy>();
        
       // pathfindingWaypoints = enemy.pathfinding.FindPath(transform.position, patrolWaypoints[0].transform.position); // get initial path
    }

    public override void EnterState(StateManager _stateManager)
    {
        Debug.Log("Patrol state entered");     
        
    }

    public override void UpdateEnemy(StateManager _stateManager)
    {
        // Patrol - going between waypoints

        // get first waypoints array here (for some reason didnt work in EnterState)
        if (aboutToGetFirstWaypoints)
        {
            pathfindingWaypoints = pathfinding.FindPath(enemy.gameObject.transform.position, patrolWaypoints[0].transform.position); // get initial path
            //System.Array.Reverse(pathfindingWaypoints);
            aboutToGetFirstWaypoints=false;
        }
        //Debug.Log(pathfindingWaypoints.Length);
        //Debug.Log(currentWaypointIndex);



enemy.gameObject.transform.position = Vector3.MoveTowards(enemy.gameObject.transform.position, pathfindingWaypoints[currentWaypointIndex], speed * Time.deltaTime);
        enemy.gameObject.transform.LookAt(pathfindingWaypoints[currentWaypointIndex]);

        if (Vector3.Distance(pathfindingWaypoints[currentWaypointIndex], transform.position) < 0.1f)
        {
            currentWaypointIndex++;        
            
            if (Vector3.Distance(pathfindingWaypoints[pathfindingWaypoints.Length - 1], transform.position) < 0.2f)
            {
            // if is very close to the end of path created by pathfinding script -> find new path to next patrol point
            currentPatrolWaypoint++;
            if (currentPatrolWaypoint > patrolWaypoints.Length-1)
            {
                    currentPatrolWaypoint = 0;
            }
            pathfindingWaypoints = pathfinding.FindPath(transform.position, patrolWaypoints[currentPatrolWaypoint].transform.position);
            //System.Array.Reverse(pathfindingWaypoints);
            currentWaypointIndex = 0;
            }
        }

        // Detecting Player
        CanSeePlayer(player);

        if (detectionMeter >= 10.0f)
        {
            // if meter is fully filled -> player is detected and must be chased
            _stateManager.ChangeState(_stateManager.chaseState);
        }

    }

    void CanSeePlayer(Transform player)
    {

        Vector3 targetDirection = player.position - transform.position;

        float fov = Vector3.Angle(targetDirection, enemy.gameObject.transform.forward);
        //Debug.Log(fov);
        RaycastHit hit; // var to store hit info

        // by comapring angle between player and AI I determin if player is in AI's FOV
        // also need to check if player is not hidden behind wall
        if (fov > minimumDetectionAngle && fov < maximumDetectionAngle &&
            Physics.Raycast(transform.position, player.position - transform.position, out hit, 15f))
        {
            //!player.GetComponent<Player>().isHidden
            if (hit.collider.CompareTag("Player"))
            {
                Debug.Log("Can see player");
                detectionMeter += 5.0f * Time.deltaTime;
                //Debug.Log(detectionMetter);
            }
        }
        else
        {
            detectionMeter -= 0.3f * Time.deltaTime;
            if (detectionMeter < 0)
            {
                detectionMeter = 0;
            }
        }
        
    }






}
