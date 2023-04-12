using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KirbyCollisionsManager : MonoBehaviour
{
 
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Apple")
        {
            //hurt Kirby
            //inflicts damage (reduces health bar)
            //ui.SendMessage("KirbyDamage");

            Debug.Log("APPLE HURT");
            Destroy(collision.gameObject);
        }
    }

    
}
