using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ChainArrow : MonoBehaviour
{
    Rigidbody2D myRigidbody;
    [SerializeField] float arrowSpeed = 15f;
    [SerializeField] float arrowDamage = 15f;
    [SerializeField] float arrowLifeTime = 0.1f;
    PlayerMovement player;
    float xSpeed;
    bool isStuck;

    Vector3 mousePos;
    Vector3 direction;

    void Awake()
    {
        Destroy(gameObject, arrowLifeTime);
    }

    void Start()
    {
        isStuck = false;

        myRigidbody = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<PlayerMovement>();
        xSpeed = player.transform.localScale.x * arrowSpeed;

        mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        direction = mousePos - transform.position;
        direction.z = 0;
        direction.Normalize();
    }

    void Update()
    {
        //myRigidbody.velocity = new Vector2(xSpeed, 0f);
        if (isStuck) { return; }
        transform.position += direction * arrowSpeed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("in collision");
        if (collision.tag == "Enemy")
        {
            collision.GetComponent<Health>().ReduceHealth(arrowDamage);
            // Destroy(collision.gameObject);
            Destroy(gameObject);
        }
        else if (collision.tag == "Stuck")
        {
            Debug.Log("stucked");
            player.setChain(true);
            isStuck = true;
            //player.transform.position = collision.transform.position;
        }

        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }

    public bool getStuck()
    {
        return isStuck;
    }
}
