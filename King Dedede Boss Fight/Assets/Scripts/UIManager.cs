using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    //Whispy Woods Variables
    [SerializeField] GameObject bossBar;
    [SerializeField] GameObject bossBlueBar;
    [SerializeField] float blueBarIncrease; //when boss initiates, increase blue bar
    [SerializeField] float percentBossDamage; //amount of health taken away from the boss' health bar per hit

    public bool bossBarVisible;
    public bool isDefeated = false;
    float a; // is assigned to be the scale.x of the boss blue bar
    bool finishedBossIntro = false;

    //Kirby Variables
    public GameObject kirbyBar;
    public float percentKirbyDamage; //how much damage is taken from enemy attacks

    KirbyAnimationController anim;
    //TImer for invincibility
    [SerializeField] SpriteRenderer kirby;
    [SerializeField] float maxInvinibilityTimer;
    private float currentTime;

    [SerializeField] float flickerRate;
    Color color = Color.white;

    //sounds
    AudioSource audioSource;
    [SerializeField]AudioClip[] sounds;

    private void Awake()
    {
        anim = GameObject.Find("Kirby").GetComponent<KirbyAnimationController>();
        currentTime = Time.time;
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        bossBarManager();

        //if Kirby is invincible, rapidly change Kirby's sprite to flicker
        if (Time.time - currentTime <= maxInvinibilityTimer && kirbyBar.transform.localScale.x < 0.43)
        {
            InvokeRepeating("Invincible", 0, flickerRate);
        }
        else
        {
            CancelInvoke();
            color.a =1;
            kirby.color = new Color(1, 1, 1, color.a);
        }
    }

    void Invincible()
    {
        color.a++;
        color.a %= 2;
        kirby.color = new Color(1, 1, 1, color.a);
    }
    void bossBarManager()
    {
        //at the beginning when Kirby enters the boss area to increase the health bar from 0 to full
        if (bossBarVisible && !finishedBossIntro)
        {
            IncreaseBossBar();
        }

    }

    void IncreaseBossBar()
    {
        //get the current scale of the boss bar (starts at 0) 
        //increase the scale in the x axis every update until it reaches full capacity
        //called at the beginning when kirby enters the boss trigger
        //boss bar increases from 0 to full

        bossBar.SetActive(true);
        audioSource.clip = sounds[3];
        audioSource.Play();

        a = bossBlueBar.transform.localScale.x;
        a += blueBarIncrease * Time.deltaTime;

        bossBlueBar.transform.localScale = new Vector3(a, bossBlueBar.transform.localScale.y, 0);
       
        //when the boss bar loads up all the way to being full, then commence the battle and keep it's scale at max
        if(a >= 0.3836945f)
        {
            finishedBossIntro = true;
            a = 0.3836945f;
            audioSource.Stop();
        }
    }

    void BossDamage()
    {
        audioSource.clip = sounds[1];
        audioSource.Play();

        //manages updating the boss' health when hit with an attack
        //Is Sent a message by the Attack class to run when the attack collides with Whispy Woods (The boss)
        bossBlueBar.transform.localScale -= new Vector3(a - percentBossDamage, 0, 0);

        //if the boss' health is fully depleted then hide the bar and set the victory condition (isDefeated) to true
        if(bossBlueBar.transform.localScale.x <= 0)
        {
            audioSource.clip = sounds[2];
            audioSource.Play();

            bossBlueBar.SetActive(false);
            isDefeated = true;
        }
    }


    void KirbyDamage()
    {
        if(Time.time - currentTime >= maxInvinibilityTimer)
        {
            audioSource.clip = sounds[0];
            audioSource.Play();

            //gets the current scale of Kirby's health bar

            //subtracts a certain amount from the health bar to inflict damage
            kirbyBar.transform.localScale -= new Vector3(percentKirbyDamage, 0, 0);

            //if Kirby's health bar is less than 0, then the player loses
            if (kirbyBar.transform.localScale.x <= 0)
            {
                anim.SendMessage("Outro", "Outro3");
                kirbyBar.transform.localScale = new Vector3(0, kirbyBar.transform.localScale.y, kirbyBar.transform.localScale.z);
            }
            currentTime = Time.time;
        }
        
    }

}
