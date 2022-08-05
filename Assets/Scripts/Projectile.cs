using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Space]
    [SerializeField] private float speed;

    public Transform spawnPoint;

    public bool isCasted = false;

    private void Update()
    {
        if (!isCasted)
        {
            transform.position = spawnPoint.position;
        }
        if (isCasted)
        {
            transform.position += transform.forward * (speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != 3)
        {
            Debug.Log(other.gameObject.layer);
            Debug.Log("test1");
            Destroy(gameObject);//temp;, then pool
        }
    }
}
