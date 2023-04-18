using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateStar : MonoBehaviour
{
    //Variables
    [SerializeField] float rotateSpeed;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float travelTime;
    [HideInInspector] public int direction;
    [HideInInspector] public Vector2 position;
    private float currentTravelTime;

    [Header("Dicipate Time")]
    [SerializeField] private float decreaseScaleSpeed;
    [SerializeField] private float maxTimeLast;
    private float currentTimeLast;

    private void Update()
    {
        //Rotate the star always
        transform.Rotate(Vector3.forward, rotateSpeed);

        //increase time. Stops moving star once greater than max time
        currentTravelTime += Time.deltaTime;

        if (currentTravelTime < travelTime)
        {
            //moving the star outwards
            Vector2 pos = transform.position;
            pos.x += direction * moveSpeed * Time.deltaTime;
            transform.position = pos;
        }


        CheckDestroyTime();


    }

    private void CheckDestroyTime()
    {
        currentTimeLast += Time.deltaTime;

        if (currentTimeLast < maxTimeLast)
            return;

        //once the time is up, shrink the star until it vanishes
        transform.localScale -= Vector3.one * decreaseScaleSpeed * Time.deltaTime;

        Vector2 scale = transform.localScale;

        if (scale.y <= 0)
            Destroy(gameObject);
    }
}
