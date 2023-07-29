using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KirbyShield : MonoBehaviour
{
    //Components
    [SerializeField] private SO_KirbyValueParams kirbyParams;
    private KirbyInputHandler input;
    private KirbyMovement kirbyMov;
    private KirbyInhale kirbyInhale;

    private void Awake()
    {
        input = GetComponent<KirbyInputHandler>();
        kirbyMov = GetComponent<KirbyMovement>();
        kirbyInhale = GetComponent<KirbyInhale>();

    }

    private void Update()
    {
        if (!GameManager.gameEnded)
            Shielding();
    }

    private void Shielding()
    { 
        bool pressedShieldButtonAudio = (input.playerInput.actions["Shield"].WasPerformedThisFrame() || input.playerInput.actions["Shield"].WasReleasedThisFrame() )&& !kirbyMov.isPuffed && !kirbyInhale.inhaling && !kirbyInhale.hasFood;
        bool canShield = input.IsHoldingShield && !kirbyMov.isPuffed && !kirbyInhale.inhaling && !kirbyInhale.hasFood;
        
        //Debug.Log(input.playerInput.actions["Shield"].WasPerformedThisFrame());

        if (pressedShieldButtonAudio)
        {
            //Play Sound
            AudioManager.Instance.StopSound("k_shield");
            AudioManager.Instance.PlaySound("k_shield");
        }
        else if (canShield)
        {
           
            kirbyMov.isShielding = true;

            //slow down Kirby's x movement when holding down the shield button (RMB)
            kirbyMov.SendMessage("EaseMove");
            
        }

        if (!input.IsHoldingShield)
        {
            //if Let go of RMP, they aren't shielding anymore and have their original speed back
            kirbyMov.isShielding = false;

        }
    }
}
