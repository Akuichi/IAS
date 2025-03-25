using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newDeadStateData", menuName = "Data/State Data/Dead State")]
public class Data_DeadState : ScriptableObject
{
    public GameObject DeathChunkParticle;
    public GameObject DeathBloodParticle;
}
