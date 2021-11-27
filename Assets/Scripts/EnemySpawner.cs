using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] MoveConfigSO moveConfig;

    void Start()
    {
        SpawnEnemies();
    }

    public MoveConfigSO GetMoveConfig()
    {
        return moveConfig;
    }

    void SpawnEnemies()
    {
        Instantiate(moveConfig.GetEnemyPrefab(0),
                    moveConfig.GetStartingWaypoint().position,
                    Quaternion.identity,
                    transform);
    }

}
