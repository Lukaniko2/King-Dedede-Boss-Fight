using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealthBar : MonoBehaviour
{
    //References
    private UIManager uiManager;
    private RectTransform bossRectTransform;

    //Variables
    [SerializeField] private float blueBarIncreaseSpeed;
    private float bossBarScaleX;
    

    private void Awake()
    {
        bossRectTransform = GetComponent<RectTransform>();
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
        bossBarScaleX = bossRectTransform.sizeDelta.x;
        bossBarScaleX += blueBarIncreaseSpeed * Time.deltaTime;

        bossRectTransform.sizeDelta = new Vector2(bossBarScaleX, bossRectTransform.sizeDelta.y);

        //when the boss bar loads up all the way to being full, then commence the battle and keep it's scale at max
        if (bossRectTransform.sizeDelta.x >= 100)
        {
            uiManager.FinishedBossIntro = true;

            AudioManager.Instance.StopSound("ui_bossBarIncrease");
        }
    }

    public void BossDamage(float bossHealthDecrement)
    {
        AudioManager.Instance.PlaySound("ui_bossHurt");

        //Decrement boss health when hit. Called from the UIManager Class
        bossRectTransform.sizeDelta -= new Vector2(bossHealthDecrement, 0);

        bool bossIsDefeated = bossRectTransform.sizeDelta.x <= 0;
        if (bossIsDefeated)
        {
            GameManager.bossIsDefeated = true;
            AudioManager.Instance.PlaySound("ui_bossDefeated");

            gameObject.SetActive(false);
            
        }
    }

}
