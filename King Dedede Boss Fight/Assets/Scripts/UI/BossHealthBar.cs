using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealthBar : MonoBehaviour
{
    //References
    private UIManager uiManager;

    float bossBarScaleX;
    [SerializeField] float blueBarIncreaseSpeed;

    private void Awake()
    {
        uiManager = GameObject.FindObjectOfType<UIManager>();
    }

    private void Start()
    {
        AudioManager.Instance.PlaySound("ui_bossBarIncrease");
    }

    private void Update()
    {
        if (!uiManager.FinishedBossIntro)
            IncreaseBossBar();
    }

    private void IncreaseBossBar()
    {
        //Increase the blue boss bar until full. (Mini animation that plays at beginning)
        bossBarScaleX = transform.localScale.x;
        bossBarScaleX += blueBarIncreaseSpeed * Time.deltaTime;

        transform.localScale = new Vector3(bossBarScaleX, transform.localScale.y, 0);

        //when the boss bar loads up all the way to being full, then commence the battle and keep it's scale at max
        if (bossBarScaleX >= 0.3836945f)
        {
            uiManager.FinishedBossIntro = true;

            bossBarScaleX = 0.3836945f;

            AudioManager.Instance.StopSound("ui_bossBarIncrease");
        }
    }

    public void BossDamage(float bossHealthDecrement)
    {
        AudioManager.Instance.PlaySound("ui_bossHurt");

        //Decrement boss health when hit. Called from the UIManager Class
        transform.localScale -= new Vector3(bossBarScaleX - bossHealthDecrement, 0, 0);

        bool bossIsDefeated = transform.localScale.x <= 0;
        if (bossIsDefeated)
        {
            GameManager.bossIsDefeated = true;
            AudioManager.Instance.PlaySound("ui_bossDefeated");

            gameObject.SetActive(false);
            
        }
    }

}
