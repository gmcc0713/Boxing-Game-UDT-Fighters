using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SkillZombie : Skill
{
    private void Start()
    {
    }
    [PunRPC]
    public override void SkillUse()
    {
		Debug.Log("HealSkillUse");
        photonView.RPC("ZombieHeal", RpcTarget.All);
    }
    [PunRPC]
    public void ZombieHeal()
    {
        StartCoroutine(Heal());
    }
    IEnumerator Heal()
    {
        Debug.Log("Heal");
        for(int i =0;i<5;i++)
        {
            playerController.HPHeal(10);
            Debug.Log(playerController.health);
            yield return new WaitForSeconds(1.0f);
        }

	}
}

