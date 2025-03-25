using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newEntityData", menuName = "Data/Entity Data/Base Data")]
public class Data_Entity : ScriptableObject
{
    public float WallCheckDistance = 0.2f;
    public float LedgeCheckDistance = 0.4f;
    public float GroundCheckRadius = 0.3f;

    public float MinAggroDistance = 3f;
    public float MaxAggroDistance = 4f;

    public float StunRecoveryTime = 2f;

    public float CloseRangeActionDistance = 1f;
    public GameObject HitParticleEffect;

    public LayerMask WhatIsPlayer;
    public LayerMask WhatIsGround;
}
