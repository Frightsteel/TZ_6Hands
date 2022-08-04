using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooterController : MonoBehaviour
{
    [SerializeField] private StarterAssetsInputs _input;
    [SerializeField] private LayerMask aimColliderLayerMask = new LayerMask();
    [SerializeField] private Transform debugTransform;
    [SerializeField] private Transform fireballProjectilePF;
    [SerializeField] private Transform spawnFireballPosition;

    [SerializeField] private Animator _animator;

    // animations IDs
    private int _animIDSkill;

    private bool _hasAnimator;

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

        Shoot();
        Skill();
        Timer();
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
            debugTransform.position = raycastHit.point;
            return raycastHit.point;
        }
        return Vector3.zero;
    }

    private void Skill()
    {
        if (_input.skill && !timerON)
        {
            timerON = true;

            //Rotate player to look at aim point
            RotatePlayerToLookAtPoint(GetMouseWorldPosition());

            //character anim
            if (_hasAnimator)
            {
                //_animator.SetLayerWeight(1, 1f);
                _animator.SetBool(_animIDSkill, true);
            }
        }
        else if (!_input.skill && timerON)
        {
            timerON = false;
            
            //character anim
            if (_hasAnimator)
            {
                //_animator.SetLayerWeight(1, 0f);
                _animator.SetBool(_animIDSkill, false);
            }
        }
    }

    private void Timer()
    {
        if (timerON)
        {
            timer += Time.deltaTime;
        }
        else
        {
            Damage();
            //Debug.Log(skillDamageSUM);
            timer = 1f;
        }
    }

    private void Damage()
    {
        skillDamageSUM = Mathf.Pow(skillDamage, timer);
    }
}
