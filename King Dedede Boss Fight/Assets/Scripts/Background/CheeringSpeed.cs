using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CheeringSpeed : MonoBehaviour
{
    [SerializeField] private SO_AdjustHealth checkIfBossHit;
    [SerializeField] private float speedOfCheering;
    [SerializeField] private float maxTimeBeforeNormal;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        checkIfBossHit.changeBossHealthEvent.AddListener(SpeedUpCheering);
    }

    private void SpeedUpCheering(ChangeHealth temp)
    {
        animator.speed = speedOfCheering;
        Invoke("ReturnToNormalSpeed", maxTimeBeforeNormal);
    }

    private void ReturnToNormalSpeed()
    {
        animator.speed = 1;
    }
}
