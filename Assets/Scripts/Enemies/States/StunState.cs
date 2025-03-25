using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunState : State
{
    protected Movement Movement { get => movement ?? core.GetCoreComponent(ref movement); }
    private CollisionManager CollisionManager { get => collisionManager ?? core.GetCoreComponent(ref collisionManager); }

    private Movement movement;
    private CollisionManager collisionManager;
    protected Data_StunState stateData;
    protected bool isStunTimeOver;
    protected bool isGrounded;
    protected bool isMovementStopped;
    protected bool performCloseRangeAction;
    protected bool isPlayerInMinAggroRange;
    public StunState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Data_StunState stateData) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void DoChecks()
    {
        base.DoChecks();
        isGrounded = CollisionManager.Ground;
        performCloseRangeAction = entity.CheckPlayerInCloseRangeAction();
        isPlayerInMinAggroRange = entity.CheckPlayerInMinAggroRange();
    }

    public override void Enter()
    {
        base.Enter();
        isMovementStopped = false;
        isStunTimeOver = false;
        Movement.SetVelocity(stateData.StunKnockbackSpeed, stateData.StunKnockbackAngle, entity.LastDamageDirection);
    }

    public override void Exit()
    {
        base.Exit();
        entity.ResetPoise();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (Time.time >= startTime + stateData.StunDuration)
        {
            isStunTimeOver = true;
        }
        if(isGrounded && Time.time >= startTime + stateData.StunKnockbackDuration && !isMovementStopped)
        {
            isMovementStopped = true;
            Movement.SetVelocityX(0);
        }
    }

    public override void PhsyicsUpdate()
    {
        base.PhsyicsUpdate();
    }
}
