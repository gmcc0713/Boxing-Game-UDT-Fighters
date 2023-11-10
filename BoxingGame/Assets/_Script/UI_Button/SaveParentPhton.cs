using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SaveParentPhton : MonoBehaviour
{
    public Component playerPhton;

    // Start is called before the first frame update
    void Start()
    {
        playerPhton = GetComponentInParent<Animator>();
        Debug.Log(playerPhton);
    }

}
