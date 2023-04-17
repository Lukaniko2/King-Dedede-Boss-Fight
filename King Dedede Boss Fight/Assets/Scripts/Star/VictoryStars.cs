using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryStars : MonoBehaviour
{
    
    KirbyAnimationController anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GameObject.Find("Kirby").GetComponent<KirbyAnimationController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if the player collides with the star after beating the boss, play the animaiton and sound (outro) and destroy the star
        if (!collision.CompareTag("Player"))
            return;


        anim.SendMessage("Outro", "Outro2");
        Destroy(this.gameObject);
       
    }
}
