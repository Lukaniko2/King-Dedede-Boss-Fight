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
    private KirbyAnimationController animation;
  
    public Vector2 speed;

    //Check isGrounded Variables
    private CircleCollider2D circleCol;

    //Jump parameters
    public float codeGravity; //will switch between regular gravity and puff gravity
    public float codeMinSpeed;


    private float currentJumpHoldTime;
    public bool isGrounded = false;
    public bool isHoldingJump = false;
    public bool isJumping = false; //for the fall animation

    //shield
    public bool isSheilding = false;

    //for falling animation
    public bool bigFall = false;
    float bigFallTimer = 0.4f;

    //puff jump variables
    public bool isPuffed = false;
    public bool isHoldingJumpPuff = false;


    private void Awake()
    {
        input = GetComponent<KirbyInputHandler>();

        circleCol = GetComponent<CircleCollider2D>();
        kirbyInhale = GetComponent<KirbyInhale>();
        animation = GetComponent<KirbyAnimationController>();

        //Setting the player's regular gravity
        codeGravity = kirbyParams.gravityRegularJump; 
        codeMinSpeed = kirbyParams.minSpeedRegularJump;

    }


    // Update is called once per frame
    void Update()
    {
        checkGrounded();

        //if they didn't grab the star at the end, then run as normal
        if (!animation.gameHalt)
        {
            
            Jump();
            FlipSprite();
            
        }

    }

    private void FixedUpdate()
    {
        JumpLogic();
    }

    void checkGrounded()
    {
        LayerMask groundLayer = LayerMask.GetMask("Ground");

        //create raycast from the sprite's position downwards
        RaycastHit2D hit = Physics2D.Linecast(transform.position, transform.position + Vector3.down * (circleCol.radius + 0.01f), groundLayer);

        if (hit)
        {
            isGrounded = true;
            isPuffed = false;
        }
        else
        {
            isGrounded = false;
            animation.canPlayStar = true;
        }

        Debug.DrawLine(transform.position, transform.position + Vector3.down * (circleCol.radius + 0.01f), Color.red);
    }
    
    void Jump()
    {
        //if the player is holding down the jump button and is on the ground, make them not on the ground anymore
        if (input.IsHoldingJump && isGrounded && !kirbyInhale.inhaling && !isSheilding)
        {
            //play Jump Sound
            AudioManager.Instance.PlaySound("k_jump");

            isGrounded = false;
            isHoldingJump = true;
            speed.y = kirbyParams.jumpStrength; //set the upwards speed to the jumpStrength
            isJumping = true;
            codeGravity = kirbyParams.gravityRegularJump;
            codeMinSpeed = kirbyParams.minSpeedRegularJump;

        }
        else if (input.jumpPressedInput.WasPerformedThisFrame() && !isGrounded && !kirbyInhale.inhaling && !kirbyInhale.hasFood && !isSheilding)
        {
            //play JumpPuff Sound
            AudioManager.Instance.StopSound("k_puffJump");
            AudioManager.Instance.PlaySound("k_puffJump");

            //if they are in the air and they press jump
            //then Kirby will enter a new state called the puff state
            //Gravity will be less and they are all puffed up and can jump as many times as they want in air
            speed.y = kirbyParams.jumpPuffForce;
            isPuffed = true;
            isHoldingJumpPuff = true;
            codeGravity = kirbyParams.gravityPuffJump;
            codeMinSpeed = kirbyParams.minSpeedPuffJump; //for the clamp, the lowest value the speed can be when 
        }


    }


    void JumpLogic()
    {
        //get kirby's current position
        Vector2 pos = transform.position;


        //move Kirby

        //update the x position by multiplying it by speed and direction
        if (!kirbyInhale.frozen && !animation.gameHalt)
            pos.x += speed.x * input.HorizontalMovement.x * Time.fixedDeltaTime;
        

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
            {
                isHoldingJump = false;
            }
            if (Time.time - currentJumpHoldTime >= bigFallTimer)
            {
                bigFall = true;
            }
        }
        else if (!isGrounded && isPuffed)
        {
            pos.y += speed.y * Time.fixedDeltaTime;

            //calculate how long the player held jump for. Like processing, if the time right now minus the time when they pressed jump is greater than
            //max jump hold time, then make the player not hold jump anymore
            if (Time.time - currentJumpHoldTime >= kirbyParams.maxJumpHoldPuff)
            {
                isHoldingJumpPuff = false;
            }

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

    void FlipSprite()
    {
        //flip sprite. If the player is facing right and are holding "a", flip to -1 and vice versa
        if (input.HorizontalMovement.x < 0 && transform.localScale.x > 0)
            transform.localScale = new Vector3(-1, 1, 1);

        else if (input.HorizontalMovement.x > 0 && transform.localScale.x < 0)
            transform.localScale = new Vector3(1, 1, 1);

    }

    public void EaseMove()
    {
        //want to access it for shielding too in the kirby movement class, so is public
        //eases movement on x axis when inhaling or shielding in the air

        //to ease the movement when inhaling while on the air instead of coming to a sudden halt

        if(isGrounded)
        {
            //if the player is on the ground, then when inhaling set the speed to a hard stop
            speed.x = 0;
            return;
        }

        speed.x -= kirbyParams.slowDownInAir * Time.deltaTime;

        if (speed.x >= -0.05f && speed.x <= 0.05f)
            speed.x = 0;

        Debug.Log("Working");
    }

}
