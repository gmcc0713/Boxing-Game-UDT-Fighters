using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SaveParentPhton : MonoBehaviour
{
    protected Component playerPhton;

    // Start is called before the first frame update
    void Start()
    {
        playerPhton = GetComponentInParent<PhotonView>();
        Debug.Log("Save parent Phton : "+ playerPhton);
    }

}
