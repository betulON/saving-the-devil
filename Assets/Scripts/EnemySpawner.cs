using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] MoveConfigSO moveConfig;

    void Start()
    {
        StartCourutine(SpawnEnemies());
    }

    public MoveConfigSO GetMoveConfig()
    {
        return moveConfig;
    }

    IEnumerator SpawnEnemies()
    {
        for (int i = 0; i < moveConfig.GetEnemyCount(); i++)
        {
            Instantiate(moveConfig.GetEnemyPrefab(i),
                    moveConfig.GetStartingWaypoint().position,
                    Quaternion.identity,
                    transform);

            yield return new WaitForSeconds(moveConfig.GetRandomSpwanTime());
        } 
    }

}
