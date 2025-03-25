using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Entity : MonoBehaviour
{
    private Movement Movement { get => movement ??= Core.GetCoreComponent<Movement>(); }
    private Movement movement;
    private Stats Stats { get => stats ??= Core.GetCoreComponent<Stats>(); }
    private Stats stats;
    public FiniteStateMachine StateMachine;
    public Data_Entity EntityData;
    public Animator Animator { get; private set; }
    public AnimationToStateMachine atsm {  get; private set; }

    [SerializeField] private Transform wallCheck;
    [SerializeField] private Transform ledgeCheck;
    [SerializeField] private Transform playerCheck;
    [SerializeField] private Transform groundCheck;
    private Vector2 velocityWorkspace;

    //private float currentStunResistance;
    private float lastDamageTime;
    public int LastDamageDirection { get; private set; }
    public Core Core { get; private set; }

    //protected bool isStunned;
    //protected bool isDead;
    public virtual void Awake()
    {
        Core = GetComponentInChildren<Core>();
        Animator = GetComponent<Animator>();

        atsm = GetComponent <AnimationToStateMachine>();

        StateMachine = new FiniteStateMachine();
    }

    private void OnEnable()
    {
        Stats.OnPoiseZero += HandlePoiseZero;
    }

    private void OnDisable()
    {
        stats.OnPoiseZero -= HandlePoiseZero;
    }


    public virtual void Update()
    {
        Core.LogicUpdate();
        StateMachine.CurrentState.LogicUpdate();
    }

    public virtual void FixedUpdate()
    {
        StateMachine.CurrentState.PhsyicsUpdate();
    }

    
    #region CHECKS
    public virtual bool CheckPlayerInMinAggroRange()
    {
        return Physics2D.Raycast(playerCheck.position, transform.right, EntityData.MinAggroDistance, EntityData.WhatIsPlayer);
    }

    public virtual bool CheckPlayerInMaxAggroRange()
    {
        return Physics2D.Raycast(playerCheck.position, transform.right, EntityData.MaxAggroDistance, EntityData.WhatIsPlayer);
    }
    #endregion

    protected virtual void HandlePoiseZero()
    {
        
    }      

    public void ResetPoise()
    {
        stats.IncreasePoise(999);
    }
    public virtual bool CheckPlayerInCloseRangeAction()
    {
        return Physics2D.Raycast(playerCheck.position, transform.right, EntityData.CloseRangeActionDistance, EntityData.WhatIsPlayer);
    }

    public virtual void OnDrawGizmos()
    {
        if (Core!= null)
        {
            Gizmos.DrawLine(wallCheck.position, wallCheck.position + (Vector3)(Vector2.right * Movement.FacingDirection * EntityData.WallCheckDistance));
            Gizmos.DrawLine(ledgeCheck.position, ledgeCheck.position + (Vector3)(Vector2.down * EntityData.LedgeCheckDistance));
            Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(Vector2.right * EntityData.CloseRangeActionDistance), 0.2f);
            Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(Vector2.right * EntityData.MinAggroDistance), 0.2f);
            Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(Vector2.right * EntityData.MaxAggroDistance), 0.2f);
        }
        
    }
}
