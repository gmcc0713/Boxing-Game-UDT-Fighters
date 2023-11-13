using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.SceneManagement;
using TMPro;
using Photon.Pun.Demo.PunBasics;
using Unity.VisualScripting;

public class LobbySceneManager : MonoBehaviourPunCallbacks
{
    [SerializeField] Button GameStartButton;
    [SerializeField] public Button ReadyButton;
    [SerializeField] public Button ReadyButtonRed;
    [SerializeField] GameObject CharacterInfoPanel;
	[SerializeField] private Button characterInfoLeft;
	[SerializeField] private Button characterInfoRight;
	[SerializeField] private GameObject[] characterInfo;
	[SerializeField] private TextMeshProUGUI[] infoTexts;

    [SerializeField] private Image masterUser;
    [SerializeField] private Image remoteUser;
    int characterInfoindex;

    private CharacterSelectController characterSelectController;
	private bool player2Ready = false; //플레이어2 준비상태 
    public bool p2Exit = false;
    void Start()
    {
        characterSelectController = FindObjectOfType<CharacterSelectController>();
        ReadyButton.interactable = false;
        GameStartButton.interactable = false; //처음엔 비활성화
        characterInfoindex = 0;
        if(PhotonNetwork.IsMasterClient)
            masterUser.gameObject.SetActive(true);
        else
            remoteUser.gameObject.SetActive(true);

    }
    //레디on
    public void OnReadyButtonClicked()
    {
        //방장이아닌 플레이어가 클릭하였을 때
        if (!PhotonNetwork.IsMasterClient)
        {
            player2Ready = true;  //준비상태 true
            
            photonView.RPC("UpdateGameStartButton", RpcTarget.All, player2Ready); //모든pc에게 준비상태 전달

        }
    }
    //레디off
    public void OnReadyOffButtonClicked()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            player2Ready = false;  //준비상태 false
            
            photonView.RPC("UpdateGameStartButton", RpcTarget.All, player2Ready); //모든pc에게 준비상태 전달

        }
    }
    [PunRPC]
    private void UpdateGameStartButton(bool isPlayer2Ready)
    {
        if(isPlayer2Ready == true)
        {
            characterSelectController.leftButtonP1.interactable = false;
            characterSelectController.leftButtonP2.interactable = false;
            characterSelectController.rightButtonP1.interactable = false;
            characterSelectController.rightButtonP2.interactable = false;
            ReadyButton.gameObject.SetActive(false);
            ReadyButtonRed.gameObject.SetActive(true);
            //게임시작버튼 true로
            GameStartButton.interactable = isPlayer2Ready;
        }
        else
        {
            characterSelectController.leftButtonP1.interactable = true;
            characterSelectController.leftButtonP2.interactable = true;
            characterSelectController.rightButtonP1.interactable = true;
            characterSelectController.rightButtonP2.interactable = true;
            ReadyButton.gameObject.SetActive(true);
            ReadyButtonRed.gameObject.SetActive(false);
            //게임시작버튼 true로
            GameStartButton.interactable = isPlayer2Ready;
        }
        
    }
    public void OnGameStartButtonClicked()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            //모든 플레이어에게 게임 시작을 알림
            photonView.RPC("StartGame", RpcTarget.All);
        }
    }
    [PunRPC]
    private void StartGame()
    {
        PhotonNetwork.LoadLevel("ActionTestScene");
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (PhotonNetwork.IsMasterClient)
            {
                // PhotonNetwork.LeaveRoom();
                photonView.RPC("AllLeaveRoom", RpcTarget.All);
            }
            else
            {
                p2Exit = true;
                Debug.Log(p2Exit);
                characterSelectController.Player2Delete(p2Exit);
                PhotonNetwork.LeaveRoom();
            }
        }
    }
    [PunRPC]
    public void AllLeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("TitleScene");
    }
  

    public void OpenCharacterInfo()
    {
        Debug.Log("Character Info Open");
        if (CharacterInfoPanel)
        {
            CharacterInfoPanel.SetActive(true);
        }
    }
    public void CloseCharacterInfo()
    {
        Debug.Log("Character Info Close");
        CharacterInfoPanel.SetActive(false);
    }
	public void ClickCharacterInfoLeftRightButton(int type)
	{
		characterInfoindex += type;
		if (characterInfoindex < 1)
		{
			characterInfoLeft.gameObject.SetActive(false);
		}
		else if (characterInfoindex > 1)
		{
			characterInfoRight.gameObject.SetActive(false);
		}
		else
		{
			characterInfoLeft.gameObject.SetActive(true);
			characterInfoRight.gameObject.SetActive(true);
		}
        for(int i =0;i<3;i++)
        {
            characterInfo[i].SetActive(false);
            infoTexts[i].gameObject.SetActive(false);
		}
        characterInfo[characterInfoindex].SetActive(true);
		infoTexts[characterInfoindex].gameObject.SetActive(true);
	}
}
