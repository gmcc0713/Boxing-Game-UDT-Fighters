using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillHorse : Skill
{
    private float dashSpeed;
    private float dashTime;

    private void Start()
    {
        dashSpeed = 20;
        dashTime = 0.25f;
    }
    public override void SkillUse()
    {
        Debug.Log("SkillYUseeeee");
        StartCoroutine(Dash());
    }
    IEnumerator Dash()
    {
        float startTime = Time.time;

        while (Time.time < startTime + dashTime)
        {
            Debug.Log("Dash update");
            playerController.transform.position += transform.forward * dashSpeed * Time.deltaTime;
            yield return null;
        }
    }
}
