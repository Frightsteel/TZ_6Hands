using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour, IDamageable
{
    [Space]
    [SerializeField] private Animator _animator;
    [Space]
    public float health;
    public float mana;
    public bool manaAutoRegeneration;
    public float manaAutoRegenerationValue;
    public bool healthAutoRegeneration;
    public float healthAutoRegenerationValue;
    //[SerializeField] private float criticalChance;
    //[SerializeField] private float dodgeChance;

    private float maxHealth;
    private float maxMana;

    public HealthBar healthBar;
    public ManaBar manaBar;

    // animations IDs
    private int _animIDDeath;

    private PhotonView view;

    private void Start()
    {
        view = GetComponent<PhotonView>();
        AssignAnimationIDs();

        if (view.IsMine)
        {
            maxHealth = health;
            maxMana = mana;
            manaBar.maxValue = maxMana;
            healthBar.maxValue = maxHealth;
        }
    }

    private void Update()
    {
        if (manaAutoRegeneration && mana < maxMana)
        {
            ChangeManaPool(manaAutoRegenerationValue * Time.deltaTime);
        }
        //if (healthAutoRegeneration)
        //{
        //    health += healthAutoRegenerationValue * Time.deltaTime;
        //}
    }

    private void AssignAnimationIDs()
    {
        _animIDDeath = Animator.StringToHash("Death");
    }

    public void ChangeManaPool(float value)
    {
        mana += value;
        manaBar.ChangeValue(mana);
    }

    public void TakeDamage(float damage)
    {
        view.RPC("RPC_TakeDamage", RpcTarget.All, damage);
    }

    [PunRPC]
    public void RPC_TakeDamage(float damage)
    {
        health -= damage;
        if (view.IsMine)
        {
            healthBar.ChangeValue(health);
        }
        
        if (health <= 0)
        {
            _animator.SetTrigger(_animIDDeath);
        }
    }
}
