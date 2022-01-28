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
    [SerializeField] float runSpeed = 10f;
    [SerializeField] float jumpSpeed = 30f;
    [SerializeField] float slidingSpeed = 40f;
    // [SerializeField] float climbSpeed = 1f;
    [SerializeField] float maxHealth = 100f;
    [SerializeField] float maxMana = 50f;
    [SerializeField] float ghostMana = 2f;
    

    [SerializeField] GameObject arrow;
    [SerializeField] Transform bow;
    [SerializeField] HealthbarBehavior healthbar;

    AudioPlayer audioPlayer;

    Animator myAnimator;
    bool isAlive = true;
    float health;

    bool isGhost;
    bool isRunning;
    bool isChained;
    bool isHanging;
    bool isSliding;
    Vector3 chainArrowPos;
    float ghostInput;
    float currentMana;


    void Awake()
    {
        audioPlayer = FindObjectOfType<AudioPlayer>();
    }

    void Start()
    {
        isChained = false;
        isHanging = false;
        isSliding = false;
        isGhost = false;

        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myCapsuleCollider = GetComponent<CapsuleCollider2D>();
        health = maxHealth;
        currentMana = maxMana;
        healthbar.SetHealth(health, maxHealth);
        healthbar.SetMana(currentMana, maxMana);

    }

    void Update()
    {
        if (!isAlive) { return; }
        if (isHanging) { myRigidbody.gravityScale = 0; }
        else { myRigidbody.gravityScale = 10; }
        if (isSliding)
        {
            MoveToArrow();
            return;
        }
        Walk();
        FlipSprite();
        Die();
        UpdateHealth();
        Ghost();
        UpdateMana();
        //Climb();
    }

    void OnMove(InputValue value)
    { 
        if (!isAlive) { return; }
        moveInput = value.Get<Vector2>();
    }
    void OnJump(InputValue value)
    {
        if (!isAlive || isHanging || isChained) { return; }
        bool isTouchingGround = myCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));
        if (value.isPressed && isTouchingGround)
        {
            myRigidbody.velocity += new Vector2(0f, jumpSpeed);
        }
    }

// Left mouse button to fire an arrow
// If the player is chained pushing left mouse button begins sliding
// If the player is sliding pushing left mouse button releases the player
    void OnFire(InputValue value)
    {
        if (isGhost || isHanging) { return; }
        if (!isAlive || myAnimator.GetCurrentAnimatorStateInfo(0).IsName("PShooting") ) { return; }
        if (value.isPressed)
        {
            if (isChained && !isHanging)
            {
                isSliding = true;
            }
            else
            {
                // Shooting arrow
                // Mouse rotation
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
                Vector3 targetDirection = mousePos - transform.position;
                targetDirection.Normalize();
                float rotation_z = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
                Quaternion rotationArrow = Quaternion.Euler(0f, 0f, rotation_z);

                transform.localScale = new Vector2(Mathf.Sign(targetDirection.x), 1f);
                myAnimator.SetTrigger("Shooting");
                Instantiate(arrow, bow.position, rotationArrow);

                audioPlayer.PlayShootingClip();
            }
            
        }
    }

    void OnRelease(InputValue value)
    {
        if (isHanging || isChained)
        {
            isHanging = false;
            isChained = false;
            isSliding = false;
        }
    }

    void OnGhost(InputValue value)
    {
        isGhost = Mathf.Abs(value.Get<float>()) > Mathf.Epsilon && currentMana >= ghostMana;
    }

    void OnRun(InputValue value)
    { 
        isRunning = Mathf.Abs(value.Get<float>()) > Mathf.Epsilon;
    }

    void Ghost()
    {
        if (isGhost)
        {
            ReducePlayerMana(ghostMana);
        }
    }

    void Walk()
    {
        float speed = 0f;
        if (isRunning)
        {
            speed = runSpeed;
        } else
        {
            speed = walkSpeed;
        }

        Vector2 playerVelocity = new Vector2(moveInput.x * speed, myRigidbody.velocity.y);
        myRigidbody.velocity = playerVelocity;

        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("isWalking", playerHasHorizontalSpeed);
        
    }
    // void Climb()
    // {
    //     if (!myCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Climbing"))) { return; }
    //     Vector2 playerVelocity = new Vector2(myRigidbody.velocity.x, moveInput.y * climbSpeed);
    //     myRigidbody.velocity = playerVelocity;

    //     //bool playerHasVerticalSpeed = Mathf.Abs(myRigidbody.velocity.y) > Mathf.Epsilon;
    //    // myAnimator.SetBool("isClimbing", playerHasVerticalSpeed);
        
    // }

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
        if (isGhost) { return; }
        health -= damage;

        audioPlayer.PlayHurtingClip();
    }

    void UpdateHealth()
    {
        healthbar.SetHealth(health, maxHealth);
    }

    void ReducePlayerMana(float value)
    {
        currentMana -= value;
    }

    void UpdateMana()
    {
        healthbar.SetMana(currentMana, maxMana);
    }

    public bool GetVisibility()
    {
        return !isGhost;
    }

    public void SetChain(bool chain, Vector2 pos)
    {
        isChained = chain;
        chainArrowPos = pos;
    }

    void ReleaseChain() 
    {
        isHanging = false;
        isChained = false;
    }

    void MoveToArrow()
    {
        myRigidbody.gravityScale = 0;        

        Vector3 targetPos = chainArrowPos;
        targetPos.y = targetPos.y - 2f;

        Vector3 direction = targetPos - transform.position;
        float distance = Vector3.Distance(targetPos, transform.position);

        if (distance < Mathf.Epsilon)
        {
            isSliding = false;
            isHanging = true;
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, slidingSpeed * Time.fixedDeltaTime);
        }
        

    }

}
