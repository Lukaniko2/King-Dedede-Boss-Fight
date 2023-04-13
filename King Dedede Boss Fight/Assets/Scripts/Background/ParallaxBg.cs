using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBg : MonoBehaviour
{
    /// <summary>
    /// Parallax effect for the Background Elements
    /// Check to see camera's position in world space
    /// Move the object based off the camera offset from its original position
    /// </summary>


    [SerializeField] private float moveSpeed;
    private Vector2 originalPositions;

    private void Start()
    {
        originalPositions = transform.position;
    }

    private void Update()
    {
        Vector2 camDistToWorldOrigin = Vector2.zero - (Vector2)Camera.main.transform.localPosition;

        transform.position = originalPositions + (camDistToWorldOrigin * moveSpeed);
    }
}
