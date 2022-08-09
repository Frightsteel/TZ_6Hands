using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerStats : MonoBehaviour, IDamageable
{
    [Space]
    [SerializeField] private Animator _animator;
    [Space]
    [SerializeField] private float hp;
    [SerializeField] private float mana;
    //[SerializeField] private float criticalChance;
    //[SerializeField] private float dodgeChance;

    // animations IDs
    private int _animIDDeath;

    private PhotonView view;

    private void Start()
    {
        view = GetComponent<PhotonView>();
        AssignAnimationIDs();
    }

    private void AssignAnimationIDs()
    {
        _animIDDeath = Animator.StringToHash("Death");
    }

    public void TakeDamage(float damage)
    {
        view.RPC("RPC_TakeDamage", RpcTarget.All, damage);
    }

    [PunRPC]
    public void RPC_TakeDamage(float damage)
    {
        hp -= damage;
        Debug.Log($"HP - {hp}");
        if (hp <= 0)
        {
            _animator.SetTrigger(_animIDDeath);
            Debug.Log("DEAD");
        }
    }
}
