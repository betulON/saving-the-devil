using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] List<WaveConfigSO> waveConfigs;
    [SerializeField] float timeBetweenWaves;
    WaveConfigSO currentWave;

    void Start()
    {
        StartCoroutine(CreateWaves());
    }

    public WaveConfigSO GetWaveConfig()
    {
        return currentWave;
    }

    IEnumerator SpawnEnemies()
    {
        for (int i = 0; i < currentWave.GetEnemyCount(); i++)
        {
            Instantiate(currentWave.GetEnemyPrefab(i),
                    currentWave.GetStartingWaypoint().position,
                    Quaternion.identity,
                    transform);

            yield return new WaitForSeconds(currentWave.GetRandomSpwanTime());
        } 
    }

    IEnumerator CreateWaves()
    {
        foreach (WaveConfigSO waveConfig in waveConfigs)
        {
            currentWave = waveConfig;
            StartCoroutine(SpawnEnemies());
        }
        yield return new WaitForSeconds(timeBetweenWaves);

    }

}
