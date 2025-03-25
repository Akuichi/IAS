using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newStunStateData", menuName = "Data/State Data/Stun State")]
public class Data_StunState : ScriptableObject
{
    public float StunDuration = 3f;
    public float StunKnockbackDuration = 0.2f;
    public float StunKnockbackSpeed = 20f;
    public Vector2 StunKnockbackAngle;
}
