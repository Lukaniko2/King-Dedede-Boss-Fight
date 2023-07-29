using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryStars : MonoBehaviour
{
    /// <summary>
    /// Plays the Kirby Dance Animation Once touched
    /// </summary>
    private KirbyAnimationController anim;

    
    void Start()
    {
        anim = GameObject.FindObjectOfType<KirbyAnimationController>().GetComponent<KirbyAnimationController>();
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
