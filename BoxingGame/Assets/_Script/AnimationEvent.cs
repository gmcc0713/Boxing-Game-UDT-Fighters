using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvent : MonoBehaviour
{
	public MultiAttackCollider attackCollider;

    public void Attack(float dmg)
    {
        attackCollider.SetDamage(dmg);

		attackCollider.gameObject.SetActive(true);

	}
    public void AttackEnd()
    {
		attackCollider.gameObject.SetActive(false);
	}
    // Update is called once per frame
    void Update()
    {
        
    }
}
