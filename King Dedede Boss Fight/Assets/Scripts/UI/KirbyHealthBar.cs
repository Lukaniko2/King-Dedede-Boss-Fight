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
    public void KirbyDamage(float kirbyDamageDecrement)
    {
        //If Invincible, don't take any damage
        bool isInvincible = Time.time - currentInvincibilityTime <= kirbyParams.maxInvinibilityTimer;
        if (isInvincible)
            return;

        AudioManager.Instance.PlaySound("k_hurt");

        //reduce health by certain threshold
        kirbyRectTransform.localScale -= new Vector3(kirbyDamageDecrement / IsShielding(), 0, 0);

        //if Kirby's health bar is less than 0, then the player loses
        if (transform.localScale.x <= 0)
        {
            anim.SendMessage("Outro", "Outro3");
            kirbyRectTransform.localScale = new Vector3(0, kirbyRectTransform.localScale.y, kirbyRectTransform.localScale.z);
        }

        //Set them invincible for a certain period of time
        currentInvincibilityTime = Time.time;

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
