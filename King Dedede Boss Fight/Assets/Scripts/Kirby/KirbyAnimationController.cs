using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class KirbyAnimationController : MonoBehaviour
{
    //Components
    KirbyInputHandler input;

    KirbyInhale kirbyInhale;
    KirbyMovement kirbyMovement;
    Animator animator;
    CameraMovement cam;

    public bool inhaleLoop;
    public bool canPlayStar;

    //Particle Elements
    GameObject run;
    ParticleSystem runDust;

    ParticleSystem starFall;

    GameObject breathe;
    ParticleSystem inhale;

    //Audio Components
    AudioSource audioSource;
    [SerializeField] AudioClip[] sounds;

    //star
    [SerializeField] VictoryStars stars;
    public bool gameHalt = false;
    // Start is called before the first frame update
    void Awake()
    {
        input = GetComponent<KirbyInputHandler>();

        cam = GameObject.Find("BossStartTrigger").GetComponent<CameraMovement>();
        kirbyInhale = GetComponent<KirbyInhale>();
        kirbyMovement = GetComponent<KirbyMovement>();
        animator = GetComponent<Animator>();
        run = GameObject.Find("RunDust");
        runDust = run.GetComponent<ParticleSystem>();
        starFall = GameObject.Find("StarFall").GetComponent<ParticleSystem>();
        breathe = GameObject.Find("Inhale");
        inhale = breathe.GetComponent<ParticleSystem>();

        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //KIRBY HAS A LOT OF ANIMATIONS!! AND THEY'RE SO ANNOYING TO CONFIGURE!!
        if(!gameHalt)
        {
            animController();
            animControllerFood();
            ParticleController();
        }

    }
  


    void animController()
    {
        //set animation state to running if horizontal movement
        if (input.HorizontalMovement.x != 0 && !kirbyInhale.inhaling)
        {
            animator.SetInteger("animState", 1);
        }
        //set animation state to idle animation if no horizontal movement
        else if (input.HorizontalMovement.x == 0 && !kirbyInhale.inhaling && kirbyMovement.isGrounded)
        {
            animator.SetInteger("animState", 0);

            if(!input.IsHoldingInhale)
            {
               // AudioManager.Instance.PlaySound("k_run");
            }
            
        }

        //if Kirby is puffed
        if (kirbyMovement.isHoldingJumpPuff)
        {
            animator.SetInteger("animState", 16);
        }
        else if (kirbyMovement.isPuffed)
        {
            animator.SetInteger("animState", 17);
        }
        else if (kirbyInhale.frozen)
        {
            animator.SetInteger("animState", 12);
            
        }
        //jump up animation when the player presses space
        else if (kirbyMovement.isHoldingJump || kirbyMovement.speed.y > 0 && !kirbyInhale.inhaling)
        {
            animator.SetInteger("animState", 2);
           
        }
        //when the jump reaches the peak
        else if (kirbyMovement.isJumping && kirbyMovement.speed.y <= 0 && !kirbyInhale.inhaling)
        {
            animator.SetInteger("animState", 3);
        }
        //isfalling
        else if (kirbyMovement.speed.y <= 0 && !kirbyMovement.isGrounded && !kirbyMovement.bigFall && !kirbyInhale.inhaling)
        {
            animator.SetInteger("animState", 4);
        }
        else if (kirbyMovement.bigFall && !kirbyInhale.inhaling)
        {
            animator.SetInteger("animState", 5);
        }

        //inhaling animations
        if (kirbyInhale.inhaling && inhaleLoop == false)
        {
            animator.SetInteger("animState", 6);
        }
        else if (kirbyInhale.inhaling)
        {
            animator.SetInteger("animState", 15);
        }

        //shielding animations
        if(kirbyMovement.isSheilding)
        {
            animator.SetInteger("animState", 18);
        }
        
    }
    void animControllerFood()
    {

        if (kirbyInhale.frozen)
        {
            animator.SetInteger("animState", 12);
        }
        //run
        else if (kirbyInhale.hasFood && input.HorizontalMovement.x != 0 && !kirbyInhale.frozen)
        {
            animator.SetInteger("animState", 8);
        }
        //idle
        else if (input.HorizontalMovement.x == 0 && kirbyInhale.hasFood)
        {
            animator.SetInteger("animState", 7);
        }
        //jump up animation when the player presses space
        if (kirbyMovement.isHoldingJump && kirbyInhale.hasFood || kirbyMovement.speed.y > 0 && kirbyInhale.hasFood)
        {
            animator.SetInteger("animState", 9);
        }
        //when the jump reaches the peak
        if (kirbyMovement.isJumping && kirbyMovement.speed.y <= 0 && kirbyInhale.hasFood)
        {
            animator.SetInteger("animState", 10);
        }
        //isfalling
        else if (kirbyMovement.speed.y <= 0 && !kirbyMovement.isGrounded && kirbyInhale.hasFood)
        {
            animator.SetInteger("animState", 11);
        }

    }


    void ParticleController()
    {
        //for making particles visible if certain conditions are met

        //if the player runs, spawn dust particles
        if(input.HorizontalMovement.x == -1 && !kirbyInhale.inhaling && kirbyMovement.isGrounded)
        {
            runDust.startSpeed = 0.4f;
            run.SetActive(true);
        }
        else if (input.HorizontalMovement.x == 1 && !kirbyInhale.inhaling && kirbyMovement.isGrounded)
        {
            runDust.startSpeed = -0.4f;
            run.SetActive(true);
        }
        else
        {
            run.SetActive(false);
        }

        //if the player falls on the ground, spawn a star particle
        if(kirbyMovement.isGrounded && canPlayStar)
        {
            starFall.Play();
            canPlayStar = false;
        }

        //when the player inhales spawn air particles that get sucked into kirby's mouth
        if(kirbyInhale.inhaling)
        {
            breathe.SetActive(true);
            float a = breathe.transform.localScale.x; //flip the sprite depending on the player's sprite
            a = transform.localScale.x;
            breathe.transform.localScale = new Vector3(a, 1, 1);

        }
        else if(!kirbyInhale.inhaling)
        {
            breathe.SetActive(false);
        }
    }

    void InhaleLoop()
    {
        inhaleLoop = true;
    }

    void doIsFalling()
    {
        animator.SetInteger("animState", 4);
    }


    //For end of game when beat boss
    void Outro(string winLose)
    {
        //this method will be called on by other scripts
        Invoke(winLose, 1);
    }
    void Outro2()
    {
        //If kirby touches the victory star after beating the boss, play the victory dance then reset the scene
        gameHalt = true;
        animator.SetInteger("animState", 19);
        audioSource.Stop();
        audioSource.clip = sounds[2];
        audioSource.Play();
    }
    void Outro3()
    {
        //If kirby touches the victory star after beating the boss, play the victory dance then reset the scene
        gameHalt = true;
        cam.SendMessage("LowerVolume");
        animator.SetInteger("animState", 20);
        audioSource.Stop();
        audioSource.clip = sounds[3];
        audioSource.Play();
    }

    //for reloading the scene after the player wins or loses
    void InvokeScene()
    {
        //this method will be called on by other scripts
        Invoke("ReinstateScene", 1);
    }
    void ReinstateScene()
    {
        SceneManager.LoadScene(0);
    }

}
