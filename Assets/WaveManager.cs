using System.Collections;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public EnemySpawner enemySpawner; // Reference to the EnemySpawner
    public int waveNumber = 1; // Current wave number
    public float timeBetweenWaves = 5f; // Delay between waves

    void Start()
    {
        StartCoroutine(SpawnWaves());
    }

    IEnumerator SpawnWaves()
    {
        while (true) // Infinite wave loop
        {
            Debug.Log($"Starting wave {waveNumber}");

            // Spawn a number of enemies equal to the wave number
            for (int i = 0; i < waveNumber; i++)
            {
                enemySpawner.SpawnEnemy(waveNumber); // Call the spawner to spawn an enemy
                yield return new WaitForSeconds(1f); // Delay between spawns within a wave
            }

            waveNumber++; // Increment the wave number
            Debug.Log($"Wave {waveNumber} completed! Next wave in {timeBetweenWaves} seconds.");

            yield return new WaitForSeconds(timeBetweenWaves); // Delay between waves
        }
    }
}
