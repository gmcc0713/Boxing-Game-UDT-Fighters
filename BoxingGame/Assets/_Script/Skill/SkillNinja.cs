using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillNinja : Skill
{
    private float shootSpeed;
    private float dashTime;
     public MultiPlayer player;
    [SerializeField] private GameObject skillBall;
    [SerializeField] private Transform skillPos;
    private void Start()
    {
        shootSpeed = 20;
        dashTime = 0.25f;
    }
    [PunRPC]
    public override void SkillUse()
    {
        Debug.Log("Zombie skill ");
        photonView.RPC("NinjaShoot", RpcTarget.All);
       
    }
    [PunRPC]
    public void NinjaShoot()
    {
        Shoot();
    }
    void Shoot()
    {
        Debug.Log("Skill on");
        GameObject cloneSkillBall = Instantiate(skillBall,transform.position, transform.rotation);
        cloneSkillBall.GetComponent<Bullet>().SetPlayer(player);
		Rigidbody bulletRigid = cloneSkillBall.GetComponent<Rigidbody>();
        bulletRigid.velocity = this.transform.forward* shootSpeed;

    }
}

