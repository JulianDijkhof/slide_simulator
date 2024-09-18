using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class RapidFirePU : MonoBehaviour
{

    private PlayerController controller;
    public float expirationTime = 20f;
    public float effectDuration = 4f;
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

    void ActivatePowerUp(GameObject player)
    {
        player.GetComponent<PlayerController>().FireSpeedPu(effectDuration);
    }

    private IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(expirationTime);
        Destroy(gameObject);
    }
}
