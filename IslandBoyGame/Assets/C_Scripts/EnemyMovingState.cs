using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class EnemyMovingState : MonoBehaviour, IState
{
    private NavMeshAgent agent;
    public GameObject target;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.destination = target.transform.position;
    }

    public void OnEnter()
    {

    }

    public void OnExit()
    {

    }

    void IState.Update()
    {
        agent.destination = target.transform.position;
    }
}