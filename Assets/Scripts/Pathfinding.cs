using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Not using 11/28/2021
public class Pathfinding : MonoBehaviour
{
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
        waveConfig = enemySpawner.GetWaveConfig();
        waypoints = waveConfig.GetWaypoints();
        transform.position = waypoints[waypointIndex].position;
    }

    void Update()
    {
        //FollowPath();
    }

    public void FollowPath()
    {
        Debug.Log("waypoint index, reverse");
        Debug.Log(waypointIndex);
        Debug.Log(reverse);
        if (!InBetweenWaypoints())
        {
            Debug.Log("inbetween not");
            RestartPath();
        }
        if (reverse)
        {
            if (waypointIndex >= 0)
            {
                Debug.Log("waypoint index, reverse");
                Debug.Log(waypointIndex);
                Debug.Log(reverse);

                Vector3 targetPosition = waypoints[waypointIndex].position;
                float delta = waveConfig.GetMovementSpeed() * Time.deltaTime;
                transform.position = Vector2.MoveTowards(transform.position, targetPosition, delta);
                FlipSprite(targetPosition);
                if (transform.position == targetPosition)
                {
                    waypointIndex--;
                }
                Debug.Log("transform position;");
                Debug.Log(transform.position);
                Debug.Log("target position;");
                Debug.Log(targetPosition);
                Debug.Log("delta");
                Debug.Log(delta);
                Debug.Log("waveConfig");
                Debug.Log(waveConfig);
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

                Debug.Log("transform position;");
                Debug.Log(transform.position);
                Debug.Log("target position;");
                Debug.Log(targetPosition);
                Debug.Log("delta");
                Debug.Log(delta);
                Debug.Log("waveConfig");
                Debug.Log(waveConfig);
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

    void RestartPath()
    {
        waypointIndex = 0;
        reverse = true;
    }

    bool InBetweenWaypoints()
    {
        return ((transform.position.x <= waypoints[0].transform.position.x) && (transform.position.x > waypoints[waypoints.Count - 1].transform.position.x));
    }

}
