using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLedgeClimbState : PlayerState
{
    protected Movement Movement { get => movement ??= core.GetCoreComponent<Movement>(); }
    private CollisionManager CollisionManager { get => collisionManager ??= core.GetCoreComponent<CollisionManager>(); }

    private Movement movement;
    private CollisionManager collisionManager;

    private Vector2 detectedPos;
    private Vector2 cornerPos;
    private Vector2 startPos;
    private Vector2 stopPos;
    private Vector2 workspace;

    private bool isHanging;
    private bool isClimbing;
    private bool jumpInput;
    private bool isTouchingCeiling;

    private int xInput;
    private int yInput;

    public PlayerLedgeClimbState(Player player, PlayerStateMachine stateMachine, PlayerMovementData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
        player.Anim.SetBool("climbLedge", false);
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
        isHanging = true;
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
        Movement.SetVelocityZero();

        player.transform.position = detectedPos;
        cornerPos = DetermineCornerPosition();

        startPos.Set(cornerPos.x - (Movement.FacingDirection * playerData.StartOffset.x), cornerPos.y - playerData.StartOffset.y);
        stopPos.Set(cornerPos.x + (Movement.FacingDirection * playerData.StopOffset.x), cornerPos.y + playerData.StopOffset.y);

        player.transform.position = startPos;

    }

    public override void Exit()
    {
        base.Exit();
        isHanging = false;

        if (isClimbing)
        {
            player.transform.position = stopPos;
            isClimbing = false;
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (isAnimationFinished)
        {
            stateMachine.ChangeState(player.IdleState);
            
        }
        else
        {
            xInput = player.InputHandler.NormInputX;
            yInput = player.InputHandler.NormInputY;
            jumpInput = player.InputHandler.JumpInput;

            Movement?.SetVelocityZero();
            player.transform.position = startPos;

            if (xInput == Movement.FacingDirection  && isHanging && !isClimbing)
            {
                isClimbing = true;
                player.Anim.SetBool("climbLedge", true);
            }
            else if ((xInput == -Movement.FacingDirection || yInput == -1) && isHanging && !isClimbing)
            {
                stateMachine.ChangeState(player.InAirState);
            }
            else if (jumpInput && !isClimbing)
            {
                player.WallJumpState.DetermineWallJumpDirection(true);
                stateMachine.ChangeState(player.WallJumpState);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }


    public void SetDetectedPosition(Vector2 pos) => detectedPos = pos;
    private Vector2 DetermineCornerPosition()
    {
        RaycastHit2D xHit = Physics2D.Raycast(CollisionManager.WallCheck.position, Vector2.right * Movement.FacingDirection, CollisionManager.WallCheckDistance, CollisionManager.WhatIsGround);
        float xDist = xHit.distance;
        workspace.Set((xDist + 0.015f) * Movement.FacingDirection, 0f);
        RaycastHit2D yHit = Physics2D.Raycast(CollisionManager.LedgeCheckHorizontal.position + (Vector3)(workspace), Vector2.down, CollisionManager.LedgeCheckHorizontal.position.y - CollisionManager.WallCheck.position.y + 0.015f, CollisionManager.WhatIsGround);
        float yDist = yHit.distance;

        workspace.Set(CollisionManager.WallCheck.position.x + (xDist * Movement.FacingDirection), CollisionManager.LedgeCheckHorizontal.position.y - yDist);
        return workspace;
    }
}
