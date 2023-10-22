using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class LobbySceneManager : MonoBehaviourPunCallbacks
{
    [SerializeField] Button GameStartButton;
    [SerializeField] Button ReadyButton;

    private bool player2Ready = false; //플레이어2 준비상태 
    void Start()
    {
        ReadyButton.interactable = true;
        GameStartButton.interactable = false; //처음엔 비활성화
    }
    public void OnReadyButtonClicked()
    {
        //방장이아닌 플레이어가 클릭하였을 때
        if (!PhotonNetwork.IsMasterClient)
        {
            player2Ready = true;  //준비상태 true
            photonView.RPC("UpdateGameStartButton", RpcTarget.All, player2Ready); //모든pc에게 준비상태 전달

        }
    }
    [PunRPC]
    private void UpdateGameStartButton(bool isPlayer2Ready)
    {
        //게임시작버튼 true로
        GameStartButton.interactable = isPlayer2Ready;
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
            PhotonNetwork.LeaveRoom();
        }
    }
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("TitleScene");
    }
}
