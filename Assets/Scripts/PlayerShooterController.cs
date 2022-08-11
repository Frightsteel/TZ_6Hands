using StarterAssets;
using UnityEngine;
using Photon.Pun;

public class PlayerShooterController : MonoBehaviour
{
    [SerializeField] private StarterAssetsInputs input;
    [SerializeField] private LayerMask aimColliderLayerMask = new LayerMask();
    [SerializeField] private Transform fireballProjectilePF;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private Animator animator;

    private Projectile currentProjectile;
    private Spawner spawner;

    // animations IDs
    private int animIDCast;

    private bool hasAnimator;
    private bool isCasting = false;

    private float timer = 1f;

    [SerializeField] private float skillDamage; //temp, later skill class
    private float skillDamageSUM;

    private PhotonView view;

    private void Start()
    {
        hasAnimator = TryGetComponent(out animator);
        view = GetComponent<PhotonView>();
        spawner = GameObject.Find("Spawner").GetComponent<Spawner>();
        AssignAnimationIDs();
    }

    private void Update()
    {
        if (view.IsMine)
        {
            hasAnimator = TryGetComponent(out animator);
            Skill();
        }
    }

    private void AssignAnimationIDs()
    {
        animIDCast = Animator.StringToHash("Cast");
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
        currentProjectile.spawnPoint = shootPoint;
        currentProjectile.castUser = view.ViewID;
    }

    private void Skill()
    {
        if (input.skill && GetComponent<PlayerStats>().mana >= 25f)//temp
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
                if (hasAnimator)
                {
                    animator.SetBool(animIDCast, true);
                }
            }
        }
        else if (!input.skill)
        {
            if (isCasting)
            {
                isCasting = false;

                timer = Time.time - timer;
                MathDamage(timer);

                //character anim
                if (hasAnimator)
                {
                    animator.SetBool(animIDCast, false);
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
        currentProjectile.isCasted = true;
        currentProjectile.damage = skillDamageSUM;
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