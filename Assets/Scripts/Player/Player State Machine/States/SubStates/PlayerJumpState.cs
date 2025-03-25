using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerAbilityState
{
    private int bonusJumpsLeft;
    public PlayerJumpState(Player player, PlayerStateMachine stateMachine, PlayerMovementData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
        bonusJumpsLeft = playerData.BonusJumpsCount;
    }

    public override void Enter()
    {
        base.Enter();
        Movement?.SetGravityScale(playerData.GravityScale);
        player.InputHandler.UseJumpInput();
        Jump();
        isAbilityDone = true;
        player.InAirState.SetIsJumping();
    }
    public bool CanAirJump()
    {
        if (bonusJumpsLeft > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ResetBonusJumpsLeft() => bonusJumpsLeft = playerData.BonusJumpsCount;

    public void DecreaseBonusJumpsLeft() => bonusJumpsLeft--;

    private void Jump()
    {
        Vector2 velocity = Movement.CurrentVelocity;
        float force = playerData.JumpForce;
        if (velocity.y < 0)
            force -= velocity.y;

        Movement.AddForce(Vector2.up * force, ForceMode2D.Impulse);
    }
}
