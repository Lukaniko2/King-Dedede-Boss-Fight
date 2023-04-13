using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIManager : MonoBehaviour
{
    //References
    [SerializeField] private SO_AdjustHealth adjustHealth;
    [SerializeField] private SO_KirbyValueParams kirbyValueParams;
    [SerializeField] private SO_BossParameters bossParameters;

    private KirbyHealthBar kirbyHealthBar;
    private BossHealthBar bossHealthBar;

    //Variables
    public bool bossBarVisible;

    private bool finishedBossIntro = false;
    public bool FinishedBossIntro
    {
        get => finishedBossIntro;
        set => finishedBossIntro = value;
    }

    private void Awake()
    {
        kirbyHealthBar = GameObject.FindObjectOfType<KirbyHealthBar>();
        bossHealthBar = GameObject.FindObjectOfType<BossHealthBar>();

        //Once event is called, run these methods
        adjustHealth.changeKirbyHealthEvent.AddListener(AdjustKirbyHealth);
        adjustHealth.changeBossHealthEvent.AddListener(AdjustBossHealth);
    }

    private void AdjustKirbyHealth(ChangeHealth changeHealthState)
    {
        float healthAdjustment = GetHealthValue(changeHealthState);
        kirbyHealthBar.KirbyDamage(healthAdjustment);
    }
    private void AdjustBossHealth(ChangeHealth changeHealthState)
    {
        float healthAdjustment = GetHealthValue(changeHealthState);
        bossHealthBar.BossDamage(healthAdjustment);
    }


    private float GetHealthValue(ChangeHealth changeHealthState)
    {
        float healthToReturn = 0;
        switch(changeHealthState)
        {
            case ChangeHealth.Default_Damage:
                healthToReturn = kirbyValueParams.kirbyDamageRegular;
                break;
            case ChangeHealth.Big_Damage:
                healthToReturn = kirbyValueParams.kirbyDamageRegular * 2;
                break;
            case ChangeHealth.Small_Heal:
                break;
            case ChangeHealth.Medium_Heal:
                break;
            case ChangeHealth.Full_Heal:
                break;


        }
        return healthToReturn;

    }
}
