using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : State
{
    protected Movement Movement { get => movement ?? core.GetCoreComponent(ref movement); }
    private CollisionManager CollisionManager { get => collisionManager ?? core.GetCoreComponent(ref collisionManager); }

    private Movement movement;
    private CollisionManager collisionManager;

    protected Data_MoveState stateData;
    protected bool isDetectingWall;
    protected bool isDetectingLedge;
    protected bool isPlayerInMinAggroRange;
    public MoveState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Data_MoveState stateData) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void DoChecks()
    {
        base.DoChecks();
        isDetectingLedge = CollisionManager.LedgeVertical;
        isDetectingWall =  CollisionManager.WallFront;
        isPlayerInMinAggroRange = entity.CheckPlayerInMinAggroRange();
    }

    public override void Enter()
    {
        base.Enter();
        Movement.SetVelocityX(stateData.MovementSpeed * Movement.FacingDirection);        
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        Movement.SetVelocityX(stateData.MovementSpeed * Movement.FacingDirection);
    }

    public override void PhsyicsUpdate()
    {
        base.PhsyicsUpdate();
    }
}
