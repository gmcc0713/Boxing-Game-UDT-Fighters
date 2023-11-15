using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class SkillHorse : Skill
{
    private float dashSpeed;
    private float dashTime;

    private void Start()
    {
        dashSpeed = 10;
        dashTime = 0.25f;
    }
    [PunRPC]
    public override void SkillUse()
    {
        Debug.Log("Horse use skill");
        photonView.RPC("HorseDash", RpcTarget.All);
    }
    [PunRPC]
    public void HorseDash()
    {
        StartCoroutine(Dash());
    }
    IEnumerator Dash()
    {
        float startTime = Time.time;

        Debug.Log("111");
        while (Time.time < startTime + dashTime)
        {
            Debug.Log("Dash update");
            playerController. transform.position += transform.forward * dashSpeed * Time.deltaTime;

            yield return null;
        }
        playerController.animator.SetBool("IsSkill", false);
	}
    
}
