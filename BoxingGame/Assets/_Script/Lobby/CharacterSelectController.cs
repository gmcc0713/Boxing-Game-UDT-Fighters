using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using UnityEngine.SceneManagement;

public enum Character
{
    Empty = 0,
    Random,
    Horse,
    Zombie,
    Ninja,
    Count ,
}
public enum Direction
{
    Left = -1,
    Right = 1,
}
public class CharacterSelectController : MonoBehaviourPunCallbacks
{
    public Button leftButtonP1;
    public Button rightButtonP1;
    public Button leftButtonP2;
    public Button rightButtonP2;
    [SerializeField] private GameObject[]characters;


    //public bool isReady = false;
    [SerializeField] private Button ReadyButton;

    private Character curCharacterP1 = Character.Empty;
    private Character curCharacterP2 = Character.Empty;
    private LobbySceneManager lobbyScene;
    void Start()
    {
        leftButtonP1.interactable = false;
        rightButtonP1.interactable = true;
        leftButtonP2.interactable = false;
        rightButtonP2.interactable = true;
        lobbyScene = FindObjectOfType<LobbySceneManager>();
		//마스터 클라이언트의 화면에선 P2의 버튼 false
		if (PhotonNetwork.IsMasterClient)
        {
            leftButtonP2.gameObject.SetActive(false);
            rightButtonP2.gameObject.SetActive(false);

            if (!photonView.IsMine)
            {
                //일반 플레이어가 방에 들어왔을때 정보를 변경
                photonView.RPC("SyncCharacterChangeP1", RpcTarget.Others, (int)curCharacterP1);
                PlayerPrefs.SetInt("Player1Character", (int)curCharacterP1);
            }
        }
        else
        {
            leftButtonP1.gameObject.SetActive(false);
            rightButtonP1.gameObject.SetActive(false);
            //마스터에게 정보를 받음
            photonView.RPC("RequestMasterClientCharacterSelection", RpcTarget.MasterClient);
        }

        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("SyncCharacterChangeP1", RpcTarget.Others, (int)curCharacterP1);
            PlayerPrefs.SetInt("Player1Character", (int)curCharacterP1);
        }
   //     else
   //     {
   //         //여기가 들어왔을때 플레이어2가 랜덤캐릭터로 변경하는 부분 여기서 준비가 활성화 되어야함 그치?
   //         curCharacterP2 = Character.Random;
           
			//photonView.RPC("SyncCharacterChangeP2", RpcTarget.All, (int)curCharacterP2);
   //         PlayerPrefs.SetInt("Player2Character", (int)curCharacterP2);
   //     }
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
        OnCharacterSelected((int)curCharacterP1);
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
        OnCharacterSelected((int)curCharacterP1);
        if (photonView.IsMine)
        {
            photonView.RPC("SyncCharacterChangeP1", RpcTarget.Others, (int)curCharacterP1);
            //게임매니저에 선택한 열거형값 전달
            PlayerPrefs.SetInt("Player1Character", (int)curCharacterP1);
            Debug.Log((int)curCharacterP1);
        }
    }
    [PunRPC]
    void RequestMasterClientCharacterSelection()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            // 방장이 아닌 경우에만 방장의 캐릭터 선택 정보를 요청
            photonView.RPC("SyncCharacterChangeP1", RpcTarget.Others, (int)curCharacterP1);
            PlayerPrefs.SetInt("Player1Character", (int)curCharacterP1);
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

            
            if (lobbyScene != null)
            {
                lobbyScene.ReadyButton.interactable = true;
            }
        }

    }
    [PunRPC]
    void SyncCharacterChangeP2(int curCharacter)
    {
		//lobbyScene.ReadyButton.interactable = true;
		ChangeCharacter(curCharacter);

        //PlayerPrefs.SetInt("Player2Character", curCharacter);
    }
    public void OnCharacterSelected(int selectedCharacter)
    {

        // 선택된 캐릭터 정보를 RPC로 다른 플레이어들에게 전달
        photonView.RPC("SyncCharacterSelection", RpcTarget.OthersBuffered, selectedCharacter);
    }

    [PunRPC]
    void SyncCharacterSelection(int selectedCharacter)
    {
        // 다른 플레이어가 방에 들어왔을 때 이 RPC 메시지를 받아 캐릭터를 설정
        ChangeCharacter(selectedCharacter);
    }

    void ChangeCharacter(int curCharacter)
    {
        Debug.Log(curCharacter);

        if(curCharacter == 0)
        {
            return;
        }
        
        foreach (GameObject c in characters) 
        {
            c.SetActive(false);
        }

        characters[curCharacter].SetActive(true);
    }
    public void Player2Delete(bool p2Exit)
    {
        Debug.Log(p2Exit);
        photonView.RPC("ResetPlayer2", RpcTarget.MasterClient);
    }

    [PunRPC]
    public void ResetPlayer2()
    {
        Debug.Log("RPC호출");
        characters[1].SetActive(false);
        characters[2].SetActive(false);
        characters[3].SetActive(false);
        characters[4].SetActive(false);
        lobbyScene.GameStartButton.interactable = false;
        lobbyScene.ReadyButtonRed.gameObject.SetActive(false);
        lobbyScene.ReadyButton.gameObject.SetActive(false);
        rightButtonP1.interactable = true;
        leftButtonP1.interactable = true;
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("TitleScene");
    }

}
