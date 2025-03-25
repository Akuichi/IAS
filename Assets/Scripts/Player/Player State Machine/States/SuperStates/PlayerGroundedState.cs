using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerGroundedState : PlayerState
{

    protected int xInput;
    protected int yInput;

    protected bool isTouchingCeiling;

    protected Movement Movement
    {
        get => movement ?? core.GetCoreComponent(ref movement);
    }

    private Movement movement;

    private CollisionManager CollisionManager
    {
        get => collisionManager ?? core.GetCoreComponent(ref collisionManager);
    }

    private CollisionManager collisionManager;


    private bool jumpInput;
    private bool grabInput;
    private bool isGrounded;
    private bool isTouchingWall;
    private bool isTouchingLedge;
    private bool dashInput;



    public PlayerGroundedState(Player player, PlayerStateMachine stateMachine, PlayerMovementData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {   
        
    }

    public override void DoChecks()
    {
        base.DoChecks();
        if (CollisionManager)
        {
            isGrounded = CollisionManager.Ground;
            isTouchingWall = CollisionManager.WallFront;
            isTouchingLedge = CollisionManager.LedgeHorizontal;
            isTouchingCeiling = CollisionManager.Ceiling;
        }
    }

    public override void Enter()
    {
        base.Enter();
        Movement?.SetGravityScale(playerData.GravityScale);
        player.JumpState.ResetBonusJumpsLeft();
        player.DashState.ResetCanDash();
        player.Anim.SetBool("doubleJump", false);
        //Reset Dash
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        xInput = player.InputHandler.NormInputX;
        yInput = player.InputHandler.NormInputY;
        jumpInput = player.InputHandler.JumpInput;
        //grabInput = player.InputHandler.GrabInput;
        dashInput = player.InputHandler.DashInput;
        if (player.InputHandler.AttackInput)
        {
            stateMachine.ChangeState(player.AttackState);
        }
        else if (jumpInput)
        {
            stateMachine.ChangeState(player.JumpState);
        }
        else if (!isGrounded)
        {
            player.InAirState.StartCoyoteTime();
            stateMachine.ChangeState(player.InAirState);
        }
        else if (dashInput && player.DashState.CheckIfCanDash())
        {
            player.DashState.CheckDashDirection(player.InputHandler.RawMovementInput);
            stateMachine.ChangeState(player.DashState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        MoveGround(1, xInput);
    }

    public void MoveGround(float lerpAmount, int inputX)
    {
        Vector2 velocity = Movement.CurrentVelocity;
        //Run
        float targetSpeed = inputX * playerData.MoveSpeed;
        targetSpeed = Mathf.Lerp(velocity.x, targetSpeed, lerpAmount);

        //Get Accelrate
        float accelRate;
        accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? playerData.MoveAcceleration : playerData.MoveDeceleration;


        #region Conserve Momentum
        //We won't slow the player down if they are moving in their desired direction but at a greater speed than their maxSpeed
        if (Mathf.Abs(velocity.x) > Mathf.Abs(targetSpeed) && Mathf.Sign(velocity.x) == Mathf.Sign(targetSpeed) && Mathf.Abs(targetSpeed) > 0.01f)
        {
            //Prevent any deceleration from happening, or in other words conserve are current momentum
            //You could experiment with allowing for the player to slightly increae their speed whilst in this "state"
            accelRate = 0;
        }
        #endregion
        float speedDif = targetSpeed - velocity.x;
        float movement = speedDif * accelRate;

        Movement.AddForce(movement * Vector2.right);
    }
}
