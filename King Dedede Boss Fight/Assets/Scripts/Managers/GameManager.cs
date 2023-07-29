using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static bool gameEnded = false;
    public static bool bossIsDefeated = false;


    //Scriptable Objects
    [SerializeField] private SO_BossDefeatedEventSender SObossDefeat;

    private void Awake()
    {

        SObossDefeat.bossIsDefeatedEvent.AddListener(IsDefeated);

    }

    private void Start()
    {
        gameEnded = false;
        AudioManager.Instance.PlaySound("bgm_bossMusic");

    }

    private void IsDefeated()
    {
        //Once we defeat the boss, we will do some stuff
        AudioManager.Instance.LowerMusicVolume();

        AudioManager.Instance.PlaySound("ui_bossDefeated");

        bossIsDefeated = true;

        //Flicker Screen
        SObossDefeat.FlickerScreenSend();

        AudioManager.Instance.PlaySound("d_scream");

        //the star and defeat animation is spawned in a class on the boss called 'BossCheckDefeat'
    }
}
