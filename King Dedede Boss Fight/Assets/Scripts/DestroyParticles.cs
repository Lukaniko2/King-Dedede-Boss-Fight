using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParticles : MonoBehaviour
{
    [SerializeField] private float timeToDestroy;
    void Start()
    {
        Invoke("DestroyObject", timeToDestroy);
    }

    private void DestroyObject()
    {
        Destroy(gameObject);
    }
}
