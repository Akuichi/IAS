using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAnimationToWeapon : MonoBehaviour
{
    private Weapon weapon;
    private void Start()
    {
        weapon = GetComponentInParent<Weapon>();
    }

    private void AnimationFinishTrigger()
    {
        weapon.AnimationFinishTrigger();
    }

    private void AnimationStartMovementTrigger()
    {
        weapon.AnimationStartMovementTrigger();
    }

    private void AnimationStopMovementTrigger()
    {
        weapon.AnimationStopMovementTrigger();
    }

    private void AnimationTurnOffFlipTrigger()
    {
        weapon.AnimationTurnOffFlipTrigger();
    }

    private void AnimationTurnOnFlipTrigger()
    {
        weapon.AnimationTurnOnFlipTrigger();
    }

    private void AnimationActionTrigger()
    {
        weapon.AnimationActionTrigger();
    }

    private void AnimationHitboxActiveTrigger()
    {
        weapon.AnimationHitboxActiveTrigger();
    }

    private void AnimationHitboxInactiveTrigger()
    {
        weapon.AnimationHitboxInactiveTrigger();
    }

    private void AnimationNextMoveFire()
    {
        weapon.AnimationNextMoveFire();
    }    

    private void AnimationMovementInputListenTrigger()
    {
        weapon.AnimationMovementInputListenTrigger();
    }
}
