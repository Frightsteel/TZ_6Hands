using UnityEngine;
using Photon.Pun;
using Cinemachine;
using StarterAssets;

public class Spawner : MonoBehaviour
{
    [Space]
    [SerializeField] private GameObject playerPF;
    [SerializeField] private Projectile fireballPF;
    [SerializeField] private GameObject vCam;
    [SerializeField] private GameObject healthBar;
    [SerializeField] private GameObject manaBar;
    [Space]
    [SerializeField] private int poolSize;
    [SerializeField] private bool autoExpand;
    [Space]
    [SerializeField] private Transform[] spawnPoints;

    public PoolMono<Projectile> fireballPool { get; private set; }

    void Start()
    {
        GameObject player = PhotonNetwork.Instantiate(playerPF.name, spawnPoints[PhotonNetwork.CurrentRoom.PlayerCount - 1].position, Quaternion.identity);
        
        if (player.GetComponent<PhotonView>().IsMine)
        {
            vCam.GetComponent<CinemachineVirtualCamera>().Follow = player.GetComponent<ThirdPersonController>().CinemachineCameraTarget.transform;
            player.GetComponent<PlayerStats>().healthBar = healthBar.GetComponent<HealthBar>();
            player.GetComponent<PlayerStats>().manaBar = manaBar.GetComponent<ManaBar>();
        }

        fireballPool = new PoolMono<Projectile>(fireballPF, poolSize, transform);
        fireballPool.autoExpand = autoExpand;
    }
}
