using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillZombie : Skill
{
    private float shootSpeed;
    private float dashTime;
    [SerializeField] private GameObject skillBall;
    [SerializeField] private Transform skillPos;
    private void Start()
    {
        shootSpeed = 20;
        dashTime = 0.25f;
    }
    public override void SkillUse()
    {
        Debug.Log("Zombie skill ");
        StartCoroutine(Shoot());
    }
    IEnumerator Shoot()
    {
        Debug.Log("Skill on");
        GameObject cloneSkillBall = Instantiate(skillBall,transform.position, transform.rotation);
        Rigidbody bulletRigid = cloneSkillBall.GetComponent<Rigidbody>();
        bulletRigid.velocity = this.transform.forward* shootSpeed;
        yield return null;
    }
}

