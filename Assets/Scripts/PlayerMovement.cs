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
    [SerializeField] float flyingSpeed = 5f;
    Animator myAnimator;
    bool isAlive = true;
    
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myCapsuleCollider = GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAlive) { return; }
        Walk();
        FlipSprite();
        Die();
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
        if (value.isPressed && isTouchingGround)
        {
            myRigidbody.velocity += new Vector2(0f, jumpSpeed);
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
        if (myCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Enemies")))
        {
            isAlive = false;
            myAnimator.SetTrigger("Dying");
        }
    }
}
