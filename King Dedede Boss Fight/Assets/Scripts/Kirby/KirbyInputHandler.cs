using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class KirbyInputHandler : MonoBehaviour
{
    //values will be read by playerMovement and playerAnimationsScripts
    //Movement
    private int horizontalMovement;
    public int HorizontalMovement
    {
        get => horizontalMovement;
        set => horizontalMovement = value;
    }

    //Jumping
    private bool isHoldingJump;
    public bool IsHoldingJump
    {
        get => isHoldingJump;
        set => isHoldingJump = value;
    }
    public InputAction jumpPressedInput;

    //Inhaling
    private bool isHoldingInhale;
    public bool IsHoldingInhale
    {
        get => isHoldingInhale;
        set => isHoldingInhale = value;
    }
    public InputAction inhalePressedInput;

    //Shielding
    private bool isHoldingShield;
    public bool IsHoldingShield
    {
        get => isHoldingShield;
        set => isHoldingShield = value;
    }
    public InputAction shieldPressedInput;

    private void Awake()
    {
        jumpPressedInput.Enable();
        shieldPressedInput.Enable();
        inhalePressedInput.Enable();
    }

    //Setting the values of the inputs based on the context of the press
    public void OnHoriMovement(InputAction.CallbackContext context)
    {
        if (context.started)
            return;

        Vector2 allDirections = context.ReadValue<Vector2>();
        HorizontalMovement = (int)allDirections.x;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
            return;

        IsHoldingJump = context.action.triggered;
    }

    public void OnInhale(InputAction.CallbackContext context)
    {
        if (context.started)
            return;

        IsHoldingInhale = context.action.triggered;
    }
    public void OnShield(InputAction.CallbackContext context)
    {
        if (context.started)
            return;

        IsHoldingShield = context.action.triggered;
    }
}
