using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    UIManager ui;
    //BossAnimationManager bossAnim;

    [SerializeField] float horizontalForce;
    GameObject kirby;
    Rigidbody2D rb;

    private float travelDirection;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        kirby = GameObject.Find("Kirby");
        ui = GameObject.Find("Canvas").GetComponent<UIManager>();
        //bossAnim = GameObject.Find("WhispyWoods").GetComponent<BossAnimationManager>();
        ui = GameObject.Find("Canvas").GetComponent<UIManager>();
        travelDirection = kirby.transform.localScale.x;
    }

    void Update()
    {
        //if the boss is defeated and there are still some enemies on screen, destroy them
        if (ui.isDefeated)
        {
            Destroy(this.gameObject);
        }
    }
    private void FixedUpdate()
    {
        //move the attack in the direction facing (determined by multiplying Kirby's local scale, which will be -1 if left and 1 if right)
        transform.Translate( horizontalForce * travelDirection * Time.fixedDeltaTime,0,0);
        
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if kirby's attack collides with Whispy Woods, deal damage to the boss and destroy the attack on impact
        if (collision.gameObject.tag == "Boss" && collision.gameObject.tag != "Player")
        {
            //send to reduce scale of boss bar
            ui.SendMessage("BossDamage");

            //send to do hurt animation
            //bossAnim.SendMessage("HurtAnimation"); 

            Destroy(this.gameObject);
            
        }
        else if (collision.gameObject.tag != "Player" && collision.gameObject.tag != "Ground")
        {
            //Debug.Log(collision.gameObject.name);
            Destroy(this.gameObject);
        }
    }
}
