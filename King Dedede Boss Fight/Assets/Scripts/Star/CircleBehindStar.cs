using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleBehindStar : MonoBehaviour
{
    Coroutine blinkEffect;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        StartCoroutine(BlinkEffect());
    }

    IEnumerator BlinkEffect()
    {
        while (true)
        {
            spriteRenderer.color = new Color(1, 1, 1, 1);
            spriteRenderer.sortingOrder = 1;
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = new Color(1, 1, 1, 0);
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = new Color(1, 1, 1, 1);
            spriteRenderer.sortingOrder = 3;
            yield return new WaitForSeconds(0.1f);
        }
        

    }
}
