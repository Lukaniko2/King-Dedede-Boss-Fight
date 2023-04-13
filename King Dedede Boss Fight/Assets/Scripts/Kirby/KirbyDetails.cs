using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KirbyDetails : MonoBehaviour
{
    [SerializeField] private float kirbyHealth;
    public float KirbyHealth
    {
        get => kirbyHealth;
        set
        {
            kirbyHealth = value;
            kirbyHealth = Mathf.Clamp(kirbyHealth, 0, 100);
        }
    }

    private void Start()
    {
        kirbyHealth = 100;
    }
}
