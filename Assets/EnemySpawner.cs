using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs;  // Array of enemy prefabs
    public Transform spawnPoint;       // Enemy spawn location

    // No longer need timer variables since WaveManager handles timing

    public void SpawnEnemy(int waveNumber)
    {
        if (enemyPrefabs == null || enemyPrefabs.Length == 0)
        {
            Debug.LogError("Enemy Prefabs array is empty!");
            return;
        }

        GameObject enemyToSpawn = SelectEnemyPrefab(waveNumber);

        if (enemyToSpawn == null)
        {
            Debug.LogError("No enemy prefab selected for spawning.");
            return;
        }

        Instantiate(enemyToSpawn, spawnPoint.position, Quaternion.identity);
    }

    private GameObject SelectEnemyPrefab(int waveNumber)
    {
        if (waveNumber <= 3)
        {
            // Spawn only the basic enemy
            return enemyPrefabs[0];
        }
        else if (waveNumber <= 6)
        {
            // Randomly select between basic and stronger enemy
            int randomIndex = Random.Range(0, 2); // 0 or 1
            return enemyPrefabs[randomIndex];
        }
        else
        {
            // Randomly select from all enemy types
            int randomIndex = Random.Range(0, enemyPrefabs.Length);
            return enemyPrefabs[randomIndex];
        }
    }
}
