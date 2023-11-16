using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleAnimationEvent : MonoBehaviour
{
    public SingleAttackCollider attackColliderSingle;
    public SoundManager soundManager;
    public void Attack(float dmg)
    {
        Debug.Log("Ani Attack");
        attackColliderSingle.gameObject.SetActive(true);
    }
    public void AttackEnd()
    {
        attackColliderSingle.gameObject.SetActive(false);
    }
    public void AttackSound(int idx)
    {
        SoundManager.Instance.PlayCharacterAttackSound(idx);
    }
}
