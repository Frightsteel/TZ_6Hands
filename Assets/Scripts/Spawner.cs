using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Cinemachine;
using StarterAssets;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject playerPF;
    [SerializeField] private GameObject vCam;
    [SerializeField] private GameObject healthBar;
    [SerializeField] private GameObject manaBar;

    [SerializeField] private int poolSize;
    [SerializeField] private bool autoExpand;
    [SerializeField] private MonoBehaviour fireballPF;
    [SerializeField] private Transform[] spawnPoints;

    public PoolMono<MonoBehaviour> fireballPool { get; private set; }

    void Start()
    {
        GameObject player = PhotonNetwork.Instantiate(playerPF.name, spawnPoints[PhotonNetwork.CurrentRoom.PlayerCount - 1].position, Quaternion.identity);
        
        if (player.GetComponent<PhotonView>().IsMine)
        {
            vCam.GetComponent<CinemachineVirtualCamera>().Follow = player.GetComponent<ThirdPersonController>().CinemachineCameraTarget.transform;
            player.GetComponent<PlayerStats>().healthBar = healthBar.GetComponent<HealthBar>();
            player.GetComponent<PlayerStats>().manaBar = manaBar.GetComponent<ManaBar>();

        }

        fireballPool = new PoolMono<MonoBehaviour>(fireballPF, poolSize, transform);
        fireballPool.autoExpand = autoExpand;
    }
}
