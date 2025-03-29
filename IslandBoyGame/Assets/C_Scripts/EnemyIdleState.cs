using System;
using UnityEngine;

public class EnemyIdleState : IState
{
    public EventHandler PlayerInRoom;

    public void OnEnter()
    {
        PlayerInRoom.Invoke(this, EventArgs.Empty);
    }

    public void OnExit()
    {

    }

    public void Update()
    {

    }
}
