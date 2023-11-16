using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleCameraSetup : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        CinemachineVirtualCamera followCamSing =
            FindObjectOfType<CinemachineVirtualCamera>();

        followCamSing.Follow = transform;
        followCamSing.LookAt = transform;
    }

}
