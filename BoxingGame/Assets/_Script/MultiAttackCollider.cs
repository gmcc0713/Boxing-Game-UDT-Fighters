using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MultiAttackCollider : MonoBehaviourPun
{
    public float damage = 10;
    public float MpUp = 10;

    [PunRPC]
    private void OnTriggerEnter(Collider other)
    {
        if (!photonView.IsMine)
        {
            return; // 로컬 플레이어가 아니면 처리하지 않습니다.
        }

        if (other.CompareTag("Player") && !other.GetComponent<PhotonView>().IsMine)
        {
            int attackerID = photonView.ViewID; // 공격한 플레이어의 PhotonView ID를 가져옵니다.
            int targetID = other.GetComponent<PhotonView>().ViewID; // 공격 대상 플레이어의 PhotonView ID를 가져옵니다.

            if (attackerID != targetID)
            {
                other.GetComponent<MultiPlayer>().TakeDamage(damage, attackerID); // 공격 대상 플레이어에게 데미지를 주도록 수정합니다.
            }
        }
        else if (other.CompareTag("Player") && photonView.IsMine)
        {
            //int myId = photonView.ViewID;
            other.GetComponent<MultiPlayer>().TakeMp(MpUp);
        }
    }
}
