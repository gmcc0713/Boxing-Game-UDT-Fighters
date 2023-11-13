using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Unity.VisualScripting;

public class MultiAttackCollider : MonoBehaviourPun
{
    public float damage = 10f;
    public float MpUp = 25f;

    private MultiPlayer player;
    public ParticleSystem attack;

    void Start()
    {
        player = transform.parent.GetComponent<MultiPlayer>();
    }
    [PunRPC]
    public void OnTriggerEnter(Collider other)
    {
        if (!photonView.IsMine)
        {
            return; // ���� �÷��̾ �ƴϸ� ó������ �ʽ��ϴ�.
        }

        if (other.CompareTag("Player") && !other.GetComponent<PhotonView>().IsMine)
        {
            int attackerID = photonView.ViewID; // ������ �÷��̾��� PhotonView ID�� �����ɴϴ�.
            int targetID = other.GetComponent<PhotonView>().ViewID; // ���� ��� �÷��̾��� PhotonView ID�� �����ɴϴ�.
       
            if (attackerID != targetID)
            {
                other.GetComponent<MultiPlayer>().TakeDamage(damage, other); // ���� ��� �÷��̾�� �������� �ֵ��� �����մϴ�.
                attack.Play();
                Debug.Log("play particle");
                player.TakeMp(MpUp); //�ڽ��� Mp�� ȸ���ϰ�
            }

        }
    }

    void OnEnable()
    {
        StartCoroutine(WaitForDisable());
    }

    IEnumerator WaitForDisable()
    {
        yield return new WaitForSeconds(0.1f);
        this.gameObject.SetActive(false);
    }
}
