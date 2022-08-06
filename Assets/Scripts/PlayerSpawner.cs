using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Cinemachine;
using StarterAssets;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject playerPF;
    [SerializeField] private GameObject vCam;

    // Start is called before the first frame update
    void Start()
    {
        GameObject player = PhotonNetwork.Instantiate(playerPF.name, new Vector3(0, 5, 0), Quaternion.identity);
        if (player.GetComponent<PhotonView>().IsMine)
        {
            vCam.GetComponent<CinemachineVirtualCamera>().Follow = player.GetComponent<ThirdPersonController>().CinemachineCameraTarget.transform;
        }
    }
}
