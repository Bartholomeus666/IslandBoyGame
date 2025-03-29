using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class EnemyMovingState :IState
{
    private GameObject[] _targets;
    private NavMeshAgent _agent;
    private CharacterController _player;

    public EnemyMovingState(NavMeshAgent agent, GameObject[] targets)
    {
        _agent = agent;
        _targets = targets;
        _player = GameObject.FindAnyObjectByType<CharacterController>();
    }

    public void OnEnter()
    {
        Debug.Log(_targets.Length);
        _agent.destination = _targets[0].transform.position;
    }

    public void OnExit()
    {

    }

    public void Update()
    {
        _agent.destination = FindDestinationAwayFromPlayer().transform.position;
    }

    private GameObject FindDestinationAwayFromPlayer()
    {
        float distance = 0;
        GameObject transform = null;

        foreach(GameObject target in _targets)
        {
            if(Vector3.Distance(_player.transform.position, target.transform.position) > distance)
            {
                distance = Vector3.Distance(_player.transform.position, target.transform.position);
                transform = target;
            }
        }
        Debug.Log(distance);
        Debug.Log(transform.name);
        return transform;
    }
}