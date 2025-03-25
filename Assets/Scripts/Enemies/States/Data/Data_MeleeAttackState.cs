using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "newMeleeAttackStateData", menuName = "Data/State Data/Melee Attack State")]
public class Data_MeleeAttackState : ScriptableObject
{
    public float PoiseDamage = 40f;
    public float AttackRadius = 0.5f;
    public float AttackDamage = 10f;
    public Vector2 KnockbackAngle = Vector2.one;
    public float KnockbackStrength = 10f;

    public LayerMask WhatIsPlayer;
}
