//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using Photon.Pun;
//public class LivingEntity : MonoBehaviourPun, IDamageable
//{
//    public float startingHealth = 100f; //시작체력
//    public float health { get; protected set; } //현재 체력
//    public bool dead { get; protected set; } // 사망상태
//    public event Action onDeath; //사망시 발동하는 이벤트

//    [PunRPC]
//    public void ApplyUpdateHealth(float newHealth, bool newDead)
//    {
//        health = newHealth;
//        dead = newDead;
//    }
//    //생명체가 활성화될 때 상태를 리셋
//    protected virtual void OnEnable()
//    {
//        dead = false; //사망하지 않는 상태로 시작
//        health = startingHealth; // 체력을 시작 체력으로
//    }
//    //데미지를 입는 기능
//    [PunRPC]
//    public virtual void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
//    {
//        if(PhotonNetwork.IsMasterClient)
//        {
//            //데미지만큼 체력감소
//            health -= damage;
//            photonView.RPC("ApplyUpdateHealth", RpcTarget.Others, health,dead);
//            //다른 클라이언트도 OnDamage를 실행
//            photonView.RPC("OnDamage", RpcTarget.Others, damage, hitPoint, hitNormal);
//        }
//        //체력이 0 이하&&아직 죽지 않았다면 사망처리 실행
//        if (health <= 0 && !dead)
//        {
//            Die();
//        }
//    }
//    //체력을 회복하는 기능 < 이건 추후에
//    //사망처리
//    public virtual void Die()
//    {
//        //onDeath 이벤트에 등록된 메서드가 있다면 실행
//        if (onDeath != null)
//        {
//            onDeath();
//        }
//        //사망 상태를 참으로 변경
//        dead = true;
//    }
   
//}
