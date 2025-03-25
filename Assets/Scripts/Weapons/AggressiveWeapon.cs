using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AggressiveWeapon : Weapon
{
    protected SO_AggressiveWeaponData aggressiveWeaponData;
    private Dictionary<object, (bool damaged, bool knockedBack, bool poiseDamaged)> affectedEntities = new();
    private List<IKnockbackable> detectedKnockbackables = new List<IKnockbackable>();
    private List<IPoiseDamageable> detectedPoiseDamageables = new List<IPoiseDamageable>();
    private List<IDamageable> detectedDamageables = new List<IDamageable>();


    protected override void Awake()
    {
        base.Awake();
        if (weaponData is SO_AggressiveWeaponData)
        {
            aggressiveWeaponData = (SO_AggressiveWeaponData)weaponData;
        }
        else
        {

        }
    }
    public override void AnimationActionTrigger()
    {        
        base.AnimationActionTrigger();
        CheckMeleeAttack();
    }

    private void Update()
    {
        if (isHitboxActive)
        {
            CheckMeleeAttack();
        }
    }

    public override void EnterWeapon()
    {
        base.EnterWeapon();
        affectedEntities.Clear();
    }
    private void CheckMeleeAttack()
    {
        WeaponAttackDetails details = aggressiveWeaponData.AttackDetails[attackCounter];
        foreach (IDamageable item in  detectedDamageables.ToList())
        {
            if (!affectedEntities.TryGetValue(item, out var status) || !status.damaged)
            {
                item.Damage(details.DamageAmount);
                affectedEntities[item] = (true, status.knockedBack, status.poiseDamaged);
            }
        }

        foreach (IKnockbackable item in detectedKnockbackables.ToList())
        {
            if (!affectedEntities.TryGetValue(item, out var status) || !status.knockedBack)
            {
                item.Knockback(details.KnockbackAngle, details.KnockbackStrength, Movement.FacingDirection);
                affectedEntities[item] = (status.damaged, true, status.poiseDamaged);
            }
        }

        foreach (IPoiseDamageable item in detectedPoiseDamageables.ToList())
        {
            if (!affectedEntities.TryGetValue(item, out var status) || !status.poiseDamaged)
            {
                item.DamagePoise(details.PoiseDamageAmount);
                affectedEntities[item] = (status.damaged, status.knockedBack, true);
            }
        }
    }

    public void AddToDetected(Collider2D collision)
    {
        if (collision.TryGetComponent<IDamageable>(out var damageable))
        {
            detectedDamageables.Add(damageable);
        }

        if (collision.TryGetComponent<IKnockbackable>(out var knockbackable))
        {
            detectedKnockbackables.Add(knockbackable);
        }

        if (collision.TryGetComponent<IPoiseDamageable>(out var poiseDamageable))
        {
            detectedPoiseDamageables.Add(poiseDamageable);
        }

    }

    public void RemoveFromDetected(Collider2D collision)
    {
        if (collision.TryGetComponent<IDamageable>(out var damageable))
        {
            detectedDamageables.Remove(damageable);
        }

        if (collision.TryGetComponent<IKnockbackable>(out var knockbackable))
        {
            detectedKnockbackables.Remove(knockbackable);
        }

        if (collision.TryGetComponent<IPoiseDamageable>(out var poiseDamageable))
        {
            detectedPoiseDamageables.Remove(poiseDamageable);
        }
    }
}
