using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;

public class PlayerShooterController : MonoBehaviour
{
    [SerializeField] private StarterAssetsInputs _input;
    [SerializeField] private LayerMask aimColliderLayerMask = new LayerMask();
    [SerializeField] private Transform fireballProjectilePF;
    [SerializeField] public Transform shootPoint;

    [SerializeField] private Animator _animator;

    private MonoBehaviour currentProjectile;
    private Spawner spawner;

    // animations IDs
    private int _animIDSkill;

    private bool _hasAnimator;
    private bool isCasting = false;

    private float timer = 1f;

    [SerializeField] private float skillDamage;
    private float skillDamageSUM;

    private PhotonView view;

    private void Start()
    {
        _hasAnimator = TryGetComponent(out _animator);
        view = GetComponent<PhotonView>();
        spawner = GameObject.Find("Spawner").GetComponent<Spawner>();
        AssignAnimationIDs();
    }

    private void Update()
    {
        if (view.IsMine)
        {
            _hasAnimator = TryGetComponent(out _animator);

            Skill();
        }
    }

    private void AssignAnimationIDs()
    {
        _animIDSkill = Animator.StringToHash("Cast");
    }

    private void RotatePlayerToLookAtPoint(Vector3 worldPoint)
    {
        Vector3 worldAimTarget = worldPoint;
        worldAimTarget.y = transform.position.y;
        Vector3 aimDirection = (worldAimTarget - transform.position).normalized;
        transform.forward = Vector3.Lerp(transform.forward, aimDirection, 1f);
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);

        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderLayerMask))
        {
            return raycastHit.point;
        }
        return Vector3.zero;
    }

    public void SpawnProjectile()
    {
        view.RPC("RPC_SpawnProjectile", RpcTarget.All, GetMouseWorldPosition());
    }

    [PunRPC]
    public void RPC_SpawnProjectile(Vector3 mouseWorldPosition)
    {
        Vector3 aimDir = (mouseWorldPosition - shootPoint.position).normalized;
        
        currentProjectile = spawner.fireballPool.GetFreeElement();
        currentProjectile.transform.SetPositionAndRotation(shootPoint.position, Quaternion.LookRotation(aimDir, Vector3.up));
        currentProjectile.GetComponent<Projectile>().spawnPoint = shootPoint;
        currentProjectile.GetComponent<Projectile>().castUser = view.ViewID;
    }

    private void Skill()
    {
        if (_input.skill && GetComponent<PlayerStats>().mana >= 25f)//temp
        {
            //rotate player to look at aim point
            RotatePlayerToLookAtPoint(GetMouseWorldPosition());

            if (!isCasting)
            {
                isCasting = true;
                GetComponent<PlayerStats>().ChangeManaPool(-25f);//temp
                timer = Time.time;

                //start to cast the skill
                SpawnProjectile();

                //character anim
                if (_hasAnimator)
                {
                    _animator.SetBool(_animIDSkill, true);
                }
            }
        }
        else if (!_input.skill)
        {
            if (isCasting)
            {
                isCasting = false;

                timer = Time.time - timer;
                MathDamage(timer);

                //character anim
                if (_hasAnimator)
                {
                    _animator.SetBool(_animIDSkill, false);
                }
            }
        }
    }

    private void MathDamage(float timer)
    {
        skillDamageSUM = Mathf.Pow(skillDamage, timer);
    }

    // used in anim events
    public void DisableCast()
    {
        currentProjectile.GetComponent<Projectile>().isCasted = true;
        currentProjectile.GetComponent<Projectile>().damage = skillDamageSUM;
        if (view.IsMine)
        {
            view.RPC("RPC_DisableCast", RpcTarget.All, GetMouseWorldPosition());
        }
    }

    [PunRPC]
    public void RPC_DisableCast(Vector3 mouseWorldPosition)
    {
        RotatePlayerToLookAtPoint(mouseWorldPosition);
        currentProjectile.transform.LookAt(mouseWorldPosition);
    }
}