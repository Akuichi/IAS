using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour, InputPlayer.IPlayerActions
{
    private InputPlayer inputPlayer;
    public static PlayerInput PlayerInput;
    public Vector2 RawMovementInput { get; private set; }
    public int NormInputX { get; private set; }
    public int NormInputY { get; private set; }
    public bool JumpInput { get; private set; }
    public bool JumpInputStop { get; private set; }
    public bool AttackInput { get; private set; }
    public bool AttackInputStop { get; private set; }
    public bool DashInput { get; private set; }
    public bool DashInputStop { get; private set; }

    [SerializeField]
    private float inputHoldTime = 0.2f;

    private float jumpInputStartTime;
    private float dashInputStartTime;
    private float attackInputStartTime;

    private void Awake()
    {
        inputPlayer = new InputPlayer();
        inputPlayer.Player.SetCallbacks(this); // Set this class as the callback receiver
        PlayerInput = GetComponent<PlayerInput>();
    }

    private void OnEnable()
    {
        inputPlayer.Enable();
    }

    private void OnDisable()
    {
        inputPlayer.Disable();
    }

    private void Update()
    {
        CheckJumpInputHoldTime();
        CheckDashInputHoldTime();
        CheckAttackInputHoldTime();
    }

    #region Input Actions
    public void OnMove(InputAction.CallbackContext context)
    {
        RawMovementInput = context.ReadValue<Vector2>();
        NormInputX = (int)(RawMovementInput * Vector2.right).normalized.x;
        NormInputY = (int)(RawMovementInput * Vector2.up).normalized.y;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            JumpInput = true;
            JumpInputStop = false;
            jumpInputStartTime = Time.time;
        }
        else if (context.canceled)
        {
            JumpInputStop = true;
        }
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            DashInput = true;
            DashInputStop = false;
            dashInputStartTime = Time.time;
        }
        else if (context.canceled)
        {
            DashInputStop = true;
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            AttackInput = true;
            AttackInputStop = false;
            attackInputStartTime = Time.time;
        }
        else if (context.canceled)
        {
            AttackInputStop = true;
        }
    }
    #endregion

    #region Utility Functions
    public void UseJumpInput() => JumpInput = false;
    public void UseDashInput() => DashInput = false;
    public void UseAttackInput() => AttackInput = false;

    private void CheckJumpInputHoldTime()
    {
        if (Time.time >= jumpInputStartTime + inputHoldTime)
        {
            JumpInput = false;
        }
    }
    private void CheckDashInputHoldTime()
    {
        if (Time.time >= dashInputStartTime + inputHoldTime)
        {
            DashInput = false;
        }
    }
    private void CheckAttackInputHoldTime()
    {
        if (Time.time >= attackInputStartTime + inputHoldTime)
        {
            AttackInput = false;
        }
    }
    #endregion
}
