using System;
using UnityEngine;
using UnityEngine.AI;

public class SpiderEnemy : MonoBehaviour
{
    public EnemyStateMachine StateMachine;
    private NavMeshAgent _agent;
    [SerializeField] private GameObject[] _targets;

    private void Awake()
    {
        // Get the NavMeshAgent component
        _agent = GetComponent<NavMeshAgent>();

        // Initialize state machine after components are available
        InitializeStateMachine();
    }

    private void InitializeStateMachine()
    {
        if (_agent == null)
        {
            Debug.LogError("NavMeshAgent component not found on " + gameObject.name);
            return;
        }

        // Initialize states
        EnemyIdleState idle = new EnemyIdleState();
        idle.PlayerInRoom += PlayerInRoom_Invoked;

        // Create state machine with initial state
        StateMachine = new EnemyStateMachine(new EnemyMovingState(_agent, _targets));
    }

    private void PlayerInRoom_Invoked(object sender, EventArgs e)
    {
        //StateMachine.MoveToState();
    }

    //// Unity uses OnDestroy for cleanup
    //private void OnDestroy()
    //{
    //    // Clean up event subscriptions if needed
    //    if (StateMachine != null && StateMachine.CurrentState is EnemyIdleState idleState)
    //    {
    //        idleState.PlayerInRoom -= PlayerInRoom_Invoked;
    //    }
    //}
}