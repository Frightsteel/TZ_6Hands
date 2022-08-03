using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooterController : MonoBehaviour
{
    [SerializeField] private StarterAssetsInputs starterAssetsInputs;
    [SerializeField] private ThirdPersonController thirdPersonController;//temp
    [SerializeField] private LayerMask aimColliderLayerMask = new LayerMask();
    [SerializeField] private Transform debugTransform;
    [SerializeField] private Transform fireballProjectilePF;
    [SerializeField] private Transform spawnFireballPosition;

    //private InputActionReference actionReference;

    private void Update()
    {
        Vector3 mouseWorldPosition = Vector3.zero;
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderLayerMask))
        {
            debugTransform.position = raycastHit.point;
            mouseWorldPosition = raycastHit.point;
        }

        if (starterAssetsInputs.shoot)
        {
            Debug.Log("Shoot pressed ");
            //Rotates player
            Vector3 worldAimTarget = mouseWorldPosition;
            worldAimTarget.y = transform.position.y;
            Vector3 aimDirection = (worldAimTarget - transform.position).normalized;
            transform.forward = Vector3.Lerp(transform.forward, aimDirection, 1f);

            //Spawn and rotates projectile
            Vector3 aimDir = (mouseWorldPosition - spawnFireballPosition.position).normalized;
            Instantiate(fireballProjectilePF, spawnFireballPosition.position, Quaternion.LookRotation(aimDir, Vector3.down));//temp, then pool
            starterAssetsInputs.shoot = false;
        }

        if (starterAssetsInputs.skill)
        {
            Debug.Log("Skill pressed ");
            starterAssetsInputs.skill = false;
            Debug.Log("Skill canceled ");
            //Use anim
            //Use skill
        }
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            //Debug.Log("test1");
            //thirdPersonController.JumpAndGravity();
            Debug.Log("Ge");
        }
        if (context.performed)
        {
            Debug.Log("Genshin impact top game");
        }
        if (context.canceled)
        {
            Debug.Log("Getop game");
        }
    }
}
