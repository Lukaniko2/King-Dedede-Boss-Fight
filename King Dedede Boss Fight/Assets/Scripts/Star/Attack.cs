using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    //BossAnimationManager bossAnim;
    [SerializeField] private SO_AdjustHealth adjustHealth;
    [SerializeField] float horizontalForce;
    [SerializeField] private GameObject hitParticles;

    //direction to move in
    private float travelDirection;
    public float TravelDirection
    {
        get => travelDirection;
        set => travelDirection = value;
    }

    private void Awake()
    {
        AudioManager.Instance.PlaySound("k_attack");

        Invoke("DestroyTheObject", 10);

        Physics2D.IgnoreLayerCollision(0,9);
    }

    void Update()
    {
        //move star in direction
        transform.Translate(horizontalForce * TravelDirection * Time.deltaTime, 0, 0);

        
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            return;
        if (collision.gameObject.CompareTag("Ground"))
            return;


        if(collision.gameObject.CompareTag("Enemy"))
        {
            //send to reduce scale of boss bar
            adjustHealth.changeBossHealthEvent.Invoke(ChangeHealth.Default_Damage);
        }

        Instantiate(hitParticles, transform.position + Vector3.right * TravelDirection * 0.2f, Quaternion.identity);
        DestroyTheObject();
    }

    private void DestroyTheObject()
    {
        Destroy(this.gameObject);
    }
}
