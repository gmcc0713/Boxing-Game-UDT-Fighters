using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public GameObject player;
    Vector3 cameraPoint = new Vector3(0, 3, -6);

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void LateUpdate()
    {
       transform.position = player.transform.position + cameraPoint;
    }
}
