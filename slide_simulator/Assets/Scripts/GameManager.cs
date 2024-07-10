using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

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
    public bool freezePowerActive = false;
    private int currentScore = 0;
    public bool doublePoints = false;

    public TMP_Text scoreText;
    public Image health1;
    public Image health2;
    public Image health3;

    public Image dmg1;
    public Image dmg2;
    public Image dmg3;

    public GameObject nukePU;
    public GameObject freezePU;
    public GameObject shieldPU;
    public GameObject doublePointsPU;

    public Image freezeImage;
    public Image shieldImage;
    public Image doublePointsImage;

    public PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        currentSpawnRate = initialSpawnRate;
        StartCoroutine(SpawnEnemies());
        currentEnemySpeed = startEnemySpeed;
        InvokeRepeating("AddTimeToScore", 0, 1);
        InvokeRepeating("SpawnNukePU", 180, 120);
        InvokeRepeating("SpawnFreezePU", 120, 60);
        InvokeRepeating("SpawnShieldPU", 180, 150);
        InvokeRepeating("SpawnDoublePointsPU", 60, 135);
        freezeImage.enabled = false;
        shieldImage.enabled = false;
        doublePointsImage.enabled = false;
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
            float enemySpeed = Mathf.Lerp(startEnemySpeed, targetEnemySpeed, normalizedTime);
            if (!freezePowerActive)
            {
                currentEnemySpeed = enemySpeed;
            }
        }

        currentTime = Time.time;

        scoreText.text = currentScore.ToString();

        if (playerController.health == 3)
        {
            health1.enabled = true;
            health2.enabled = true;
            health3.enabled = true;

            dmg1.enabled = false;
            dmg2.enabled = false;
            dmg3.enabled = false;
        }
        else if (playerController.health == 2)
        {
            health1.enabled = true;
            health2.enabled = true;
            health3.enabled = false;

            dmg1.enabled = false;
            dmg2.enabled = false;
            dmg3.enabled = true;
        }
        else if (playerController.health == 1)
        {
            health1.enabled = true;
            health2.enabled = false;
            health3.enabled = false;

            dmg1.enabled = false;
            dmg2.enabled = true;
            dmg3.enabled = true;
        }
        else if (playerController.health == 0)
        {
            health1.enabled = false;
            health2.enabled = false;
            health3.enabled = false;

            dmg1.enabled = true;
            dmg2.enabled = true;
            dmg3.enabled = true;
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
        CancelInvoke("AddTimeToScore");
    }

    IEnumerator SpawnEnemies()
    {
        while (true)
        {
            // Calculate the next spawn interval with some randomness
            float spawnInterval = currentSpawnRate + UnityEngine.Random.Range(-randomFactor, randomFactor);
            spawnInterval = Mathf.Max(0.1f, spawnInterval); // Ensure the interval is not too short

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

    public void ActivateFreeze(float duration)
    {
        freezePowerActive = true;
        freezeImage.enabled = true;
        currentEnemySpeed = 0;
        StartCoroutine(DeactivateFreezePU(duration));
    }

    public void AddKillToScore()
    {
        currentScore += 5;
        if (doublePoints)
        {
            currentScore += 10;
        }
    }

    public void AddTimeToScore()
    {
        currentScore += 1;
        if (doublePoints)
        {
            currentScore += 2;
        }
    }

    public void DoublePoints(float duration)
    {
        doublePoints = true;
        doublePointsImage.enabled = true;
        StartCoroutine(DeactivateDoublePointsPU(duration));
    }

    private IEnumerator DeactivateDoublePointsPU(float duration)
    {
        yield return new WaitForSeconds(duration);
        doublePoints = false;
        doublePointsImage.enabled = false;
    }

    private IEnumerator DeactivateFreezePU(float duration)
    {
        yield return new WaitForSeconds(duration);
        freezePowerActive = false;
        freezeImage.enabled = false;
    }

    private void SpawnNukePU()
    {
        Vector3 spawnLocation = new Vector3(
            UnityEngine.Random.Range(9f, 95f),   // X coordinate between 9 and 95
            1.5f,                               // Y coordinate fixed at 1.5
            UnityEngine.Random.Range(35f, 75f)  // Z coordinate between 35 and 75
        );

        Instantiate(nukePU, spawnLocation, Quaternion.identity);
    }

    private void SpawnFreezePU()
    {
        Vector3 spawnLocation = new Vector3(
            UnityEngine.Random.Range(9f, 95f),   // X coordinate between 9 and 95
            1.5f,                               // Y coordinate fixed at 1.5
            UnityEngine.Random.Range(35f, 75f)  // Z coordinate between 35 and 75
        );

        Instantiate(freezePU, spawnLocation, Quaternion.identity);
    }

    private void SpawnShieldPU()
    {
        Vector3 spawnLocation = new Vector3(
            UnityEngine.Random.Range(9f, 95f),   // X coordinate between 9 and 95
            1.5f,                               // Y coordinate fixed at 1.5
            UnityEngine.Random.Range(35f, 75f)  // Z coordinate between 35 and 75
        );

        Instantiate(shieldPU, spawnLocation, Quaternion.identity);
    }

    public void ActivateShield(float duration)
    {
        playerController.shieldActive = true;
        shieldImage.enabled = true;
        StartCoroutine(DeactivateShieldPU(duration));
    }

    private IEnumerator DeactivateShieldPU(float duration)
    {
        yield return new WaitForSeconds(duration);
        playerController.shieldActive = false;
        shieldImage.enabled = false;
    }

    private void SpawnDoublePointsPU()
    {
        Vector3 spawnLocation = new Vector3(
    UnityEngine.Random.Range(9f, 95f),   // X coordinate between 9 and 95
    1.5f,                               // Y coordinate fixed at 1.5
    UnityEngine.Random.Range(35f, 75f)  // Z coordinate between 35 and 75
);

        Instantiate(doublePointsPU, spawnLocation, Quaternion.identity);
    }
}
