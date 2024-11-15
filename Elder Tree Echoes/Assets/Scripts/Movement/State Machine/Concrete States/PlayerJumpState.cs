using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerState
{
    public PlayerJumpState(PlayerController pC, PlayerStateMachine pSM) : base(pC, pSM)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
       // Debug.Log("jumping");

        if (player.IsGrounded())
        {
            player.StateMachine.ChangeState(player.IdleState);
        }

        base.FrameUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        player.HandleMovement();
        player.HandleJump();
    }
}
