using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player Movement Data")]
public class PlayerMovementData : ScriptableObject
{
    [Header("Gravity")]
    [HideInInspector] public float GravityStrength; //Downwards force (gravity) needed for the desired JumpHeight and JumpTimeToApex.
    [HideInInspector] public float GravityScale;

    [Space(5)]
    public float FallGravityMult; //Multiplier to the player's GravityScale when falling.
    public float MaxFallSpeed; //Maximum fall speed (terminal velocity) of the player when falling.
    [Space(5)]
    public float FastFallGravityMult; //Larger multiplier to the player's GravityScale when they are falling and a downwards input is pressed.
                                      //Seen in games such as Celeste, lets the player fall extra fast if they wish.
    public float MaxFastFallSpeed; //Maximum fall speed(terminal velocity) of the player when performing a faster fall.


    [Header("Movement")]
    [Range(0f, 100f)] public float MoveSpeed = 10f;
    [Range(0f, 100f)] public float MoveAcceleration = 10f;
    [Range(0f, 100f)] public float MoveDeceleration = 20f;
    [Range(0f, 5f)] public float AirAccelerationMultiplier = 1f;
    [Range(0f, 5f)] public float AirDecelerationMultiplier = 1f;

    [Header("Jump")]
    public float JumpHeight; //Height of the player's jump
    public float JumpTimeToApex; //Time between applying the jump force and reaching the desired jump height. These values also control the player's gravity and jump force.
    public int BonusJumpsCount = 1;
    [HideInInspector] public float JumpForce; //The actual force applied (upwards) to the player when they jump.

    [Header("Wall Jump")]
    public Vector2 WallJumpForce;
    [Space(5)]
    [Range(0f, 1f)] public float WallJumpRunLerp; //Reduces the effect of player's movement while wall jumping.
    [Range(0f, 1.5f)] public float WallJumpTime; //Time after wall jumping the player's movement is slowed for.


    [Header("Both Jumps")]
    [Range(0f, 1)] public float JumpHangGravityMult; //Reduces gravity while close to the apex (desired max height) of the jump
    public float JumpHangTimeThreshold; //Speeds (close to 0) where the player will experience extra "jump hang". The player's velocity.y is closest to 0 at the jump's apex (think of the gradient of a parabola or quadratic function)
    [Space(0.5f)]
    public float JumpHangAccelerationMult;
    public float JumpHangMaxSpeedMult;
    public float JumpCutGravityMult = 2f;


    [Header("Wall Slide")]
    public float WallSlideSpeed;
    public float WallSlideAccel;

    [Header("Ledge Climb")]
    public Vector2 StartOffset;
    public Vector2 StopOffset;


    [Header("Dash")]
    public int DashAmount;
    public float DashSpeed;
    public float DashSleepTime; //Duration for which the game freezes when we press dash but before we read directional input and apply a force
    [Space(5)]
    public float DashAttackTime;
    [Space(5)]
    public float DashEndTime; //Time after you finish the inital drag phase, smoothing the transition back to idle (or any standard state)
    public Vector2 DashEndSpeed; //Slows down player, makes dash feel more responsive (used in Celeste)
    [Range(0f, 1f)] public float DashEndRunLerp; //Slows the affect of player movement while dashing
    [Space(5)]
    public float DashCooldown;


    [Header("Assists")]
    [Range(0.01f, 0.5f)] public float CoyoteTime; 
    [Range(0.01f, 0.5f)] public float JumpInputBufferTime;



    private void OnValidate()
    {
        //Calculate gravity strength using the formula (gravity = 2 * JumpHeight / timeToJumpApex^2) 
        GravityStrength = -(2 * JumpHeight) / (JumpTimeToApex * JumpTimeToApex);

        //Calculate the rigidbody's gravity scale (ie: gravity strength relative to unity's gravity value, see project settings/Physics2D)
        GravityScale = GravityStrength / Physics2D.gravity.y;

        //Calculate JumpForce using the formula (initialJumpVelocity = gravity * timeToJumpApex)
        JumpForce = Mathf.Abs(GravityStrength) * JumpTimeToApex;
    }

}

