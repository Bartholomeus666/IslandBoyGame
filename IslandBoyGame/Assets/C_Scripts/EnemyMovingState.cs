using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class EnemyMovingState :IState
{
    private GameObject[] _targets;
    private NavMeshAgent _agent;

    public EnemyMovingState(NavMeshAgent agent, GameObject[] targets)
    {
        _agent = agent;
        _targets = targets;
        Debug.Log(_targets.Length);

    }

    public void OnEnter()
    {
        Debug.Log(_targets.Length);
        _agent.destination = _targets[0].transform.position;
    }

    public void OnExit()
    {

    }

    void IState.Update()
    {

    }
}