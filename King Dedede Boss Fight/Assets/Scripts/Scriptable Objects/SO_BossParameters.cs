using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Boss Parameters", menuName = "ScriptableObjects/BossParameters")]
public class SO_BossParameters : ScriptableObject
{
    public string bossName;

    [Header("HEALTH PARAMETERS")]
    public float bossDamageDecrement;

    [Header("MOVEMENT PARAMETERS")]
    public float bossSpeedSlow;
    public float bossSpeed;
    public float maxIdleTime;

}
