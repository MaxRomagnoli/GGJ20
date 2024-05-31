using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemToFix : MonoBehaviour
{
    [SerializeField] float secondToBeFixed = 3f;
    [SerializeField] Rigidbody2D rb;
    bool isTouching = false;
    bool isWinning = false;
    float secondPassed = 0;

    void Update()
    {
        if(rb != null)
        {
            isTouching = rb.angularVelocity < 10f;
        }
        
        if(isTouching)
        {
            if(secondPassed > secondToBeFixed)
            {
                isWinning = true;
            } else
            {
                secondPassed += Time.deltaTime;
            }
        } else
        {
            isWinning = false;
            secondPassed = 0;
        }
    }

    public bool Win()
    {
        return isWinning;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        isTouching = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        isTouching = false;
    }
}
