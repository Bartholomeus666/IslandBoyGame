using UnityEngine;

public class EnemyStateMachine
{
    public IState CurrentState;

    public EnemyStateMachine(IState startState)
    {
        CurrentState = startState;
        CurrentState.OnEnter();
    }

    public void Update()
    {
        CurrentState.Update();
    }

    public void MoveToState(IState nextState)
    {
        CurrentState.OnExit();
        CurrentState = nextState;
        CurrentState.OnEnter();
    }
}
public enum EnemyState
{
    Idle,
    Walking,
    Chasing,
    Attacking
}
