using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class Combat : CoreComponent, IDamageable, IKnockbackable, IPoiseDamageable
{
    [SerializeField] private GameObject damageParticles;
    [SerializeField] private float maxKnockbackTime = 0.2f;
    private bool isKnockbackActive;
    private float knockbackStartTime;

    #region CORE COMPONENTS
    private Movement Movement { get => movement ??= core.GetCoreComponent<Movement>(); }
    private CollisionManager CollisionManager { get => collisionManager ??= core.GetCoreComponent<CollisionManager>(); }   
    private Stats Stats { get => stats ??= core.GetCoreComponent<Stats>(); }
    private ParticleManager ParticleManager => particleManager ??= core.GetCoreComponent<ParticleManager>();


    private Movement movement;
    private CollisionManager collisionManager;
    private Stats stats;    
    private ParticleManager particleManager;
    #endregion


    public override void LogicUpdate()
    {
        CheckKnockback();
    }

    public void Damage(float amount)
    {
        Stats.DecreaseHealth(amount);
        ParticleManager.CallDamageFlash();
        //ParticleManager.StartParticlesWithRandomRotation(damageParticles);
    }

    public void DamagePoise(float amount)
    {
        Stats.DecreasePoise(amount);
    }

    public void Knockback(Vector2 angle, float strength, int direction)
    {
        Movement.SetVelocity(strength,angle,direction);
        Movement.CanSetVelocity = false;
        isKnockbackActive = true;
        knockbackStartTime = Time.time;
    }

    private void CheckKnockback()
    {
        if (isKnockbackActive && ((Movement.CurrentVelocity.y <= 0.01f && CollisionManager.Ground) || Time.time >= knockbackStartTime + maxKnockbackTime))
        {
            isKnockbackActive = false;
            Movement.CanSetVelocity = true;
        }
    }

    
}
