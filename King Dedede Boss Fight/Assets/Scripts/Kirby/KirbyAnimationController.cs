using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class KirbyAnimationController : MonoBehaviour
{
    //Kirby References
    private KirbyInputHandler input;

    private KirbyInhale kirbyInhale;
    private KirbyMovement kirbyMovement;
    private Animator animator;

    //Particle Elements
    private GameObject inhaleGameObject;
    private GameObject runDustGameObject;
    private ParticleSystem runDustParticle;
    private ParticleSystem starFall;

    //Variables
    public bool inhaleLoop; //inhale anim does a unique start, then goes into a new looping animation
    public bool canPlayStar;
    private bool canPlayRunSound = true;


    private void Awake()
    {
        //References
        input = GetComponent<KirbyInputHandler>();

        kirbyInhale = GetComponent<KirbyInhale>();
        kirbyMovement = GetComponent<KirbyMovement>();
        animator = GetComponent<Animator>();

        //Particle Getting
        runDustGameObject = transform.GetChild(1).gameObject;
        runDustParticle = runDustGameObject.GetComponent<ParticleSystem>();

        starFall = transform.GetChild(2).gameObject.GetComponent<ParticleSystem>();
        inhaleGameObject = transform.GetChild(0).transform.GetChild(1).gameObject;

    }

    private void Update()
    {
        if (GameManager.gameEnded)
            return;

        animController();
        animControllerFood();
        ParticleController();
    }
  


    void animController()
    {
        //Running Anims
        bool anim_Run = input.HorizontalMovement != 0 && !kirbyInhale.inhaling && !kirbyMovement.isShielding;
        bool anim_Idle = input.HorizontalMovement == 0 && !kirbyInhale.inhaling && kirbyMovement.isGrounded;

        if (anim_Run)
        {
            animator.SetInteger("animState", 1);

            if(canPlayRunSound && kirbyMovement.isGrounded)
            {
                AudioManager.Instance.PlaySound("k_run");
                canPlayRunSound = false;
            }
                
        }
        else if (anim_Idle)
        {
            animator.SetInteger("animState", 0);
            canPlayRunSound = true;
        }
            

        //Jump / Puff / Fall Anims
        bool anim_PuffStart = kirbyMovement.isHoldingJumpPuff;
        bool anim_PuffIdle = kirbyMovement.isPuffed;
        bool anim_Exhaled = kirbyInhale.frozen;

        bool anim_JumpingUp = kirbyMovement.isHoldingJump || kirbyMovement.speed.y > 0 && !kirbyInhale.inhaling;
        bool anim_PeakOfJump = kirbyMovement.isJumping && kirbyMovement.speed.y <= 0 && !kirbyInhale.inhaling;

        bool anim_IsFalling = kirbyMovement.speed.y <= 0 && !kirbyMovement.isGrounded && !kirbyMovement.bigFall && !kirbyInhale.inhaling;
        bool anim_BigFall = kirbyMovement.bigFall && !kirbyInhale.inhaling;

        if (anim_PuffStart)
            animator.SetInteger("animState", 16);

        else if (anim_PuffIdle)
            animator.SetInteger("animState", 17);

        else if (anim_Exhaled)
            animator.SetInteger("animState", 12);

        //Jumping Anims
        else if (anim_JumpingUp)
            animator.SetInteger("animState", 2);

        else if (anim_PeakOfJump)
            animator.SetInteger("animState", 3);

        //Falling Anims
        else if (anim_IsFalling)
            animator.SetInteger("animState", 4);

        else if (anim_BigFall)
            animator.SetInteger("animState", 5);

        //Inhaling Anims
        bool anim_InhaleStart = kirbyInhale.inhaling && inhaleLoop == false;
        bool anim_InhaleLoop = kirbyInhale.inhaling;

        if (anim_InhaleStart)
            animator.SetInteger("animState", 6);

        else if (anim_InhaleLoop)
            animator.SetInteger("animState", 15);

        //Shielding Anims
        bool anim_Shielding = kirbyMovement.isShielding;

        if(anim_Shielding)
            animator.SetInteger("animState", 18);
        
    }
    void animControllerFood()
    {
        //Moving Food Anims
        bool anim_exhaled = kirbyInhale.frozen;
        bool anim_foodRun = kirbyInhale.hasFood && input.HorizontalMovement != 0 && !kirbyInhale.frozen;
        bool anim_foodIdle = input.HorizontalMovement == 0 && kirbyInhale.hasFood;

        if (anim_exhaled)
            animator.SetInteger("animState", 12);

        else if (anim_foodRun)
            animator.SetInteger("animState", 8);

        else if (anim_foodIdle)
            animator.SetInteger("animState", 7);

        //Jumping / Falling Food Anims
        bool anim_foodJump = kirbyMovement.isHoldingJump && kirbyInhale.hasFood || kirbyMovement.speed.y > 0 && kirbyInhale.hasFood;
        bool anim_foodJumpPeak = kirbyMovement.isJumping && kirbyMovement.speed.y <= 0 && kirbyInhale.hasFood;
        bool anim_foodFalling = kirbyMovement.speed.y <= 0 && !kirbyMovement.isGrounded && kirbyInhale.hasFood;

        if (anim_foodJump)
            animator.SetInteger("animState", 9);

        if (anim_foodJumpPeak)
            animator.SetInteger("animState", 10);

        else if (anim_foodFalling)
            animator.SetInteger("animState", 11);

    }


    private void ParticleController()
    {
        #region Dust Particles
        bool isMovingOnGround = input.HorizontalMovement != 0 && !kirbyInhale.inhaling && !kirbyMovement.isShielding && kirbyMovement.isGrounded;
        if (isMovingOnGround)
        {
            runDustParticle.startSpeed = 0.4f * -Mathf.Sign(input.HorizontalMovement);
            runDustGameObject.SetActive(true);
        }
        else
            runDustGameObject.SetActive(false);

        #endregion

        #region Star Particle When Hit Ground

        if (kirbyMovement.isGrounded && canPlayStar)
        {
            starFall.Play();
            canPlayStar = false;
        }
        #endregion

        #region Inhaling Particles When Hold Down LMB

        if (kirbyInhale.inhaling)
        {
            inhaleGameObject.SetActive(true);

            //flip Inhale Game Object depending on scale of player
            float direction = inhaleGameObject.transform.localScale.x;
            direction = transform.localScale.x;

            inhaleGameObject.transform.localScale = new Vector3(direction, 1, 1);

        }
        else if (!kirbyInhale.inhaling)
            inhaleGameObject.SetActive(false);

        #endregion
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
        GameManager.gameEnded = true;

        animator.SetInteger("animState", 19);

        AudioManager.Instance.StopAll();
        AudioManager.Instance.PlaySound("bgm_victory");
    }
    void Outro3()
    {
        //If kirby loses all health, then lose
        GameManager.gameEnded = true;

        animator.SetInteger("animState", 20);

        AudioManager.Instance.StopAll();
        AudioManager.Instance.PlaySound("bgm_defeat");
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
