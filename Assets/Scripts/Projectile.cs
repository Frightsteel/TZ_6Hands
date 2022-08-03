using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Space]
    [SerializeField] private Rigidbody rb;
    [Space]
    [SerializeField] private float speed;

    private void Start()
    {
        rb.velocity = transform.forward * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);//temp;, then pool
    }
}
