using System.Collections;
using System.Collections.Generic;
using UnityEditor.AnimatedValues;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    protected Movement Movement { get => movement ?? core.GetCoreComponent(ref movement); }
    private Movement movement;
    [SerializeField] protected SO_WeaponData weaponData;
    protected Animator animator;
    //weapon animator here

    protected PlayerAttackState state;

    protected Core core;
    protected bool isHitboxActive;

    protected int attackCounter;
    private float lastHitTime;
    protected virtual void Awake()
    {
        animator = transform.Find("Base").GetComponent<Animator>();
        gameObject.SetActive(false);
    }

    public virtual void EnterWeapon()
    {
        gameObject.SetActive(true);
        if (attackCounter >= weaponData.AmountOfAttacks || Time.time > lastHitTime + 0.35)
            attackCounter = 0;
        animator.SetBool("attack", true);
        //weapon here

        animator.SetInteger("attackCounter", attackCounter);

    }

    public virtual void ExitWeapon()
    {
        animator.SetBool("attack", false);
        attackCounter++;
        gameObject.SetActive(false);
    }



    #region Animation
    public virtual void AnimationFinishTrigger()
    {
        state.AnimationFinishTrigger();
    }
    #endregion

    public virtual void AnimationHitboxActiveTrigger()
    {
        isHitboxActive = true;
    }

    public virtual void AnimationHitboxInactiveTrigger()
    {
        isHitboxActive = false;
    }

    public virtual void AnimationStartMovementTrigger()
    {
        state.SetPlayerVelocity(weaponData.MovementSpeed[attackCounter]);
    }

    public virtual void AnimationStopMovementTrigger()
    {
        state.SetPlayerVelocity(0);
    }

    public virtual void AnimationTurnOffFlipTrigger()
    {
        state.SetFlipCheck(false);
    }

    public virtual void AnimationTurnOnFlipTrigger()
    {
        state.SetFlipCheck(true);
    }

    public virtual void AnimationActionTrigger()
    {

    }

    public virtual void AnimationNextMoveFire()
    {
        lastHitTime = Time.time;
        state.NextMoveFire();
    }

    public virtual void AnimationMovementInputListenTrigger()
    {
        state.MovementInputListen();
    }
    public void InitializeWeapon(PlayerAttackState state, Core core)
    {
        this.state = state;
        this.core = core;
    }
}
