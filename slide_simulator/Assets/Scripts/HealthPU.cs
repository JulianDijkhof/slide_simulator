using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPU : MonoBehaviour
{

    private PlayerController controller;
    public float expirationTime = 20f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroyAfterTime());
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            ActivatePowerUp(collider.gameObject);
            Destroy(gameObject);
        }
    }

    private IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(expirationTime);
        Destroy(gameObject);
    }

    void ActivatePowerUp(GameObject player)
    {
        if (player.GetComponent<PlayerController>().health <=3)
        {
            player.GetComponent<PlayerController>().health += 1;
        }
    }
}