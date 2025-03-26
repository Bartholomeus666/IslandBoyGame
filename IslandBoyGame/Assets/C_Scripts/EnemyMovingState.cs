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
    }

    public void OnEnter()
    {
        _agent.destination = _targets[0].transform.position;
    }

    public void OnExit()
    {

    }

    void IState.Update()
    {

    }
}