using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Projectile : MonoBehaviour
{
    [Space]
    [SerializeField] private float speed;
    [Space]
    public Transform spawnPoint;
    public int castUser;
    public float damage;
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
        if (other.GetComponent<PhotonView>() != null)
        {
            if (other.GetComponent<PhotonView>().ViewID != castUser)
            {
                other.GetComponent<IDamageable>().TakeDamage(damage);
                gameObject.SetActive(false);
                isCasted = false;
            }
        }
        else
        {
            gameObject.SetActive(false);
            isCasted = false;
        }
    }
}
