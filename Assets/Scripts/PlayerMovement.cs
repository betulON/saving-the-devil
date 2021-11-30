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
    float ghostInput;
    float currentMana;

    void Awake()
    {
        audioPlayer = FindObjectOfType<AudioPlayer>();
    }


    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myCapsuleCollider = GetComponent<CapsuleCollider2D>();
        isGhost = false;
        health = maxHealth;
        currentMana = maxMana;
        healthbar.SetHealth(health, maxHealth);
        healthbar.SetMana(currentMana, maxMana);


        //    bowPlace = GetComponentInChildren<Transform>();
    }

    void Update()
    {
        if (!isAlive) { return; }
        Walk();
        FlipSprite();
        Die();
        UpdateHealth();
        Ghost();
        UpdateMana();
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
        if (isGhost) { return; }
        if (!isAlive || myAnimator.GetCurrentAnimatorStateInfo(0).IsName("PShooting") ) { return; }
        if (value.isPressed)
        {
            //Mouse rotation
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()); 
            Vector3 targetDirection = mousePos - transform.position;
            targetDirection.Normalize();
            float rotation_z = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
            Quaternion rotationArrow = Quaternion.Euler(0f, 0f, rotation_z);
            //Instantiate(arrow, bow.position, transform.rotation);

            transform.localScale = new Vector2(Mathf.Sign(targetDirection.x), 1f);
            myAnimator.SetTrigger("Shooting");
            Instantiate(arrow, bow.position, rotationArrow);

            audioPlayer.PlayShootingClip();
        }
    }

    void OnGhost(InputValue value)
    {
        isGhost = Mathf.Abs(value.Get<float>()) > Mathf.Epsilon && currentMana >= ghostMana;
    }

    void Ghost()
    {
        if (isGhost)
        {
            //myRigidbody.mass = 0.1f;
            ReducePlayerMana(ghostMana);
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

}
