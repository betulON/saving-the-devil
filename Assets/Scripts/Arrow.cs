using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Arrow : MonoBehaviour
{
    Rigidbody2D myRigidbody;
    [SerializeField] float arrowSpeed = 15f;
    [SerializeField] float arrowDamage = 15f;
    [SerializeField] float arrowLifeTime = 0.1f;
    PlayerMovement player;
    float xSpeed;

    Vector3 mousePos;

    void Awake()
    {
        Destroy(gameObject, arrowLifeTime);
    }

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<PlayerMovement>();
        xSpeed = player.transform.localScale.x * arrowSpeed;

        mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
    }

    void Update()
    {
        //myRigidbody.velocity = new Vector2(xSpeed, 0f);
        
        Debug.Log("mousePos");
        Debug.Log(mousePos);
        float delta = 15f * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, mousePos, delta);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            collision.GetComponent<Health>().ReduceHealth(arrowDamage);
           // Destroy(collision.gameObject);
        }
        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
