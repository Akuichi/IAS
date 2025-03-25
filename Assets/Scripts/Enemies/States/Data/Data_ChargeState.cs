using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newChargeStateData", menuName = "Data/State Data/Charge State")]
public class Data_ChargeState : ScriptableObject
{
    public float ChargeSpeed = 6f;
    public float ChargeTime = 2f;
}
