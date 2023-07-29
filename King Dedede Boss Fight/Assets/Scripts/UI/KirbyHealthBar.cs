using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class KirbyHealthBar : MonoBehaviour
{
    //References
    [SerializeField] SO_KirbyValueParams kirbyParams;
    private SpriteRenderer kirbySr;
    private KirbyAnimationController anim;
    private KirbyMovement kirbyMov;
    private KirbyDetails kirbyDetails;

    private GameObject kirbyGameObject;
    private RectTransform kirbyRectTransform;

    //Variables
    private float damageDivider; //if Shielding, reduce damage, if not, divide by 1
    private float currentInvincibilityTime;
    private Color color = Color.white;

    private void Awake()
    {
        kirbyGameObject = GameObject.FindObjectOfType<PlayerInput>().gameObject;
        kirbyRectTransform = GetComponent<RectTransform>();

        kirbyDetails = kirbyGameObject.GetComponent<KirbyDetails>();
        anim = kirbyGameObject.GetComponent<KirbyAnimationController>();
        kirbySr = kirbyGameObject.GetComponent<SpriteRenderer>();
        kirbyMov = kirbyGameObject.gameObject.GetComponent<KirbyMovement>();

        currentInvincibilityTime = Time.time;

    }
    void Update()
    {
        bool isInvincible = Time.time - currentInvincibilityTime <= kirbyParams.maxInvinibilityTimer;
        if (isInvincible)
            InvokeRepeating("Invincible", 0, kirbyParams.flickerRate);

        else
        {
            CancelInvoke();
            color.a = 1;
            kirbySr.color = new Color(1, 1, 1, color.a);
        }
    }

    private void Invincible()
    {
        color.a++;
        color.a %= 2;
        kirbySr.color = new Color(1, 1, 1, color.a);
    }
    public void KirbyChangeHealth(float kirbyChangedHealth)
    {
        //Check to see if healing or damage is being passed
        bool isDamage = kirbyChangedHealth < 0;
        if(isDamage)
        {
            bool isInvincible = Time.time - currentInvincibilityTime <= kirbyParams.maxInvinibilityTimer;
            if (isInvincible)
                return;

            //Reduce Health Bar
            kirbyDetails.KirbyHealth += kirbyChangedHealth / IsShielding();
            kirbyRectTransform.sizeDelta += new Vector2(kirbyChangedHealth / IsShielding(), 0);


            bool lostAllHealth = kirbyDetails.KirbyHealth <= 0;
            if (lostAllHealth)
                anim.SendMessage("Outro", "LoseGameAnimation");

            //Set them invincible for a certain period of time
            currentInvincibilityTime = Time.time;
            AudioManager.Instance.PlaySound("k_hurt");

        }
        else
        {
            //Gain Health
            kirbyDetails.KirbyHealth += kirbyChangedHealth;
            kirbyRectTransform.sizeDelta -= new Vector2(kirbyChangedHealth, 0);
        }

        

        
        

    }

    private float IsShielding()
    {
        if (kirbyMov.isShielding)
            damageDivider = kirbyParams.kirbyDamageShieldingDivider;

        else
            damageDivider = 1;

        return damageDivider;
    }
}
