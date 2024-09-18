using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float moveSpeed;

    private Transform playerTransform;
    private Rigidbody rb;

    public GameObject gameManager;

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody>();

        gameManager = GameObject.FindGameObjectWithTag("GameController");
    }

    void Update()
    {
        moveSpeed = gameManager.GetComponent<GameManager>().currentEnemySpeed;
        MoveTowardsPlayer();
    }

    void MoveTowardsPlayer()
    {
        if (playerTransform != null)
        {
            Vector3 direction = (playerTransform.position - transform.position).normalized;
            rb.velocity = direction * moveSpeed;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Deal damage to the player
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.dealDmg();  // Call the dealDmg function
            }

            // Destroy the enemy
            KillEnemy();
        }
    }

    public void KillEnemy()
    {
        gameManager.GetComponent<GameManager>().killCount += 1;
        gameManager.GetComponent<GameManager>().AddKillToScore();
        RollForPU();
        Destroy(gameObject);
    }

    void RollForPU ()
    {
        float random = Random.Range(0, 100);
        if (random >= 95)
        {
            SpawnRandomPU();
        }
    }

    void SpawnRandomPU()
    {
        float random = Random.Range(0, 4);
        if (random == 0)
        {
            gameManager.GetComponent<GameManager>().SpawnDoublePointsPU();
        }
        if (random == 1)
        {
            gameManager.GetComponent<GameManager>().SpawnFreezePU();
        }
        if (random == 2)
        {
            gameManager.GetComponent<GameManager>().SpawnNukePU();
        }
        if (random == 3)
        {
            gameManager.GetComponent<GameManager>().SpawnShieldPU();
        }
    }
}
