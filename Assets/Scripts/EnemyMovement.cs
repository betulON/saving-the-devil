using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] float damage = 1f;
    [SerializeField] float damageDistance = 10f;
    [SerializeField] float minDistanceToFollow = 0.2f;
    [SerializeField] float maxDistanceToFollow = 15f;

    Rigidbody2D myRigidbody;
    PlayerMovement player;
    Animator myAnimator;

    //pathfinder
    EnemySpawner enemySpawner;
    WaveConfigSO waveConfig;
    List<Transform> waypoints;
    int waypointIndex = 0;
    bool reverse = false;


    void Awake()
    {
        enemySpawner = FindObjectOfType<EnemySpawner>();
    }

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<PlayerMovement>();
        myAnimator = GetComponent<Animator>();

        waveConfig = enemySpawner.GetWaveConfig();
        waypoints = waveConfig.GetWaypoints();
        transform.position = waypoints[waypointIndex].position;
    }

    void Update()
    {
        Walk();
        MakeDamage();
    }

    void Walk()
    {
        float distance = PlayerDistance();
        if (distance < maxDistanceToFollow)
        {
            MoveForwardToPlayer();
        }
        else
        {
            myRigidbody.velocity = new Vector2(0f, 0f);
            MoveBetweenWaypoints();
        }
    }

    void FlipSprite()
    {
        transform.localScale = new Vector2((-1) * Mathf.Sign(myRigidbody.velocity.x), 1f);
    }

    float PlayerHorizontalDistance()
    {
        return Mathf.Abs(player.transform.position.x - transform.position.x);
    }
    float PlayerDistance()
    {
        return Vector3.Distance(player.transform.position, transform.position);
    }

    void MakeDamage()
    {
        if (PlayerDistance() <= damageDistance)
        {
            player.ReducePlayerHealth(damage / PlayerDistance());
        }
    }

    void MoveForwardToPlayer()
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

    void MoveBetweenWaypoints()
    {
        FollowPath();
    }

    void FollowPath()
    {
        if (reverse)
        {
            if (waypointIndex >= 0)
            {
                Vector3 targetPosition = waypoints[waypointIndex].position;
                float delta = waveConfig.GetMovementSpeed() * Time.deltaTime;
                transform.position = Vector2.MoveTowards(transform.position, targetPosition, delta);
                FlipSprite(targetPosition);
                if (transform.position == targetPosition)
                {
                    waypointIndex--;
                }
            }
            else
            {
                reverse = false;
                waypointIndex++;
            }
        }
        else
        {
            if (waypointIndex < waypoints.Count)
            {
                Vector3 targetPosition = waypoints[waypointIndex].position;
                float delta = waveConfig.GetMovementSpeed() * Time.deltaTime;
                transform.position = Vector2.MoveTowards(transform.position, targetPosition, delta);
                FlipSprite(targetPosition);
                if (transform.position == targetPosition)
                {
                    waypointIndex++;
                }
            }
            else
            {
                reverse = true;
                waypointIndex--;
            }

        }
    }

    void FlipSprite(Vector3 targetPosition)
    {
        transform.localScale = new Vector2(Mathf.Sign(transform.position.x - targetPosition.x), 1f);
    }
}
