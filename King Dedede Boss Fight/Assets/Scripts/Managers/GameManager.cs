using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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

        //Play Boss Defeat Animation

        //Spawn Star
    }
}
