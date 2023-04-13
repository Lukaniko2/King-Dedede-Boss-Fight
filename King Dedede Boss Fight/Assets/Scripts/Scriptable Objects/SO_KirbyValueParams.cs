using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Kirby Movement Parameters", menuName = "ScriptableObjects/Kirby_Movement")]
public class SO_KirbyValueParams : ScriptableObject
{
    [Header("MOVEMENT PARAMETERS")]
    public float slowDownInAir; //when inhaling or shielding in air, slow movement down to 0

    [Header("Jump Parameters")]
    public float jumpStrength;
    public float maxJumpHold;

    public float minSpeedRegularJump;
    public float minSpeedPuffJump;

    public float gravityRegularJump;
    public float gravityPuffJump;

    //Jump Puff Values
    public float maxJumpHoldPuff;
    public float jumpPuffForce;

    [Header("INHALING PARAMETERS")]
    public float suckSpeed;

    [Header("HEALTH PARAMETERS")]
    public float maxHealth;
    public float kirbyDamageShieldingDivider;
    public float kirbyDamageRegular;

    [Header("INVINCIBILITY PARAMETERS")]
    public float maxInvinibilityTimer;
    public float flickerRate;

    

}
