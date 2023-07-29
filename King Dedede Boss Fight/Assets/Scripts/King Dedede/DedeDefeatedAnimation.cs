using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DedeDefeatedAnimation : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private Vector3 moveDirection;

    private void Update()
    {
        //move in the upperleft direction
        transform.position += moveDirection * moveSpeed * Time.deltaTime;

        Invoke("DestroyObject", 5);
    }

    private void DestroyObject()
    {
        Destroy(gameObject);
    }
}
