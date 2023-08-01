using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuffOut : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float maxTime;

    private float currentSpeed;
    private int directionOfTravel;

    //constructor
    public void PuffSetup (int direction)
    {
        directionOfTravel = direction;
        currentSpeed = moveSpeed;

        //flip sprite based on direction of travel
        Vector3 currentScale = transform.localScale;
        transform.localScale = new Vector3(currentScale.x * direction, currentScale.y, currentScale.z);
    }

    //move the puff in a certain direction and slow down it's speed
    private void Update()
    {
        currentSpeed = Mathf.Lerp(currentSpeed, 0, Time.deltaTime / maxTime);
        transform.position += new Vector3(currentSpeed * directionOfTravel * Time.deltaTime, 0);

        //when it's slowed down enough (maxTime), destroy it
        if (currentSpeed < 0.2f)
            Destroy(gameObject);
    }
}
