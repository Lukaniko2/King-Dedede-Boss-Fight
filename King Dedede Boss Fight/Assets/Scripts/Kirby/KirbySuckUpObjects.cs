using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class KirbySuckUpObjects : MonoBehaviour
{
    //References
    [SerializeField] private SO_KirbyValueParams kirbyValueParams;
    private KirbyInhale kirbyInhale;

    //Components
    private PolygonCollider2D inhaleCollider;

    private void Awake()
    {
        kirbyInhale = transform.parent.GetComponent<KirbyInhale>();
        inhaleCollider = GetComponent<PolygonCollider2D>();
    }
    private void Update()
    {
        //if don't activate it only when inhaling, the OnTrigger stay wares off after a few seconds
        if (kirbyInhale.inhaling)
            inhaleCollider.enabled = true;
        else
            inhaleCollider.enabled = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("Catch");
        //If the object can't be sucked up, return
        if (!collision.gameObject.CompareTag("Suckable"))
            return;

        Vector2 pos = collision.transform.position;
        
        if (kirbyInhale.inhaling && !kirbyInhale.hasFood)
        {
            //move the object closer to kirby
            Vector2 dirToKirby = pos - (Vector2)transform.position;

            dirToKirby.Normalize();

            pos -= dirToKirby * kirbyValueParams.suckSpeed * Time.deltaTime;

            collision.transform.position = pos;
        }

    }
}
