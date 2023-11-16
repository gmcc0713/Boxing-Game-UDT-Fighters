using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleSkillHorse : SingleSkill
{
    private float dashSpeed;
    private float dashTime;

    private void Start()
    {
        dashSpeed = 10;
        dashTime = 0.25f;
    }
    [PunRPC]
    public override void SkillUseSing()
    {
        Debug.Log("Horse use skill");
        photonView.RPC("HorseDash", RpcTarget.All);
    }
    [PunRPC]
    public void HorseDashSing()
    {
        StartCoroutine(DashSing());
    }
    IEnumerator DashSing()
    {
        float startTime = Time.time;

        Debug.Log("111");
        while (Time.time < startTime + dashTime)
        {
            Debug.Log("Dash update");
            playerControllerSing.transform.position += transform.forward * dashSpeed * Time.deltaTime;

            yield return null;
        }
        playerControllerSing.animatorSing.SetBool("IsSkill", false);
    }

}
