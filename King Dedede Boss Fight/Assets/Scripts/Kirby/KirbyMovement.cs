using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class KirbyMovement : MonoBehaviour
{
    //Components
    private KirbyInputHandler input;

    [SerializeField] private SO_KirbyValueParams kirbyParams;

    private KirbyInhale kirbyInhale;
    private KirbyAnimationController animationComp;
  
    public Vector2 speed;

    //Check isGrounded Variables
    private CircleCollider2D circleCol;

    //Jump parameters
    public float codeGravity; //will switch between regular gravity and puff gravity
    public float codeMinSpeed;

    [HideInInspector] public float currentJumpHoldTime;
    public bool isGrounded = false;
    public bool isHoldingJump = false;
    public bool isJumping = false; //for the fall animation

    //shield
    public bool isShielding = false;

    //for falling animation
    public bool bigFall = false;
    private float bigFallTimer = 1f;

    //puff jump variables
    public bool isPuffed = false;
    public bool isHoldingJumpPuff = false;

    //1 is right, -1 is left
    [HideInInspector] public float kirbyDirectionFacing = 1;


    private void Awake()
    {
        input = GetComponent<KirbyInputHandler>();

        circleCol = GetComponent<CircleCollider2D>();
        kirbyInhale = GetComponent<KirbyInhale>();
        animationComp = GetComponent<KirbyAnimationController>();

        //Setting the player's regular gravity
        codeGravity = kirbyParams.gravityRegularJump; 
        codeMinSpeed = kirbyParams.minSpeedRegularJump;

    }


    // Update is called once per frame
    void Update()
    {
        FlipSprite();
        checkGrounded();
       

        //if they didn't grab the star at the end
        if (!GameManager.gameEnded)
            Jump();

        
    }

    private void FixedUpdate()
    {
        JumpLogic();
    }

    void checkGrounded()
    {
        LayerMask groundLayer = LayerMask.GetMask("Ground");

        RaycastHit2D hit = Physics2D.Linecast(transform.position, transform.position + Vector3.down * (circleCol.radius + 0.01f), groundLayer);

        if (hit)
        {
            isGrounded = true;
            isPuffed = false;
        }
        else
        {
            isGrounded = false;
            animationComp.canPlayStar = true;
        }

        Debug.DrawLine(transform.position, transform.position + Vector3.down * (circleCol.radius + 0.01f), Color.red);
    }
    
    void Jump()
    {
        //if the player is holding down the jump button and is on the ground, make them not on the ground anymore
        if (input.IsHoldingJump && isGrounded && !kirbyInhale.inhaling && !isShielding)
        {
            //play Jump Sound
            AudioManager.Instance.PlaySound("k_jump");

            isGrounded = false;
            isHoldingJump = true;
            speed.y = kirbyParams.jumpStrength; //set the upwards speed to the jumpStrength
            isJumping = true;
            codeGravity = kirbyParams.gravityRegularJump;
            codeMinSpeed = kirbyParams.minSpeedRegularJump;

            currentJumpHoldTime = Time.time; //resets the big fall
            bigFall = false;

        }
        //if the player jumps in the air, then make them puff
        else if (input.playerInput.actions["Jump"].WasPressedThisFrame() && !isGrounded && !kirbyInhale.inhaling && !kirbyInhale.hasFood && !isShielding && !kirbyInhale.frozen)
        {
            //play JumpPuff Sound
            AudioManager.Instance.StopSound("k_puffJump");
            AudioManager.Instance.PlaySound("k_puffJump");
            

            //if they are in the air and they press jump
            speed.y = kirbyParams.jumpPuffForce;
            isJumping = true;
            isPuffed = true;
            isHoldingJumpPuff = true;
            Invoke("ContinuePuffAnimation", 0.05f);

            //Gravity will be less and they are all puffed up and can jump as many times as they want in air
            codeGravity = kirbyParams.gravityPuffJump;
            codeMinSpeed = kirbyParams.minSpeedPuffJump;
        }


    }
    private void ContinuePuffAnimation()
    {
        isHoldingJumpPuff = false;
    }

    void JumpLogic()
    {
        //get kirby's current position
        Vector2 pos = transform.position;

        //move Kirby
        
        //update the x position by multiplying it by speed and direction
        if (!kirbyInhale.frozen && !GameManager.gameEnded)
            pos.x += speed.x * input.HorizontalMovement * Time.fixedDeltaTime;

        //if let go of jump button, simulate gravity
        if (!isHoldingJump)
        {
            speed.y += codeGravity * Time.fixedDeltaTime;
            speed.y = Mathf.Clamp(speed.y, codeMinSpeed, speed.y);
        }

        //if the player is not grounded (in the air), keep increasing their y position upwards by the speed
        if (!isGrounded && !isPuffed)
        {
            pos.y += speed.y * Time.fixedDeltaTime;

            //calculate how long the player held jump for. Like processing, if the time right now minus the time when they pressed jump is greater than
            //max jump hold time, then make the player not hold jump anymore
            if (Time.time - currentJumpHoldTime >= kirbyParams.maxJumpHold)
                isHoldingJump = false;

            if (Time.time - currentJumpHoldTime >= bigFallTimer)
                bigFall = true;

        }
        else if (!isGrounded && isPuffed)
        {
            pos.y += speed.y * Time.fixedDeltaTime;

            currentJumpHoldTime = Time.time; //resets the big fall
            bigFall = false;

        }
        else if (isGrounded)
        {
            currentJumpHoldTime = Time.time;
            isJumping = false;
            bigFall = false;
        }

        //update kirby's position because we changed the position up above without using rigidbody
        transform.position = pos;
    }

    private void FlipSprite()
    {
       
        if (input.HorizontalMovement != 0)
            kirbyDirectionFacing = input.HorizontalMovement;

        transform.localScale = new Vector3(kirbyDirectionFacing, 1, 1);

    }

    public void EaseMove()
    {
        //If grounded, no deceleration, hard stop
        if (isGrounded)
        {
            speed.x = 0;
            return;
        }

        //decelerates horizontal Movement when inhaling or shielding in air
        speed.x -= kirbyParams.slowDownInAir * Time.deltaTime;

        if (speed.x >= -0.05f && speed.x <= 0.05f)
            speed.x = 0;

    }

}
