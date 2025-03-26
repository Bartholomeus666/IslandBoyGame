using System;
using UnityEngine;
using UnityEngine.AI;

public class SpiderEnemy : EnemyBase
{
    public EnemyStateMachine StateMachine;

    [SerializeField] private NavMeshAgent _agent;

    private void Awake()
    {
        _agent = new NavMeshAgent();
    }
    public SpiderEnemy()
    {
        EnemyIdleState idle = new EnemyIdleState();
        idle.PlayerInRoom += PlayerInRoom_Invoked;
        StateMachine = new EnemyStateMachine(idle);
    }

    private void PlayerInRoom_Invoked(object sender, EventArgs e)
    {
        StateMachine.MoveToState(new EnemyMovingState(_agent));
    }
}