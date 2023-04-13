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
    private UIManager ui;

    private float originalPercentKirbyDamage;

    private void Awake()
    {
        input = GetComponent<KirbyInputHandler>();
        kirbyMov = GetComponent<KirbyMovement>();
        kirbyInhale = GetComponent<KirbyInhale>();

        ui = GameObject.FindObjectOfType<UIManager>();
    }

    private void Update()
    {
        if (!GameManager.gameEnded)
            Shielding();
    }

    void Shielding()
    {
        bool pressedShieldButtonAudio = input.shieldPressedInput.WasPerformedThisFrame() && !kirbyMov.isPuffed && !kirbyInhale.inhaling && !kirbyInhale.hasFood;
        bool canShield = input.IsHoldingShield && !kirbyMov.isPuffed && !kirbyInhale.inhaling && !kirbyInhale.hasFood;

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
