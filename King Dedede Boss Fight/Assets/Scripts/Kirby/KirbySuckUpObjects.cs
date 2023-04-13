using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class KirbySuckUpObjects : MonoBehaviour
{
    //References
    [SerializeField] private SO_KirbyValueParams kirbyValueParams;
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
