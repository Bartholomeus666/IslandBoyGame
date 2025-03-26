using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class EnemyMovingState :IState
{
    public GameObject[] Targets;
    private NavMeshAgent _agent;

    public EnemyMovingState(NavMeshAgent agent)
    {
        _agent = agent;
    }

    public void OnEnter()
    {
        _agent.destination = Targets[0].transform.position;
    }

    public void OnExit()
    {

    }

    void IState.Update()
    {

    }
}