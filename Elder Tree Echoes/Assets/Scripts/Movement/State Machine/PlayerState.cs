using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    protected PlayerController player;
    protected PlayerStateMachine playerStateMachine;

    public PlayerState(PlayerController pC, PlayerStateMachine pSM)
    {
        player = pC;
        playerStateMachine = pSM;
    }

    public virtual void EnterState() { }
    public virtual void ExitState() { }
    public virtual void FrameUpdate() { }
    public virtual void PhysicsUpdate() { }


    //public virtual void AnimationTriggerEvent() { }


}
