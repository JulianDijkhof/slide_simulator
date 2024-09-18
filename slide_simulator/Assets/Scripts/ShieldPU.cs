using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPU : MonoBehaviour
{
    private GameManager controller;
    public float duration = 4f;
    public float expirationTime = 20f;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<GameManager>();
        StartCoroutine(DestroyAfterTime());
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            controller.ActivateShield(duration);
        }
    }

    private IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(expirationTime);
        Destroy(gameObject);
    }
}
