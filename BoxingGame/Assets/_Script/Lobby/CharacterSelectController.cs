using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
public enum Character
{
    Empty = 0,
    Random,
    Horse,
    Zombie,
    Count ,
}
public class CharacterSelectController : MonoBehaviourPunCallbacks
{
    [SerializeField] private Button leftButtonP1;
    [SerializeField] private Button rightButtonP1;
    [SerializeField] private Button leftButtonP2;
    [SerializeField] private Button rightButtonP2;
    [SerializeField] private GameObject[]characters;
    private Character curCharacterP1 = Character.Empty;
    private Character curCharacterP2 = Character.Empty;

    void Start()
    {
        leftButtonP1.interactable = false;
        rightButtonP1.interactable = true;
        leftButtonP2.interactable = false;
        rightButtonP2.interactable = true;

        //마스터 클라이언트의 화면에선 P2의 버튼 false
        if (PhotonNetwork.IsMasterClient)
        {
            leftButtonP2.gameObject.SetActive(false);
            rightButtonP2.gameObject.SetActive(false);
        }
        else
        {
            leftButtonP1.gameObject.SetActive(false);
            rightButtonP1.gameObject.SetActive(false);
        }

        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("SyncCharacterChangeP1", RpcTarget.Others, (int)curCharacterP1);
            PlayerPrefs.SetInt("Player1Character", (int)curCharacterP1);
        }
        else
        {
            photonView.RPC("SyncCharacterChangeP2", RpcTarget.Others, (int)curCharacterP2);
            PlayerPrefs.SetInt("Player2Character", (int)curCharacterP2);
        }
    }
    //방장의 왼쪽 버튼클릭 이벤트
    public void ClickLeftButtonP1()
    {
        if ((int)curCharacterP1 > (int)Character.Empty + 1)
        {
            rightButtonP1.interactable = true;
            curCharacterP1--;
            ChangeCharacter((int)curCharacterP1);
            if ((int)curCharacterP1 <= 1)
            {
                leftButtonP1.interactable = false;
            }
        }
        if (photonView.IsMine)
        {
            photonView.RPC("SyncCharacterChangeP1", RpcTarget.Others, (int)curCharacterP1);
            //게임매니저에 선택한 열거형값 전달
            PlayerPrefs.SetInt("Player1Character", (int)curCharacterP1);

            Debug.Log((int)curCharacterP1);
        }
    }
    //방장의 오른쪽 버튼클릭 이벤트
    public void ClickRightButtonP1()
    {
        if ((int)curCharacterP1 < (int)Character.Count - 1)
        {
            leftButtonP1.interactable = true;
            curCharacterP1++;
            ChangeCharacter((int)curCharacterP1);
            if ((int)curCharacterP1 >= (int)Character.Count - 1)
            {
                rightButtonP1.interactable = false;
            }
        }
        if (photonView.IsMine)
        {
            photonView.RPC("SyncCharacterChangeP1", RpcTarget.Others, (int)curCharacterP1);
            //게임매니저에 선택한 열거형값 전달
            PlayerPrefs.SetInt("Player1Character", (int)curCharacterP1);
            Debug.Log((int)curCharacterP1);
        }
    }

    [PunRPC]
    void SyncCharacterChangeP1(int curCharacter)
    {
        ChangeCharacter(curCharacter);
    }
    public void ClickLeftButtonP2()
    {
        Debug.Log("성공");
        if ((int)curCharacterP2 > (int)Character.Empty + 1)
        {
            rightButtonP2.interactable = true;
            curCharacterP2--;
            ChangeCharacter((int)curCharacterP2);
            if ((int)curCharacterP2 <= 1)
            {
                leftButtonP2.interactable = false;
            }
        }
        if (!photonView.IsMine)
        {
            photonView.RPC("SyncCharacterChangeP2", RpcTarget.Others, (int)curCharacterP2);

            PlayerPrefs.SetInt("Player2Character", (int)curCharacterP2);
            Debug.Log((int)curCharacterP2);
        }

    }
    public void ClickRightButtonP2()
    {
        Debug.Log("성공");
        if ((int)curCharacterP2 < (int)Character.Count - 1)
        {
            leftButtonP2.interactable = true;
            curCharacterP2++;
            ChangeCharacter((int)curCharacterP2);
            if ((int)curCharacterP2 >= (int)Character.Count - 1)
            {
                rightButtonP2.interactable = false;
            }
        }
        if (!photonView.IsMine)
        {
            photonView.RPC("SyncCharacterChangeP2", RpcTarget.Others, (int)curCharacterP2);

            PlayerPrefs.SetInt("Player2Character", (int)curCharacterP2);
            Debug.Log((int)curCharacterP2);
        }

    }
    [PunRPC]
    void SyncCharacterChangeP2(int curCharacter)
    {
        ChangeCharacter(curCharacter);

        //PlayerPrefs.SetInt("Player2Character", curCharacter);
    }
    void ChangeCharacter(int curCharacter)
    {
        Debug.Log(curCharacter);
        if (curCharacter == 0)
        {
            return;
        }
        foreach(GameObject c in characters) 
        {
            c.SetActive(false);
        }
        characters[curCharacter].SetActive(true);
    }
}
