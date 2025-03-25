using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct WeaponAttackDetails
{
    public string AttackName;
    public float MovementSpeed;
    public float DamageAmount;
    public float KnockbackStrength;
    public float PoiseDamageAmount;
    public Vector2 KnockbackAngle;
}
