using UnityEngine;

public class SpiderEnemy : EnemyBase
{
    public EnemyStateMachine EnemyStateMachine;

    public SpiderEnemy()
    {
        EnemyStateMachine = new EnemyStateMachine(new EnemyIdleState());
    }
}