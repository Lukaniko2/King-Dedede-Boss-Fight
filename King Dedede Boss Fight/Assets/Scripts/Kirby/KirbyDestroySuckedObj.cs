using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KirbyDestroySuckedObj : MonoBehaviour
{
    //References
    private KirbyInhale kirbyInhale;

    private void Awake()
    {
        kirbyInhale = transform.root.GetComponent<KirbyInhale>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //If the object can't be sucked up, return
        if (!collision.gameObject.CompareTag("Suckable"))
            return;

        //If they're not inhaling, don't destroy any objects either
        if (!kirbyInhale.inhaling)
            return;

        Destroy(collision.gameObject);
        kirbyInhale.hasFood = true;
        kirbyInhale.inhaling = false;

    }
}
