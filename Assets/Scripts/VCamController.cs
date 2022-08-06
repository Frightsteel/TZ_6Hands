using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class VCamController : MonoBehaviour
{
    PhotonView view;
    private void Start()
    {
        view = GetComponent<PhotonView>();
    }
}
