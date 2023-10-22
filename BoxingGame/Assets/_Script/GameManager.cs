using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using TMPro;

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
            // ���� �̱��� ������ ���� ������Ʈ�� �Ҵ���� �ʾҴٸ�
            if (m_instance == null)
            {
                // ������ GameManager ������Ʈ�� ã�� �Ҵ�
                m_instance = FindObjectOfType<GameManager>();
            }

            // �̱��� ������Ʈ�� ��ȯ
            return m_instance;
        }
    }
    private static GameManager m_instance; // �̱����� �Ҵ�� static ����

    public GameObject zombiePrefab;
    public GameObject horsePrefab;
    public static int player1 = 0;
    public static int player2 = 0;
    public PhotonView pv;

    public Transform player1SpawnPoint;
    public Transform player2SpawnPoint;

    public int round = 1;
    public TextMeshProUGUI roundText;
    private void Awake()
    {
        // ���� �̱��� ������Ʈ�� �� �ٸ� GameManager ������Ʈ�� �ִٸ�
        //if (instance != this)
        //{
        //    // �ڽ��� �ı�
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

        // �÷��̾� 1�� player1SpawnPoint�� ����
        if (PhotonNetwork.IsMasterClient)
        {
            SpawnPlayer(player1, player1SpawnPoint.position);
        }

        // �÷��̾� 2�� player2SpawnPoint�� ����
        else
        {
            SpawnPlayer(player2, player2SpawnPoint.position);
        }
        RoundChange(1);
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

        // ��Ʈ��ũ ���� ��� Ŭ���̾�Ʈ�鿡�� ���� ����
        // ��, �ش� ���� ������Ʈ�� �ֵ�����, ���� �޼��带 ���� ������ Ŭ���̾�Ʈ���� ����
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
    public void RoundChange(int a)
    {
        round = a;
        Debug.Log(roundText.text);
        roundText.text = round.ToString();  
    }
}
