using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MultiAttackCollider : MonoBehaviourPun
{
    public float damage = 10f;
    public float MpUp = 25f;

    private MultiPlayer player;
    public ParticleSystem attack;
    public int a;

    void Start()
    {
        player = transform.parent.GetComponent<MultiPlayer>();
        a = player.noOfClicks;
    }
    [PunRPC]
    public void OnTriggerEnter(Collider other)
    {
        if (!photonView.IsMine)
        {
            return; // 로컬 플레이어가 아니면 처리하지 않습니다.
        }

        if (other.CompareTag("Player") && !other.GetComponent<PhotonView>().IsMine)
        {
            Debug.Log("noOfClicks : " + a);
            int attackerID = photonView.ViewID; // 공격한 플레이어의 PhotonView ID를 가져옵니다.
            int targetID = other.GetComponent<PhotonView>().ViewID; // 공격 대상 플레이어의 PhotonView ID를 가져옵니다.
       
            if (attackerID != targetID)
            {
                other.GetComponent<MultiPlayer>().TakeDamage(damage, other); // 공격 대상 플레이어에게 데미지를 주도록 수정합니다.
                attack.Play();
                Debug.Log("play particle");
                player.TakeMp(MpUp); //자신의 Mp를 회복하게
            }

        }
    }
}
