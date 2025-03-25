using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallJumpState : PlayerAbilityState
{
    private int wallJumpDirection;
    protected int yInput;
    public PlayerWallJumpState(Player player, PlayerStateMachine stateMachine, PlayerMovementData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {

    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
        Movement.SetGravityScale(playerData.GravityScale);
        player.InputHandler.UseJumpInput();
        WallJump(wallJumpDirection);
        Movement?.CheckIfShouldFlip(wallJumpDirection);
        player.JumpState.DecreaseBonusJumpsLeft();

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

        if (Time.time >= startTime + playerData.WallJumpTime)
        {
            isAbilityDone = true;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        player.InAirState.MoveAir(playerData.WallJumpRunLerp, xInput);

    }

    public void DetermineWallJumpDirection(bool isTouchingWall)
    {
        if (isTouchingWall)
        {
            wallJumpDirection = -Movement.FacingDirection;
        }
        else
        {
            wallJumpDirection = Movement.FacingDirection;
        }
    }

    public void WallJump(int direction)
    {
        Vector2 force = new Vector2(playerData.WallJumpForce.x, playerData.WallJumpForce.y);
        Vector2 Velocity = new Vector2(0,Movement.CurrentVelocity.y);
        force.x *= direction; //apply force in opposite direction of wall

        if (Mathf.Sign(Movement.CurrentVelocity.x) != Mathf.Sign(force.x))
            force.x -= Movement.CurrentVelocity.x;

        if (Movement.CurrentVelocity.y < 0) //checks whether player is falling, if so we subtract the velocity.y (counteracting force of gravity). This ensures the player always reaches our desired jump force or greater
            force.y -= Movement.CurrentVelocity.y;

        //Unlike in the run we want to use the Impulse mode.
        //The default mode will apply are force instantly ignoring masss
        Movement.AddForce(force, ForceMode2D.Impulse);
    }
}
