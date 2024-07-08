using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Timer stuff
    public float elapsedTime;
    public bool timerRunning = false;

    public int hours;
    public int minutes;
    public int seconds;

    public float currentEnemySpeed;
    public float startEnemySpeed = 5f;
    public float targetEnemySpeed = 12f;

    public List<GameObject> spawnLocations = new List<GameObject>();
    public List<GameObject> enemies = new List<GameObject>();

    // Spawning variables
    public float initialSpawnRate = 5f; // Initial time between spawns in seconds
    public float randomFactor = 2f; // Maximum amount of random variance added to the spawn interval
    private float currentSpawnRate;

    public int killCount;

    // Start is called before the first frame update
    void Start()
    {
        currentSpawnRate = initialSpawnRate;
        StartCoroutine(SpawnEnemies());
        currentEnemySpeed = startEnemySpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (timerRunning)
        {
            elapsedTime += Time.deltaTime; // Accumulate time

            // Converting time into something human readable
            TimeSpan timeSpan = TimeSpan.FromSeconds(elapsedTime);
            hours = timeSpan.Hours;
            minutes = timeSpan.Minutes;
            seconds = timeSpan.Seconds;

            // Adjust the spawn rate based on the elapsed time
            currentSpawnRate = Mathf.Max(1f, initialSpawnRate - elapsedTime / 60); // Decrease the spawn rate over time, minimum 1 second

            float normalizedTime = Mathf.Clamp01(elapsedTime / (5 * 60f));
            currentEnemySpeed = Mathf.Lerp(startEnemySpeed, targetEnemySpeed, normalizedTime);
        }
    }

    void StartTimer()
    {
        timerRunning = true;
    }

    void StopTimer()
    {
        timerRunning = false;
    }

    public void EndGame()
    {
        StopTimer();
    }

    IEnumerator SpawnEnemies()
    {
        while (true)
        {
            // Calculate the next spawn interval with some randomness
            float spawnInterval = currentSpawnRate + UnityEngine.Random.Range(-randomFactor, randomFactor);
            spawnInterval = Mathf.Max(0.5f, spawnInterval); // Ensure the interval is not too short

            yield return new WaitForSeconds(spawnInterval);

            if (enemies.Count > 0 && spawnLocations.Count > 0)
            {
                // Select a random enemy and spawn location
                GameObject enemyToSpawn = enemies[UnityEngine.Random.Range(0, enemies.Count)];
                GameObject spawnLocation = spawnLocations[UnityEngine.Random.Range(0, spawnLocations.Count)];

                // Instantiate the enemy at the spawn location
                Instantiate(enemyToSpawn, spawnLocation.transform.position, Quaternion.identity);
            }
        }
    }

    // Calculates final score based on elapsed time and kills
    public int CalculateFinalScore()
    {
        int accumulatedTime = Mathf.RoundToInt(elapsedTime);
        int accumulatedKills = killCount * 5;
        int finalScore = accumulatedTime + accumulatedKills;

        return finalScore;
    }
}
