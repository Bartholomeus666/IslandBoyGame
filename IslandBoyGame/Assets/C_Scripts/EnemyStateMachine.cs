using UnityEngine;

public class EnemyStateMachine
{
    private IState _currentState;

    public EnemyStateMachine(IState startState)
    {
        _currentState = startState;
        _currentState.OnEnter();
    }

    public void Update()
    {
        _currentState.Update();
    }

    public void MoveToState(IState nextState)
    {
        _currentState.OnExit();
        _currentState = nextState;
        _currentState.OnEnter();
    }
}
public enum EnemyState
{
    Idle,
    Walking,
    Chasing,
    Attacking
}
