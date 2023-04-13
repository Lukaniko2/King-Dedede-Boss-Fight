using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool gameEnded = false;
    public static bool bossIsDefeated = false;

    private void Start()
    {
        AudioManager.Instance.PlaySound("bgm_kingDededeMusic");
    }
}
