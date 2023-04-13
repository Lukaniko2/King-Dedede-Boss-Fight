using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KirbyCollisionsManager : MonoBehaviour
{
    [SerializeField] private SO_AdjustHealth adjustHealth;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            //hurt Kirby
            adjustHealth.changeKirbyHealthEvent.Invoke(ChangeHealth.Default_Damage);

            Debug.Log("APPLE HURT");

        }
    }

    
}
