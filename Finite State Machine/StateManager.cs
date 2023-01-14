using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    public StateBase currentState;

    public PatrolState patrolState = new PatrolState();
    public ChaseState chaseState = new ChaseState();
    public AttackState attackState = new AttackState();

    // Start is called before the first frame update
    void Start()
    {
        patrolState = GetComponentInChildren<PatrolState>();
        chaseState = GetComponentInChildren<ChaseState>();
        attackState = GetComponentInChildren<AttackState>();

        currentState = patrolState;

        currentState.EnterState(this);
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateEnemy(this);
    }



    public void ChangeState(StateBase _newState)
    {
        currentState = _newState;
        currentState.EnterState(this);
    }


}


// Referance
// iHeartGameDev. (2021). How to Program in Unity: State Machines Explained.  YouTube.    https://youtu.be/Vt8aZDPzRjI
// Infallible Code. (2020). How to Code a Simple State Machine (Unity Tutorial). YouTube. https://youtu.be/G1bd75R10m4
// 