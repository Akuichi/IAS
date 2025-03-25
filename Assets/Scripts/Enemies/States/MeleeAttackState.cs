using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackState : AttackState
{
    protected Data_MeleeAttackState stateData;

    public MeleeAttackState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Transform attackPosition, Data_MeleeAttackState stateData) : base(entity, stateMachine, animBoolName, attackPosition)
    {
        this.stateData = stateData;
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FinishAttack()
    {
        base.FinishAttack();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void PhsyicsUpdate()
    {
        base.PhsyicsUpdate();
    }

    public override void TriggerAttack()
    {
        base.TriggerAttack();

        Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(attackPosition.position, stateData.AttackRadius, stateData.WhatIsPlayer);
        foreach (Collider2D collider in detectedObjects)
        {
            if (collider.TryGetComponent<IDamageable>(out var damageable))
            {
                damageable.Damage(stateData.AttackDamage);
            }

            if (collider.TryGetComponent<IKnockbackable>(out var knockbackable))
            {
                knockbackable.Knockback(stateData.KnockbackAngle, stateData.KnockbackStrength, Movement.FacingDirection);
            }

            if (collider.TryGetComponent<IPoiseDamageable>(out var poiseDamageable))
            {
                poiseDamageable.DamagePoise(stateData.PoiseDamage);
            }
        }


    }
}
