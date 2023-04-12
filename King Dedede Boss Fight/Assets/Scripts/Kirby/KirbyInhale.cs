using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KirbyInhale : MonoBehaviour
{
    //components
    [SerializeField] private SO_KirbyValueParams kirbyParams;
    private KirbyInputHandler input;
    

    private KirbyMovement kirbyMov;
    private KirbyAnimationController animController;
    [SerializeField] private GameObject kirbyMouth;
    [SerializeField] private GameObject attackPrefab;


    public float velX; //store speed
    
    public bool hasFood = false;
    public bool inhaling = false;
    public bool canInhale = true; //checks to see if can inhale after certain amount of time after releasing the LMB
    float currentGravity; // when spitFood in air or huff in air, pause for a bit
    private Vector2 currentSpeed; // when spitFood in air or huff in air, pause for a bit
    public bool frozen = false;

    //sounds
    AudioSource audioSource;
    [SerializeField] AudioClip[] sounds;

    private void Awake()
    {
        input = GetComponent<KirbyInputHandler>();

        kirbyMov = GetComponent<KirbyMovement>();
        audioSource = GetComponent<AudioSource>();
        animController = GetComponent<KirbyAnimationController>();

        velX = kirbyMov.speed.x;
        currentGravity = kirbyMov.codeGravity;
        
    }

    // Update is called once per frame
    void Update()
    {
        Inhale();
    }

    void Inhale()
    {
        //if the player presses the left mouse button LMB, then freeze Kirby in his position and update the sprites
        //do enable trigger so that if the enemy stays in the trigger, they will get sucked in
        //Do math to calculate distance from kirby's mouth and their position to lerp it there (gameObject infront of kirby mouth)
        //Once the enemy is being sucked in, enable "isTrigger" and if the kirby collider hits the trigger, then destroy the game object

        bool canPerformInhale = input.IsHoldingInhale && !hasFood && canInhale && !kirbyMov.isPuffed && !kirbyMov.isSheilding;
        bool letGoOfInhale = !input.IsHoldingInhale;
        bool letGoOfInhaleNotPuffed = letGoOfInhale && !kirbyMov.isPuffed && !kirbyMov.isSheilding;
        bool exhaleInAir = input.inhalePressedInput.WasPerformedThisFrame() && kirbyMov.isPuffed && !kirbyMov.isSheilding;
        bool canThrowFood = input.IsHoldingInhale && hasFood;

        if (canPerformInhale)
        {
            kirbyMov.SendMessage("EaseMove");
            //while the player holds the LMB, they are inhaling so set the inhaling bool to true
            inhaling = true;

            //play Inhale Sound only if they are holding down LMB
            AudioManager.Instance.PlaySound("k_inhale");


        }
        else if(exhaleInAir)
        {
            //play exhale sound
            AudioManager.Instance.StopSound("k_puffJump");
            AudioManager.Instance.PlaySound("k_exhale");

            //if they press LMB while puffed in the air, return to original gravity
            canInhale = false;
            kirbyMov.isPuffed = false;
            kirbyMov.isHoldingJumpPuff = false;
            kirbyMov.codeGravity = kirbyParams.gravityRegularJump;
            kirbyMov.codeMinSpeed = kirbyParams.minSpeedRegularJump;
            FreezeGravity();

        }
        else if (letGoOfInhaleNotPuffed)
        {
            AudioManager.Instance.StopSound("k_inhale");

            //if the player lets go of LMB, then they are not inhaling anymore and can move again
            kirbyMov.speed.x = velX;
            inhaling = false;
            animController.inhaleLoop = false;

        }
        else if (canThrowFood)
        {
            //Kirby has food and cannot inhale anymore
            //instantiate a gameObject with an attack sprite
            Instantiate(attackPrefab, kirbyMouth.transform.position, kirbyMouth.transform.rotation);
            hasFood = false;

            FreezeGravity();
        }

    }

    public void FreezeGravity()
    {
        //code below gets kirby's current speed to save it for later
        //then it freezes kirby in the air for 0.3 seconds until the function is invoked
        canInhale = false;
        currentSpeed.y = kirbyMov.speed.y;
        kirbyMov.codeGravity = 0;
        kirbyMov.speed.y = 0;
        frozen = true;
        Invoke("ReinsstateGravity", 0.3f);

        //Right after Kirby freezes in air, they can't keep holding LMB to suck right away so we set canInhale to false and set it back to true in the Invoke
        
        Invoke("ReloadInhale", 0.5f);
    }

    


    void ReinsstateGravity()
    {
        //set Kirby's movement back to what it was before freezing Kirby in air
        kirbyMov.codeGravity = currentGravity;
        kirbyMov.speed.y = currentSpeed.y;
        frozen = false;
    }

    void ReloadInhale()
    {
        //will be invoked. Have a time delay after the player shoots out their attack (Input.GetButtonUp)
        canInhale = true;
    }

}
