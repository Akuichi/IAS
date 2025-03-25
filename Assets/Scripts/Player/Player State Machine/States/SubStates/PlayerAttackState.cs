using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerAttackState : PlayerAbilityState
{
    private Weapon weapon;
    private float velocityToSet;
    private bool setVelocity;
    private bool shouldCheckFlip;
    private bool shouldFireNextMove;
    private bool isListeningToMoveInput;
    public PlayerAttackState(Player player, PlayerStateMachine stateMachine, PlayerMovementData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
        player.InputHandler.UseAttackInput();
        setVelocity = false;
        shouldFireNextMove = false;
        nextState = null;
        weapon.EnterWeapon();
    }

    public override void Exit()
    {
        base.Exit();
        isListeningToMoveInput = false;
        shouldFireNextMove = false;
        nextState = null;
        weapon.ExitWeapon();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (player.InputHandler.DashInput)
        {
            stateMachine.ChangeState(player.DashState);
        }
        else if (player.InputHandler.AttackInput)
        {
            nextState = player.AttackState;
            player.InputHandler.UseAttackInput();
        }

        if (shouldFireNextMove)
        {
            if (nextState != null)
            {   
                if (nextState == player.DashState && player.DashState.CheckIfCanDash())
                {
                    stateMachine.ChangeState(player.DashState);
                }
                else
                {
                    stateMachine.ChangeState(nextState);
                }
                     
                return;
            }
        }

        if (isListeningToMoveInput)
        {
            if (player.InputHandler.NormInputX != 0)
            {
                stateMachine.ChangeState(player.MoveState);
            }
        }


        if (shouldCheckFlip)
            Movement?.CheckIfShouldFlip(xInput);
        if (setVelocity)
            Movement?.SetVelocityX(velocityToSet * Movement.FacingDirection);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public void SetWeapon(Weapon weapon)
    {
        this.weapon = weapon;
        weapon.InitializeWeapon(this, core);
    }

    public void SetPlayerVelocity(float velocity)
    {
        Movement?.SetVelocityX(velocity * Movement.FacingDirection);
        velocityToSet = velocity;
        setVelocity = true;
    }

    public void SetFlipCheck(bool value)
    {
        shouldCheckFlip = value;
    }

    public void NextMoveFire()
    {
        shouldFireNextMove = true;
    }
    public void MovementInputListen()
    {
        isListeningToMoveInput = true;
    }

    #region Animation Triggers
    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
        isAbilityDone = true;
    }
    #endregion
}
