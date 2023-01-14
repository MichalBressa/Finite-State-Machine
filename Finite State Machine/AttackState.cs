using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : StateBase
{
    //I'll use animator to start sword swingig and stuff
    Animator animator;
    private void Start()
    {
        animator = GetComponentInParent<Animator>();
    }
    public override void EnterState(StateManager _stateManager)
    {
        // trigger attack animation
        Debug.Log("Attack State entered");
        animator.SetBool("isAttacking", true);
    }

    public override void UpdateEnemy(StateManager _stateManager)
    {
        // if attack animation is finished go back to chase state
        if (!animator.GetBool("isAttacking"))
        {
            _stateManager.ChangeState(_stateManager.chaseState);
        }
        
    }


}
