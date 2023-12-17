using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Vehicle Properties such as frequency, upgrades
/// </summary>
/// 
[CreateAssetMenu(fileName = "New Vehicle", menuName ="Vehicle")]
public class VehicleProperties : ScriptableObject {

    public float speed;
    public int currentUpgradeLevel;
}
