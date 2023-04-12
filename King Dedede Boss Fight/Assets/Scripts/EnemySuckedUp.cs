using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySuckedUp : MonoBehaviour
{

    KirbyInhale kirbyInhale;
    GameObject kirbyMouth;
    [SerializeField] Vector2 suckSpeed;
    Vector2 sign;
    // Start is called before the first frame update
    void Awake()
    {
        kirbyInhale = GameObject.Find("Kirby").GetComponent<KirbyInhale>();
        kirbyMouth = GameObject.Find("InhaleArea");
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if(kirbyInhale.inhaling && !kirbyInhale.hasFood)
        {
            //get current position
            //add speed x and y and add sin
            Vector2 pos = transform.position;

            
            if (other.name == "InhaleArea")
            {
                //if they are inside the inhale area's radius (trigger), calculate which direction they need to move in order to reach Kirby
                sign.x = Mathf.Sign(kirbyMouth.transform.position.x - transform.position.x);
                sign.y = Mathf.Sign(kirbyMouth.transform.position.y - transform.position.y);

                //calculate the angle in which to travel in the Y axis
                float a = (Vector2.Angle(transform.position, kirbyMouth.transform.position));
                //Debug.Log(Vector2.Angle(transform.position, kirbyMouth.transform.position));
                pos.x += suckSpeed.x * sign.x * Time.deltaTime;
                pos.y += suckSpeed.y * sign.y * Mathf.Sin(a) * Time.deltaTime;
                //transform.Translate(suckSpeed.x * sign.x * Time.deltaTime, suckSpeed.y * sign.y * Mathf.Sin(a) * Mathf.Cos(a) * Time.deltaTime, 0);
                
                transform.position = pos;
            }

            if (other.name == "DestroyArea")
            {
                //if the apple is close to Kirby's mouth, then destroy the gameObject
                //Make kirby have food since they swallowed an enemy
                Destroy(this.gameObject);
                kirbyInhale.hasFood = true;
                kirbyInhale.inhaling = false;
            }
        }

      
    }
   
}
