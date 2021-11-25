using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f;
    Rigidbody2D myRigidbody;
    PlayerMovement player;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<PlayerMovement>();
    }

    void Update()
    {
        Walk();
    }

    void Walk()
    {
        float horizontalDistance = PlayerHorizontalDistance();
        if (horizontalDistance < 2f)
        {
            myRigidbody.velocity = new Vector2(0f, 0f);
            return;
        }
        if (horizontalDistance < 10f)
        {
            moveSpeed = moveSpeed * Mathf.Sign(player.transform.position.x - transform.position.x) * Mathf.Sign(moveSpeed);
        }
        
        myRigidbody.velocity = new Vector2(moveSpeed, 0f);
        FlipSprite();
    }

    void FlipSprite()
    {
        transform.localScale = new Vector2((-1) * Mathf.Sign(myRigidbody.velocity.x), 1f);
    }

    float PlayerHorizontalDistance()
    {
        return Mathf.Abs(player.transform.position.x - transform.position.x);
    }
}
