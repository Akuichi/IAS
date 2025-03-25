using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newSearchStateData", menuName = "Data/State Data/Search State")]
public class Data_SearchState : ScriptableObject
{
    public int AmountOfTurns = 2;
    public float TimeBetweenTurns = 0.75f;
}
