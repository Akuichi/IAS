using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "newIdleStateData", menuName = "Data/State Data/Idle State")]
public class Data_IdleState : ScriptableObject
{
    public float MinIdleTime = 1f;
    public float MaxIdleTime = 2f;
}
