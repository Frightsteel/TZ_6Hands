using UnityEngine;
using Photon.Pun;

public class PlayerStats : MonoBehaviour, IDamageable
{
    [Space]
    [SerializeField] private Animator animator;
    [Space]
    public HealthBar healthBar;
    public ManaBar manaBar;
    [Space]
    public float health;
    public float mana;
    [SerializeField] private bool manaAutoRegeneration;
    [SerializeField] private float manaAutoRegenerationValue;
    //[SerializeField] private float criticalChance;
    //[SerializeField] private float dodgeChance;

    private float maxHealth;
    private float maxMana;

    // animations IDs
    private int animIDDeath;

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
            ChangeManaPool(manaAutoRegenerationValue * Time.deltaTime);
    }

    private void AssignAnimationIDs()
    {
        animIDDeath = Animator.StringToHash("Death");
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
            healthBar.ChangeValue(health);
        
        if (health <= 0)
            animator.SetTrigger(animIDDeath);
    }
}
