using System;
using System.Collections;
using UnityEngine;

public class VelociityProjectile : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private float speed = 50f;

    [Header("Destroy")]
    [SerializeField] private bool destroyOnCollide = false;
    [SerializeField] private float destroyDelay = 0f;

    private Coroutine destroyCoroutine;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        if (rb != null) {
            rb.linearVelocity = transform.forward * speed;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (destroyOnCollide && destroyCoroutine == null) {
            destroyCoroutine = StartCoroutine(DestroyDelay());
        }
    }

    private IEnumerator DestroyDelay()
    {
        yield return new WaitForSeconds(destroyDelay);

        Destroy(gameObject);
    }
}