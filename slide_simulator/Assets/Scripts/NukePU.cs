using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NukePU : MonoBehaviour
{
    public float expirationTime = 5f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroyAfterTime());
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            Collider[] targets = Physics.OverlapSphere(transform.position, 2000000f);
            foreach (var target in targets)
            {
                if (target.CompareTag("Enemy"))
                {
                    target.GetComponent<EnemyController>().KillEnemy();
                }
            }
            Destroy(gameObject);
        }
    }

    private IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(expirationTime);
        Destroy(gameObject);
    }
}
