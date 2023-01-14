using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class StateBase : MonoBehaviour
{
    abstract public void UpdateEnemy(StateManager _stateManager);
    abstract public void EnterState(StateManager _stateManager);

}
