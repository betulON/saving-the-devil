using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] float damage = 10f;
    [SerializeField] float minDistanceToFollow = 0.2f;
    [SerializeField] float maxDistanceToFollow = 15f;

    Rigidbody2D myRigidbody;
    PlayerMovement player;
    Animator myAnimator;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<PlayerMovement>();
        myAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        Walk();
        MakeDamage();
    }

    void Walk()
    {
        float horizontalDistance = PlayerHorizontalDistance();
        float distanceFactor = 3f / horizontalDistance;
        if (horizontalDistance < minDistanceToFollow)
        {
            myRigidbody.velocity = new Vector2(0f, 0f);
            return;
        }
        if (horizontalDistance < maxDistanceToFollow)
        {
            moveSpeed = moveSpeed * Mathf.Sign(player.transform.position.x - transform.position.x) * Mathf.Sign(moveSpeed);
        }
        distanceFactor = Mathf.Sign(moveSpeed) * distanceFactor;
        myRigidbody.velocity = new Vector2(moveSpeed + distanceFactor, 0f);
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

    void MakeDamage()
    {
        PlayerMovement player = FindObjectOfType<PlayerMovement>();
        if (player != null)
        {            
            player.GetComponent<Health>().ReduceHealth(damage / PlayerHorizontalDistance());
        }      
    }
}
