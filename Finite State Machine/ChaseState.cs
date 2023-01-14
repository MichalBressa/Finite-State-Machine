using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : StateBase
{
    public float speed = 4.0f;

    public float attackRange = 0.5f;
    public float looseChaseMeter = 10;

    Transform player;
    Enemy enemy;
    Animator animator;

    Vector3[] pathfindingWaypoints;
    public int currentWaypointIndex = 0;
    Pathfinding pathfinding;

    public override void EnterState(StateManager _stateManager)
    {
        Debug.Log("Chase state entered");
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        enemy = GetComponentInParent<Enemy>();
        pathfinding = Pathfinding.GetInstance();
        animator = GetComponentInParent<Animator>();

    }


    public override void UpdateEnemy(StateManager _stateManager)
    {

        // if raycast can hit player (there are no obstacles) -> go in his direction

        if (Vector3.Distance(enemy.gameObject.transform.position, player.position) <= attackRange)
        {
            _stateManager.ChangeState(_stateManager.attackState);
        }

        if (!animator.GetBool("isAttacking"))
        {
            // dont move when attacking
            RaycastHit hit;
            // else if raycast can't hit player -> pathfinding.FindPath  and  slowly build up looseChaseMeter
            if (Physics.Raycast(transform.position, player.position - transform.position, out hit, 15f))
            {
                if (!hit.collider.CompareTag("Player"))
                {
                    Debug.Log("Can see player");

                    looseChaseMeter -= 2.0f * Time.deltaTime;
                    if (looseChaseMeter <= 0)
                    {
                        _stateManager.ChangeState(_stateManager.patrolState);
                    }

                    // use pathfinding to find the way to the player that is behind the obstacle

                    pathfindingWaypoints = pathfinding.FindPath(enemy.gameObject.transform.position, player.transform.position);

                    enemy.gameObject.transform.position = Vector3.MoveTowards(enemy.gameObject.transform.position, pathfindingWaypoints[currentWaypointIndex], speed * Time.deltaTime);
                    enemy.gameObject.transform.LookAt(pathfindingWaypoints[currentWaypointIndex]);

                    if (Vector3.Distance(pathfindingWaypoints[currentWaypointIndex], transform.position) < 0.1f)
                    {
                        currentWaypointIndex++;

                    }

                }
                else
                {
                    // if player is in sight then go straight at him
                    looseChaseMeter += 5.0f * Time.deltaTime;
                    enemy.gameObject.transform.position = Vector3.MoveTowards(enemy.gameObject.transform.position, player.position, speed * Time.deltaTime);
                    enemy.gameObject.transform.LookAt(player.position);
                    currentWaypointIndex = 0;
                }
            }


        }
        






    }

   

}


