using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class KirbyInputHandler : MonoBehaviour
{
    //values will be read by playerMovement and playerAnimationsScripts

    public PlayerInput playerInput { get; private set; }
    private void Update()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    //Movement
    private int horizontalMovement;
    public int HorizontalMovement
    {
        get => horizontalMovement;
        set => horizontalMovement = value;
    }

    //Setting the values of the inputs based on the context of the press
    public void OnHoriMovement(InputAction.CallbackContext context)
    {
        if (!context.started)
        {
            Vector2 allDirections = context.ReadValue<Vector2>();
            HorizontalMovement = (int)allDirections.x;
        }
      
    }

    //Jumping
    private bool isHoldingJump;
    public bool IsHoldingJump
    {
        get => isHoldingJump;
        set => isHoldingJump = value;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!context.started)
            IsHoldingJump = context.action.triggered;

    }

    //Inhaling
    private bool isHoldingInhale;
    public bool IsHoldingInhale
    {
        get => isHoldingInhale;
        set => isHoldingInhale = value;
    }

    public void OnInhale(InputAction.CallbackContext context)
    {
        if (!context.started)
            IsHoldingInhale = context.action.triggered;
    }

    //Shielding
    private bool isHoldingShield;
    public bool IsHoldingShield
    {
        get => isHoldingShield;
        set => isHoldingShield = value;
    }

    public void OnShield(InputAction.CallbackContext context)
    {
        if (context.started)
            return;

       IsHoldingShield = context.action.triggered;

    }
}
