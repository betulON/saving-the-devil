using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Vector2 moveInput;
    Rigidbody2D myRigidbody;
    CapsuleCollider2D myCapsuleCollider;
    [SerializeField] float walkSpeed = 5f;
    [SerializeField] float jumpSpeed = 30f;
    [SerializeField] float maxHealth = 100f;

    [SerializeField] GameObject arrow;
    [SerializeField] Transform bow;
    [SerializeField] HealthbarBehavior healthbar;

    Animator myAnimator;
    bool isAlive = true;
    float health;
    
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myCapsuleCollider = GetComponent<CapsuleCollider2D>();
        health = maxHealth;
        healthbar.SetHealth(health, maxHealth);
        

    //    bowPlace = GetComponentInChildren<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAlive) { return; }
        Walk();
        FlipSprite();
        Die();
        UpdateHealth();
    }

    void OnMove(InputValue value)
    { 
        if (!isAlive) { return; }
        moveInput = value.Get<Vector2>();
    }
    void OnJump(InputValue value)
    {
        if (!isAlive) { return; }
        bool isTouchingGround = myCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));
        if (value.isPressed)
        {
            myRigidbody.velocity += new Vector2(0f, jumpSpeed);
        }
    }

    void OnFire(InputValue value)
    {
        if (!isAlive || myAnimator.GetCurrentAnimatorStateInfo(0).IsName("PShooting") ) { return; }
        if (value.isPressed)
        {
            myAnimator.SetTrigger("Shooting");
            Instantiate(arrow, bow.position, transform.rotation);
        }
    }

    void Walk()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * walkSpeed, myRigidbody.velocity.y);
        myRigidbody.velocity = playerVelocity;

        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("isWalking", playerHasHorizontalSpeed);
        
    }

    void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;

        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidbody.velocity.x), 1f);
        }
    }
    void Die()
    {
        if (myCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Enemies")) || health <= 0f)
        {
            isAlive = false;
            health = 0f;
            healthbar.SetHealth(health, maxHealth);
            myAnimator.SetTrigger("Dying");
            myRigidbody.gravityScale = 0;
            myRigidbody.velocity = new Vector2(0f, 0f);
            FindObjectOfType<GameSession>().ProcessPlayerDeath();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Exit")
        {
            FindObjectOfType<GameSession>().ProcessExit();
        }
    }

    public void ReducePlayerHealth(float damage)
    {
        health -= damage;
    }

    void UpdateHealth()
    {
        healthbar.SetHealth(health, maxHealth);
    }

}
