using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvent : MonoBehaviour
{
	public MultiAttackCollider attackCollider;
    public SoundManager soundManager;
	private void Start()
	{

	}
	public void Attack(float dmg)
    {
        Debug.Log("Ani Attack");
        attackCollider.SetDamage(dmg);
		attackCollider.gameObject.SetActive(true);
	}
    public void AttackEnd()
    {
		attackCollider.gameObject.SetActive(false);
	}
    //public void AttackSound(int idx)
    //{
    //    SoundManager.Instance.PlayCharacterAttackSound();
    //}
    // Update is called once per frame
    //void Update()
    //{

    //}
}
