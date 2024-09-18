using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoublePointsPU : MonoBehaviour
{

    private GameManager gameManager;
    public float expirationTime = 20f;
    public float duration = 10f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroyAfterTime());
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            gameManager.DoublePoints(duration);
            Destroy(gameObject);
        }
    }

    private IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(expirationTime);
        Destroy(gameObject);
    }

}
