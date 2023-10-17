using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks, IPunObservable
{
    public enum Character
    {
        Empty = 0,
        Horse,
        Zombie,
        Count,
    }
    public static GameManager instance
    {
        get
        {
            // 만약 싱글톤 변수에 아직 오브젝트가 할당되지 않았다면
            if (m_instance == null)
            {
                // 씬에서 GameManager 오브젝트를 찾아 할당
                m_instance = FindObjectOfType<GameManager>();
            }

            // 싱글톤 오브젝트를 반환
            return m_instance;
        }
    }
    private static GameManager m_instance; // 싱글톤이 할당될 static 변수

    public GameObject zombiePrefab;
    public GameObject horsePrefab;
    public static int player1 = 0;
    public static int player2 = 0;
    public PhotonView pv;

    public Transform player1SpawnPoint;
    public Transform player2SpawnPoint;

    private void Awake()
    {
        // 씬에 싱글톤 오브젝트가 된 다른 GameManager 오브젝트가 있다면
        //if (instance != this)
        //{
        //    // 자신을 파괴
        //    Destroy(gameObject);
        //}

        if (m_instance != null && m_instance != this)
        {
            Destroy(gameObject);
            return;
        }

        m_instance = this;
        DontDestroyOnLoad(gameObject);
        pv = GetComponent<PhotonView>();
    }
    // Start is called before the first frame update
    void Start()
    {
        int player1Character = PlayerPrefs.GetInt("Player1Character", 1);
        int player2Character = PlayerPrefs.GetInt("Player2Character", 1);
        player1 = player1Character;
        player2 = player2Character;
        Debug.Log(player1);
        Debug.Log(player2);

        // 플레이어 1을 player1SpawnPoint에 스폰
        if (PhotonNetwork.IsMasterClient)
        {
            SpawnPlayer(player1, player1SpawnPoint.position);
        }

        // 플레이어 2를 player2SpawnPoint에 스폰
        else
        {
            SpawnPlayer(player2, player2SpawnPoint.position);
        }
    }
    void SpawnPlayer(int playerCharacter, Vector3 spawnPosition)
    {
        GameObject selectedPrefab = null;

        if (playerCharacter == 1)
        {
            selectedPrefab = horsePrefab;
        }
        else
        {
            selectedPrefab = zombiePrefab;
        }

        // 네트워크 상의 모든 클라이언트들에서 생성 실행
        // 단, 해당 게임 오브젝트의 주도권은, 생성 메서드를 직접 실행한 클라이언트에게 있음
        PhotonNetwork.Instantiate(selectedPrefab.name, spawnPosition, Quaternion.identity);
    }

    // Update is called once per frame
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


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // Sending data to other clients
            // Example: Serialize player position and rotation
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            // Receiving data from the server
            // Example: Deserialize player position and rotation
            transform.position = (Vector3)stream.ReceiveNext();
            transform.rotation = (Quaternion)stream.ReceiveNext();
        }
    }
}
