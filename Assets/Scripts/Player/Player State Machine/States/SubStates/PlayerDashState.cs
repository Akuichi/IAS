using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerAbilityState
{
    public bool CanDash { get; private set; }
    private float lastDashTime;
    public bool IsDashing;
    private bool isDashAttacking;
    private Vector2 dashDirection;
    public PlayerDashState(Player player, PlayerStateMachine stateMachine, PlayerMovementData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
        player.DashState.CheckDashDirection(player.InputHandler.RawMovementInput);
        Sleep(playerData.DashSleepTime);
        IsDashing = true;
        CanDash = false;
        player.InputHandler.UseDashInput();      
        
        CoroutineRunner.Instance.RunCoroutine(StartDash(dashDirection));
    }

    public void CheckDashDirection(Vector2 direction)
    {
        if (direction != Vector2.zero)
        {
            //dashDirection = inputDirection == 1 ? Vector2.right : Vector2.left;            
            dashDirection = direction;
        }
        else
        {
            dashDirection = Movement.FacingDirection == 1 ? Vector2.right : Vector2.left;
        }
    }
    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (isDashAttacking)
        {
            Movement?.SetGravityScale(0);
            Move(playerData.DashEndRunLerp, xInput);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    private IEnumerator StartDash(Vector2 dir)
    {
        //Overall this method of dashing aims to mimic Celeste, if you're looking for
        // a more physics-based approach try a method similar to that used in the jump
        float startTime = Time.time;
        isDashAttacking = true;

        Movement?.SetGravityScale(0);

        //We keep the player's velocity at the dash speed during the "attack" phase (in celeste the first 0.15s)
        while (Time.time - startTime <= playerData.DashAttackTime)
        {
            Movement.SetVelocity(dir.normalized * playerData.DashSpeed);
            //Pauses the loop until the next frame, creating something of a Update loop. 
            //This is a cleaner implementation opposed to multiple timers and this coroutine approach is actually what is used in Celeste :D
            yield return null;
        }

        startTime = Time.time;

        isDashAttacking = false;

        //Begins the "end" of our dash where we return some control to the player but still limit run acceleration (see Update() and Run())
        Movement?.SetGravityScale(playerData.GravityScale);
        Movement?.SetVelocity(playerData.DashEndSpeed * dir.normalized);

        while (Time.time - startTime <= playerData.DashEndTime)
        {
            yield return null;
        }

        //Dash over
        player.Anim.SetBool("airDash", false);
        IsDashing = false;
        isAbilityDone = true;
        lastDashTime = Time.time;
        Movement.SetVelocityX(0);
    }

    public void ResetCanDash() => CanDash = true;
    public bool CheckIfCanDash()
    {
        return CanDash && Time.time >= lastDashTime + playerData.DashCooldown;
    }

    private void Move(float lerpAmount, int inputX)
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

    private void Sleep(float duration)
    {
        //Method used so we don't need to call StartCoroutine everywhere
        //nameof() notation means we don't need to input a string directly.
        //Removes chance of spelling mistakes and will improve error messages if any
        CoroutineRunner.Instance.RunCoroutine(PerformSleep(duration));
    }

    private IEnumerator PerformSleep(float duration)
    {
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(duration); //Must be Realtime since timeScale with be 0 
        Time.timeScale = 1;
    }



}
