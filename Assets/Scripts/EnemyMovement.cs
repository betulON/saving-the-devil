using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    Rigidbody2D myRigidbody;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Walk();
        FlipSprite();
    }

    void Walk()
    {
        myRigidbody.velocity = new Vector2(moveSpeed, 0f);
    }

    void FlipSprite()
    {
        transform.localScale = new Vector2((-1) * Mathf.Sign(myRigidbody.velocity.x), 1f);
    }
}
