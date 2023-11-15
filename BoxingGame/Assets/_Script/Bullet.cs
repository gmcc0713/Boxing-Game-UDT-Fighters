using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Unity.VisualScripting;

public class Bullet : MonoBehaviourPun
{
    private float damage = 25f;
    public float rotateSpeed;
    public Vector3 lookForward;
    MultiPlayer me;

    public void StartSkill(Vector3 skillLookForward)
    {
        lookForward = skillLookForward;
        StartCoroutine(Shoot());
    }
    private void Update()
    {
        transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);
    }
    public void SetPlayer(MultiPlayer player)
    {
        me = player;
    }
    IEnumerator Shoot()
    {
        while (true)
        {
            transform.Translate(lookForward * 5 * Time.deltaTime);
            transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);

            yield return null;
        }
    }
    [PunRPC]
    public void OnTriggerEnter(Collider other)
    {
        //if (!photonView.IsMine)
        //{
        //    return; // 로컬 플레이어가 아니면 처리하지 않습니다.
        //}
        Debug.Log("OnTriggerEnter called"); // 디버그 로그 추가

        if (other.gameObject.CompareTag("Player") && !other.GetComponent<PhotonView>().IsMine)
        {
            int attackerID = me.photonView.ViewID; // 공격한 플레이어의 PhotonView ID를 가져옵니다.
            int targetID = other.GetComponent<PhotonView>().ViewID; // 공격 대상 플레이어의 PhotonView ID를 가져옵니다.

            Debug.Log("여기도 만족");
            Debug.Log(attackerID);
            Debug.Log(targetID);
            if (attackerID != targetID)
            {
                Debug.Log("맞아라 이놈아");
                MultiPlayer multiPlayer = other.GetComponent<MultiPlayer>();
                multiPlayer.TakeDamage(damage, other);
                //other.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.All, damage, other);


                //Destroy(this);
            }
        }
    }
}
