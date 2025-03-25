using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInAirState : PlayerState
{
    protected Movement Movement { get => movement ?? core.GetCoreComponent(ref movement); }
    private CollisionManager CollisionManager { get => collisionManager ?? core.GetCoreComponent(ref collisionManager); }

    private Movement movement;
    private CollisionManager collisionManager;

    //Input
    private int xInput;
    protected int yInput;
    private bool jumpInput;
    private bool jumpInputStop;
    private bool grabInput;
    private bool dashInput;

    //Checks
    private bool isGrounded;
    private bool isTouchingWall;
    private bool isTouchingWallBack;
    private bool oldIsTouchingWall;
    private bool oldIsTouchingWallBack;
    private bool isTouchingLedge;

    private bool coyoteTime;
    private bool wallJumpCoyoteTime;
    private bool isJumping;


    private float startWallJumpCoyoteTime;


    public PlayerInAirState(Player player, PlayerStateMachine stateMachine, PlayerMovementData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
        oldIsTouchingWall = isTouchingWall;
        oldIsTouchingWallBack = isTouchingWallBack;

        if (CollisionManager)
        {
            isGrounded = CollisionManager.Ground;
            isTouchingWall = CollisionManager.WallFront;
            isTouchingWallBack = CollisionManager.WallBack;
            isTouchingLedge = CollisionManager.LedgeHorizontal;
        }

        if (isTouchingWall && !isTouchingLedge)
        {
            player.LedgeClimbState.SetDetectedPosition(player.transform.position);
        }

        if (!wallJumpCoyoteTime && !isTouchingWall && !isTouchingWallBack && (oldIsTouchingWall || oldIsTouchingWallBack))
        {
            StartWallJumpCoyoteTime();
        }
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
        oldIsTouchingWall = false;
        oldIsTouchingWallBack = false;
        isTouchingWall = false;
        isTouchingWallBack = false;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        CheckWallJumpCoyoteTime();
        CheckCoyoteTime();

        xInput = player.InputHandler.NormInputX;
        yInput = player.InputHandler.NormInputY;
        jumpInput = player.InputHandler.JumpInput;
        jumpInputStop = player.InputHandler.JumpInputStop;
        dashInput = player.InputHandler.DashInput;


        //Reset if jumping
        if (isJumping)
        {
            if (Movement.CurrentVelocity.y <= 0f)
                isJumping = false;            
        }


        AdjustGravity();
       

        //Change States
        if (!isJumping && coyoteTime && jumpInput) //Normal jump coyote time
        {
            coyoteTime = false;
            stateMachine.ChangeState(player.JumpState);
        }
        else if (isGrounded && Movement?.CurrentVelocity.y < 0.01f) //Land
        {
            stateMachine.ChangeState(player.LandState);
        }
        else if (isTouchingWall && !isTouchingLedge && !isGrounded) //Ledge Climb
        {
            stateMachine.ChangeState(player.LedgeClimbState);
        }

        else if (jumpInput && (isTouchingWall || isTouchingWallBack || wallJumpCoyoteTime)) //Wall Jump
        {
            StopWallJumpCoyoteTime();
            isTouchingWall = CollisionManager.WallFront;
            player.WallJumpState.DetermineWallJumpDirection(isTouchingWall);
            stateMachine.ChangeState(player.WallJumpState);
        }
        else if (jumpInput && player.JumpState.CanAirJump()) //Air Jump
        {
            Movement?.SetVelocityY(0);
            stateMachine.ChangeState(player.JumpState);
            player.Anim.SetBool("doubleJump", true);
            player.JumpState.DecreaseBonusJumpsLeft();
        }
        else if (isTouchingWall && xInput == Movement?.FacingDirection && Movement?.CurrentVelocity.y <= 0) //Wall Slide
        {
            stateMachine.ChangeState(player.WallSlideState);
        }
        else if (dashInput && player.DashState.CheckIfCanDash()) //Dash
        {
            player.Anim.SetBool("airDash", true);
            player.DashState.CheckDashDirection(player.InputHandler.RawMovementInput);
            stateMachine.ChangeState(player.DashState);
        }
        else
        {
            Movement?.CheckIfShouldFlip(xInput);
            player.Anim.SetFloat("yVelocity", Movement.CurrentVelocity.y);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        MoveAir(1, xInput);
    }

    private void AdjustGravity()
    {
        //Adjust Gravity
        if (!isJumping && yInput < 0) //Holding DOWN button and Falling
        {
            Movement?.SetGravityScale(playerData.GravityScale * playerData.FastFallGravityMult);
            ClampFallSpeed(true);
        }
        else if (jumpInputStop)
        {
            Movement?.SetGravityScale(playerData.GravityScale * playerData.JumpCutGravityMult);
            ClampFallSpeed(false);
        }
        else if (Mathf.Abs(Movement.CurrentVelocity.y) < playerData.JumpHangTimeThreshold) //isJumping/Falling
        {
            Movement?.SetGravityScale(playerData.GravityScale * playerData.JumpHangGravityMult);
        }
        else if (!isJumping) //Higher gravity when falling
        {
            Movement?.SetGravityScale(playerData.GravityScale * playerData.FallGravityMult);
            ClampFallSpeed(false);
        }
    }
    private void CheckCoyoteTime()
    {
        if (coyoteTime && Time.time > startTime + playerData.CoyoteTime)
        {
            coyoteTime = false;
        }
    }

    private void CheckWallJumpCoyoteTime()
    {
        if (wallJumpCoyoteTime && Time.time > startWallJumpCoyoteTime + playerData.CoyoteTime)
        {
            wallJumpCoyoteTime = false;
        }
    }

    public void StartCoyoteTime() => coyoteTime = true;

    public void StartWallJumpCoyoteTime()
    {
        wallJumpCoyoteTime = true;
        startWallJumpCoyoteTime = Time.time;
    }

    public void StopWallJumpCoyoteTime() => wallJumpCoyoteTime = false;

    public void SetIsJumping() => isJumping = true;

    public void MoveAir(float lerpAmount, int inputX)
    {
        Vector2 velocity = Movement.CurrentVelocity;
        float targetSpeed = inputX * playerData.MoveSpeed;
        targetSpeed = Mathf.Lerp(velocity.x, targetSpeed, lerpAmount);

        //Get Accelrate
        float accelRate;

        accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? playerData.MoveAcceleration * 1f : playerData.MoveDeceleration * 1f;

        //When at apex of the jump
        if (Mathf.Abs(velocity.y) < playerData.JumpHangTimeThreshold)
        {
            accelRate *= playerData.JumpHangAccelerationMult;
            targetSpeed *= playerData.JumpHangMaxSpeedMult;
        }
        float speedDif = targetSpeed - velocity.x;
        float movement = speedDif * accelRate;
        Movement.AddForce(movement * Vector2.right, ForceMode2D.Force);
    }

    public void ClampFallSpeed(bool isFastFalling)
    {
        Vector2 velocity = Movement.CurrentVelocity;
        if (isFastFalling)
            Movement.SetVelocityY(Mathf.Max(velocity.y, -playerData.MaxFastFallSpeed));
        else
            Movement.SetVelocityY(Mathf.Max(velocity.y, -playerData.MaxFallSpeed));
    }


}
