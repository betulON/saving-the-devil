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

    // Update is called once per frame
    void Update()
    {
        Walk();
        FlipSprite();
    }

    void Walk()
    {
        float distance = PlayerDistance();
        if (distance < 10f)
        {
            moveSpeed = moveSpeed * Mathf.Sign(player.transform.position.x - transform.position.x) * Mathf.Sign(moveSpeed);
        }
        
        myRigidbody.velocity = new Vector2(moveSpeed, 0f);
    }

    void FlipSprite()
    {
        transform.localScale = new Vector2((-1) * Mathf.Sign(myRigidbody.velocity.x), 1f);
    }

    float PlayerDistance()
    {
        return Vector2.Distance(player.transform.position, transform.position);
    }
}
