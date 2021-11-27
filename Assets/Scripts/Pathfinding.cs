using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    EnemySpawner enemySpawner;
    MoveConfigSO moveConfig;
    List<Transform> waypoints;
    int waypointIndex = 0;
    bool reverse = false;

    void Awake()
    {
        enemySpawner = FindObjectOfType<EnemySpawner>();
    }

    void Start()
    {
        moveConfig = enemySpawner.GetMoveConfig();
        waypoints = moveConfig.GetWaypoints();
        transform.position = waypoints[waypointIndex].position;
    }

    void Update()
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
                float delta = moveConfig.GetMovementSpeed() * Time.deltaTime;
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
                float delta = moveConfig.GetMovementSpeed() * Time.deltaTime;
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
