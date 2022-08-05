using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooterController : MonoBehaviour
{
    [SerializeField] private StarterAssetsInputs _input;
    [SerializeField] private LayerMask aimColliderLayerMask = new LayerMask();
    [SerializeField] private Transform fireballProjectilePF;
    [SerializeField] private Transform spawnFireballPosition;

    [SerializeField] private Animator _animator;

    private Transform currentProjectile;

    // animations IDs
    private int _animIDSkill;

    private bool _hasAnimator;
    private bool isCasting = false;

    private float timer = 1f;
    private bool timerON;

    [SerializeField] private float skillDamage;
    private float skillDamageSUM;

    private void Start()
    {
        _hasAnimator = TryGetComponent(out _animator);

        AssignAnimationIDs();
    }

    private void Update()
    {
        _hasAnimator = TryGetComponent(out _animator);

        //Shoot();
        Skill();
    }

    private void AssignAnimationIDs()
    {
        _animIDSkill = Animator.StringToHash("Cast");
    }

    private void Shoot()
    {
        if (_input.shoot)
        {
            Vector3 mouseWorldPosition = GetMouseWorldPosition();
            //Rotates player
            RotatePlayerToLookAtPoint(mouseWorldPosition);

            //Spawn and rotates projectile
            Vector3 aimDir = (mouseWorldPosition - spawnFireballPosition.position).normalized;
            Instantiate(fireballProjectilePF, spawnFireballPosition.position, Quaternion.LookRotation(aimDir, Vector3.down));//temp, then pool
            Debug.Log("Created");
            _input.shoot = false;
        }
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

    private Transform SpawnProjectile()
    {
        Vector3 aimDir = (GetMouseWorldPosition() - spawnFireballPosition.position).normalized;
        return Instantiate(fireballProjectilePF, spawnFireballPosition.position, Quaternion.LookRotation(aimDir, Vector3.up));//temp, then pool
    }

    private void Skill()
    {
        if (_input.skill)
        {
            //rotate player to look at aim point
            RotatePlayerToLookAtPoint(GetMouseWorldPosition());

            if (!isCasting)
            {
                isCasting = true;

                timer = Time.time;

                //start to cast the skill
                currentProjectile = SpawnProjectile();
                currentProjectile.GetComponent<Projectile>().spawnPoint = spawnFireballPosition;

                //character anim
                if (_hasAnimator)
                {
                    _animator.SetBool(_animIDSkill, true);
                }
            }
        }
        else if (!_input.skill)
        {
            //rotate player to look at aim point
            //RotatePlayerToLookAtPoint(GetMouseWorldPosition());

            //drop current projectile
            //currentProjectile.GetComponent<Projectile>().isCasted = true; // after anim

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

    public void EnableCast()
    {
        currentProjectile.GetComponent<Projectile>().isCasted = false;
    }

    public void DisableCast()
    {
        RotatePlayerToLookAtPoint(GetMouseWorldPosition());
        currentProjectile.LookAt(GetMouseWorldPosition());
        currentProjectile.GetComponent<Projectile>().isCasted = true;
    }
}
