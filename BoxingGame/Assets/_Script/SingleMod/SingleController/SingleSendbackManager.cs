using Photon.Pun.Demo.PunBasics;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleSendbackManager : MonoBehaviour
{
    public Animator animatorSend;
    public void TakeDamageSige()
    {
        Debug.Log("attack");
        animatorSend.SetTrigger("GetHit");
    }
}
