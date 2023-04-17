using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateStar : MonoBehaviour
{
    public float rotateSpeed;

    void Update()
    {
        transform.rotation = Quaternion.Euler(0, rotateSpeed * Time.deltaTime, 0);
    }
}
