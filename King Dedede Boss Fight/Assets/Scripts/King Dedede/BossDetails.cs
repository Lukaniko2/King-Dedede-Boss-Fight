using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDetails : MonoBehaviour
{
    [SerializeField] private SO_BossDefeatedEventSender bossParams;
    public float health { get; private set; }

    private void Awake()
    {
        bossParams.updateHealthEvent.AddListener(UpdateHealth);
        health = 100f;
    }

    private void UpdateHealth(float newHealth)
    {
        health = newHealth;
    }
}
