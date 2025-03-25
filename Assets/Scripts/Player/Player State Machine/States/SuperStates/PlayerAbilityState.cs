using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilityState : PlayerState
{
    protected bool isAbilityDone;

    protected PlayerState nextState;

    protected int xInput;
    protected Movement Movement { get => movement ?? core.GetCoreComponent(ref movement); }
    private CollisionManager CollisionManager { get => collisionManager ?? core.GetCoreComponent(ref collisionManager); }

    private Movement movement;
    private CollisionManager collisionManager;

    private bool isGrounded;


    public PlayerAbilityState(Player player, PlayerStateMachine stateMachine, PlayerMovementData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {

    }

    public override void DoChecks()
    {
        base.DoChecks();
        if (CollisionManager)
        {
            isGrounded = CollisionManager.Ground;
        }
    }

    public override void Enter()
    {
        base.Enter();
        isAbilityDone = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        xInput = player.InputHandler.NormInputX;
        if (isAbilityDone)
        {
            if (isGrounded && Movement?.CurrentVelocity.y < 0.01f)
            {
                stateMachine.ChangeState(player.IdleState);
            }
            else
            {
                stateMachine.ChangeState(player.InAirState);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
