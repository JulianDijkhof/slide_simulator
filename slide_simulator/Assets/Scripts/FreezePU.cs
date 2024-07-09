using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezePU : MonoBehaviour
{
    private GameManager controller;
    public float duration = 8f;
    public float expirationTime = 5f;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<GameManager>();
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
            controller.ActivateFreeze(duration);
        }
    }

    private IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(expirationTime);
        Destroy(gameObject);
    }
}
