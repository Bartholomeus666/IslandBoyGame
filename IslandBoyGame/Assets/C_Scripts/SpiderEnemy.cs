using System;
using UnityEngine;
using UnityEngine.AI;

public class SpiderEnemy : EnemyBase
{
    public EnemyStateMachine StateMachine;

    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private GameObject[] _targets;

    private void Awake()
    {

    }
    public SpiderEnemy()
    {
        EnemyIdleState idle = new EnemyIdleState();
        idle.PlayerInRoom += PlayerInRoom_Invoked;
        StateMachine = new EnemyStateMachine(new EnemyMovingState(_agent, _targets));
    }

    private void PlayerInRoom_Invoked(object sender, EventArgs e)
    {
        //StateMachine.MoveToState();
    }
}