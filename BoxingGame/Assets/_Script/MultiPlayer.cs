using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Pun.Demo.PunBasics;

public class MultiPlayer : MonoBehaviourPunCallbacks, IPunObservable
{
    //�ִϸ��̼� ��Ʈ�ѷ� ���� ����
    Animator animator;

    //�̵����� ��麤�� ���� ����
    Vector2 input;
    //�̵� �� ȸ������ ���������� ���� ����
    Vector3 moveVec;

    //�̵��ӷ� ���� ����
    [SerializeField] float speed;
    //ȸ���ð� ���� ����
    public float turnSmoothTime = 0.1f;
    //ȸ���ӷ� ���� ����
    private float turnSmoothVelocity;
    //������ ���� �ο� ���� ����
    private bool isMove;

    //������ ���� �ο� ���� ����
    public bool isAttack = false;
    //���ݰ��� ���� �ο� ���� ����
    public bool canAttack = true;
    //�޺����� ���� �ο� ���� ����
    public bool canCombo = false;
    //���� ����
    public GameObject attackCollider;
    //���� �޺� ���� ����
    public int attackCombo = 0;
    //Ŭ�� Ƚ�� ����ƽ ���� ����
    public static int noOfClicks = 0;

    [Header("Health")]
    public float startHealth = 100;
    public float health;

    public Image MasterHealthBar;
    public Image remoteHealthBar;
    public GameObject MasterCanvas;
    public GameObject RemoteCanvas;
    [Header("Skill")]
    public float startMP = 0;
    private float mp;
    public Image MasterMpBar;
    public Image RemoteMpBar;
    public GameObject MasterMpCanvas;
    public GameObject RemoteMpCanvas;
    [SerializeField] private Skill skill;

    private GameManager gameManager;
    private PhotonView pv;
    //�ʱ���ġ
    private Vector3 initialPosition;
    void Start()
    {
        animator = GetComponent<Animator>();
        pv = GetComponent<PhotonView>();

        gameManager = FindObjectOfType<GameManager>();
        health = startHealth;
        mp = startMP;
        if (PhotonNetwork.IsMasterClient)
        {
            // ������ Ŭ���̾�Ʈ�̹Ƿ� ���� ����� UI�� �Ҵ��մϴ�.
            MasterHealthBar = transform.Find("MasterCanvas/Master/MasterHP").GetComponent<Image>();
            MasterHealthBar.fillAmount = health / startHealth;
            MasterMpBar = transform.Find("MasterMpCanvas/MasterMP/MasterMP").GetComponent<Image>();
            MasterMpBar.fillAmount = mp / startHealth;
            if (pv.IsMine)
            {
                // ���� �÷��̾ �ڽ��� ��ü�� �ƴ� ���, RemoteCanvas�� ��Ȱ��ȭ�մϴ�.
                RemoteCanvas.SetActive(false);
                RemoteMpCanvas.SetActive(false);
            }
            else
            {
                // ���� �÷��̾ �ڽ��� ��ü�� �ƴϸ�, MasterCanvas�� ��Ȱ��ȭ�մϴ�.
                MasterCanvas.SetActive(false);
                MasterMpCanvas.SetActive(false);
            }
        }
        else
        {
            // �Ϲ� Ŭ���̾�Ʈ�̹Ƿ� ������ ����� UI�� �Ҵ��մϴ�.
            remoteHealthBar = transform.Find("RemoteCanvas/Remote/RemoteHP").GetComponent<Image>();
            remoteHealthBar.fillAmount = health / startHealth;
            RemoteMpBar = transform.Find("RemoteMpCanvas/RemoteMP/RemoteMP").GetComponent<Image>();
            RemoteMpBar.fillAmount = mp / startHealth;
            if (!pv.IsMine)
            {
                // ���� �÷��̾ �ڽ��� ��ü�� �ƴϸ� RemoteCanvas�� ��Ȱ��ȭ�մϴ�.
                RemoteCanvas.SetActive(false);
                RemoteMpCanvas.SetActive(false);
            }
            else
            {
                // ���� �÷��̾ �ڽ��� ��ü�� , MasterCanvas�� ��Ȱ��ȭ�մϴ�.
                MasterCanvas.SetActive(false);
                MasterMpCanvas.SetActive(false);
            }
        }
        skill.Initilize(this);
        initialPosition = transform.position;
    }
    private void FixedUpdate()
    {
        if (!pv.IsMine)
        {
            return;
        }

        //�Ƚõ� ������Ʈ�� ���� üũ�ϱ�
        Idle();
    }
    void Update()
    {
        if (!pv.IsMine)
        {
            return;
        }

        OnAttack();
        Move();
    }
    void OnAttack()
    {
   //     if (Input.GetKeyDown(KeyCode.A))
   //     {
   //         Debug.Log("A press");
   //         OnAttackAButton();
   //     }
   //     if (Input.GetKeyDown(KeyCode.S))
   //     {
   //         Debug.Log("S press");
   //         OnAttackSButton();
   //     }
   //     if (Input.GetKeyDown(KeyCode.D))
   //     {
   //         Debug.Log("D press");
			//Debug.Log(skill);
   //         if(mp == 100)
   //         {
   //             Debug.Log("��ų������ 100!");
   //             skill.SkillUse();
   //             TakeMp(-20);
   //             Debug.Log("���罺ų������" + mp);
   //         }
   //     }
    }
    public void Move()
    {
        OnMove();
        //���ݻ��°� �ƴ� �� �̵�
        if (!isAttack)
        {
            //ĳ������ ���� ���ϱ�
            //�̵� ���� �ο� ���� ���� �� ������ ����
            if (isMove)
            {
                //���� ���� �ޱ� XZ ��
                float targetAngle = Mathf.Atan2(moveVec.x, moveVec.z) * Mathf.Rad2Deg;
                //�ε巯�� ������ ������
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                //ȸ�� �ٲٱ�
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
            }

            //�̵��ϱ�
            transform.position += moveVec * speed * Time.deltaTime;

        }
        //������ �ִϸ��̼� �۵�
        animator.SetBool("IsRun", isMove);
    }

    //����üũ �Լ�
    public void Idle()
    {
        //������ �⺻������ �� �۵�
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            //���൵�� 0.2���� ���� �� ���� ��ȭ(= �⺻���·� ��ȯ)
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.2f)
            {
                attackCombo = 0;

                //Ŭ��Ƚ�� �ʱ�ȭ
                noOfClicks = 0;
                //������ ���� ����
                isAttack = false;
                //���� ���� ���� ��
                canAttack = true;
                //�޺� ���� ���� ����
                canCombo = false;

                //�ݶ��̴� ����
                CapsuleCollider collider = gameObject.GetComponent<CapsuleCollider>();
                //�ݶ��̴� ������
                collider.radius = 0.4f;
                //�ݶ��̴� �߽�
                collider.center = new Vector3(0, 1, 0f);
            }
        }

        //������ �⺻���°� �ƴϰ�, �̵� ���µ� �ƴ� �� �۵�(= �������϶�)
        else if (!(animator.GetCurrentAnimatorStateInfo(0).IsName("Run") && noOfClicks == 2))
        {
            //���൵�� 0.5�̻� �Ǹ� ���� ��ȭ(=�޺����� ���� ����)
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.5f)
            {
                //�޺����� ���� ��
                canCombo = true;

                //���൵�� 0.7�̻� �Ǹ� ���� ��ȭ(= �޺����� ���� �Ұ��� ����)
                if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f)
                {
                    //�޺� ���� ���� ����
                    canCombo = false;
                    attackCollider.SetActive(false);
                }
            }
        }
    }
    //�̵� ��ǲ�ý��� �����Լ�
    public void OnMove()
    {
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");
        moveVec = new Vector3(input.x, 0, input.y).normalized;
        //�̵� ���� �ο� �� ���ϱ�
        isMove = moveVec.magnitude != 0;
    }
    //A���� �Լ�
    public void OnClickAttackA()
    {
        //Ŭ��Ƚ�� ���� ����
        noOfClicks = Mathf.Clamp(noOfClicks, 0, 3);

        //���ݰ��� ������ �� �۵�(=�⺻ �����϶� ��ư ���� �� �۵�)
        if (canAttack)
        {
            attackCollider.SetActive(true);
            //�⺻ A���� �ִϸ��̼� �۵�
            animator.SetTrigger("Attack_A");
            //������ ���� ��
            isAttack = true;
            //���ݰ��� ���� ����(=�⺻���°� �ƴ��� �ǹ���)
            canAttack = false;
            //Ŭ��Ƚ�� ����
            noOfClicks++;
        }
        //�޺����� ������ �� �۵�(=�⺻ ������ �ϰ� ���� ���� Ÿ�̹� �� ��ư ���� �� �۵�)
        if (canCombo)
        {
            //�޺����� ���� ����(��ư�� �������� ���� Ʈ���Ű� �� ������ ���� ����)
            canCombo = false;

            //Ŭ�� Ƚ���� ���� ���� ��� �б�
            switch (noOfClicks)
            {
                //Ŭ��Ƚ���� 1���̾��ٸ� �۵�
                case 1:
                    //���� ���� Ŭ��Ƚ���� 1�̰�, �۵��Ǵ� �ִϸ��̼��� A�϶� ��ư�� �����ٸ� AA����
                    if (animator.GetCurrentAnimatorStateInfo(0).IsName("A"))
                    {
                        animator.SetTrigger("Attack_AA");
                        attackCollider.SetActive(true);
                        //Ŭ��Ƚ�� ����
                        noOfClicks++;
                    }
                    break;
                //Ŭ��Ƚ���� 2���̾��ٸ� �бⰡ ������
                case 2:
                    //���� ���� Ŭ��Ƚ���� 2�̰�, �۵��Ǵ� �ִϸ��̼��� OA�� �� ��ư�� �����ٸ� SSA����
                    if (animator.GetCurrentAnimatorStateInfo(0).IsName("OA"))
                    {
                        animator.SetTrigger("Attack_AAA");
                        attackCollider.SetActive(true);
                    }
                    //���� ���� Ŭ��Ƚ���� 2�̰�, �۵��Ǵ� �ִϸ��̼��� OS�϶� ��ư�� �����ٸ� AAA����
                    else if (animator.GetCurrentAnimatorStateInfo(0).IsName("OS"))
                    {
                        animator.SetTrigger("Attack_SSA");
                        attackCollider.SetActive(true);
                    }
                    break;
                default:
                    break;
            }
        }
    }

    //S �����Լ�
    public void OnClickAttackS()
    {
        //Ŭ��Ƚ�� ���� ����
        noOfClicks = Mathf.Clamp(noOfClicks, 0, 3);

        //���ݰ��� ������ �� �۵�(=�⺻ �����϶� ��ư ���� �� �۵�)
        if (canAttack)
        {
            attackCollider.SetActive(true);
            //�⺻ A���� �ִϸ��̼� �۵�
            animator.SetTrigger("Attack_S");
            //������ ���� ��
            isAttack = true;
            //���ݰ��� ���� ����(=�⺻���°� �ƴ��� �ǹ���)
            canAttack = false;
            //Ŭ��Ƚ�� ����
            noOfClicks++;
        }
        //�޺����� ������ �� �۵�(=�⺻ ������ �ϰ� ���� ���� Ÿ�̹� �� ��ư ���� �� �۵�)
        if (canCombo)
        {
            //�޺����� ���� ����(��ư�� �������� ���� Ʈ���Ű� �� ������ ���� ����)
            canCombo = false;

            //Ŭ�� Ƚ���� ���� ���� ��� �б�
            switch (noOfClicks)
            {
                //Ŭ��Ƚ���� 1���̾��ٸ� SS���� �۵�
                case 1:
                    //���� ���� Ŭ��Ƚ���� 1�̰�, �۵��Ǵ� �ִϸ��̼��� S�϶� ��ư�� �����ٸ� SS����
                    if (animator.GetCurrentAnimatorStateInfo(0).IsName("S"))
                    {
                        animator.SetTrigger("Attack_SS");
                        attackCollider.SetActive(true);
                        //Ŭ��Ƚ�� ����
                        noOfClicks++;
                    }
                    break;
                //Ŭ��Ƚ���� 2���̾��ٸ� �бⰡ ������
                case 2:
                    //���� ���� Ŭ��Ƚ���� 2�̰�, �۵��Ǵ� �ִϸ��̼��� OS�� �� ��ư�� �����ٸ� SSS����
                    if (animator.GetCurrentAnimatorStateInfo(0).IsName("OS"))
                    {
                        animator.SetTrigger("Attack_SSS");
                        attackCollider.SetActive(true);
                    }
                    //���� ���� Ŭ��Ƚ���� 2�̰�, �۵��Ǵ� �ִϸ��̼��� OA�϶� ��ư�� �����ٸ� AAS����
                    else if (animator.GetCurrentAnimatorStateInfo(0).IsName("OA"))
                    {
                        animator.SetTrigger("Attack_AAS");
                        attackCollider.SetActive(true);
                    }
                    break;
                default:
                    break;
            }
        }
    }

    [PunRPC]
    public void TakeDamage(float damageAmount, Collider other)
    {
        health -= damageAmount;
        if(damageAmount == 10)
        {
            //����� ���� ���ؿ���
            float otherAngle = (other.GetComponent<Transform>().transform.rotation.eulerAngles.y);
            //��� ���������� �ڿ������� ��ȯ
            float turnAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, otherAngle, ref turnSmoothVelocity, turnSmoothTime);
            //ȸ�� �ٲٱ�
            transform.rotation = Quaternion.Euler(0f, turnAngle, 0f);

            animator.SetTrigger("GetHit");
        }

        //HP�� 0�� ������ ����
        if (health <= 0)
        {
            health = 0;
            animator.SetBool("IsDead", true);
            Debug.Log("���");


            if (PhotonNetwork.IsMasterClient)
            {
                gameManager.Player1Win();
                Debug.Log("�÷��̾�1Win");
                photonView.RPC("ResetPlayerHealth", RpcTarget.All);
            }
            //���忡�� �˸�
            else
            {
                photonView.RPC("NotifyPlayerDeath", RpcTarget.MasterClient);
                photonView.RPC("ResetPlayerHealth", RpcTarget.All);
            }
            
        }
        // �ٸ� Ŭ���̾�Ʈ���� �������� ����
        photonView.RPC("ApplyDamage", RpcTarget.Others, damageAmount);
    }

    [PunRPC]
    private void NotifyPlayerDeath()
    {
        // ���⼭ attackerID�� ������ �÷��̾��� ID
        gameManager.Player2Win();
        Debug.Log("�÷��̾�2Win");
    }
    [PunRPC]
    private void ResetPlayerHealth()
    {
        StartCoroutine(ResetPlayerHP());
    }
    private IEnumerator ResetPlayerHP()
    {
        yield return new WaitForSeconds(3.0f);

        // health = startHealth;
        Debug.Log("�� �÷��̾� ü�� �ʱ�ȭ");
  
        foreach (MultiPlayer player in FindObjectsOfType<MultiPlayer>())
        {
            player.ResetHP();
        }
    }
    public void ResetHP()
    {
        health = startHealth;
        mp = startMP;
        if (PhotonNetwork.IsMasterClient)
        {
            MasterHealthBar.fillAmount = 100.0f;
            MasterMpBar.fillAmount = 0f;
            animator.SetBool("IsDead", false);
        }
        else
        {
            remoteHealthBar.fillAmount = 100.0f;
            RemoteMpBar.fillAmount = 0f;
            animator.SetBool("IsDead", false);
        }
        // HP�ʱ�ȭ�� ���ÿ� �ʱ� ��ġ�� �̵�
        transform.position = initialPosition;

    }

    [PunRPC] //�ߺ�ȣ�� ������
    private void ApplyDamage(float damageAmount)
    {
        health -= damageAmount;

        if (PhotonNetwork.IsMasterClient)
        {
            MasterHealthBar.fillAmount = health / startHealth;
        }
        else
        {
            remoteHealthBar.fillAmount = health / startHealth;
        }

    }
    [PunRPC]
    public void TakeMp(float damageAmount)
    {
      
        if(!photonView.IsMine)
        {
            return;
        }

        mp += damageAmount;

        if (PhotonNetwork.IsMasterClient)
        {
            MasterMpBar.fillAmount = mp / startHealth;
        }
        else
        {
            RemoteMpBar.fillAmount = mp / startHealth;
        }

        // �ٸ� Ŭ���̾�Ʈ���� �������� �����մϴ�.
        photonView.RPC("ApplyMp", RpcTarget.Others, damageAmount);

    }
    [PunRPC] //�ߺ�ȣ�� ������
    public void ApplyMp(float damageAmount)
    {
        mp += damageAmount;

        if (PhotonNetwork.IsMasterClient)
        {
            MasterMpBar.fillAmount = mp / startHealth;
        }
        else
        {
            RemoteMpBar.fillAmount = mp / startHealth;
        }
    }

    public bool AttackCheck()
    {
        if (isAttack)
        {
            return true;
        }
        return false;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // ���� �÷��̾��� �����͸� �ٸ� �÷��̾�鿡�� �����ϴ�.
            stream.SendNext(health);
            stream.SendNext(mp);
            // �߰��� healthBar�� fillAmount�� ����ȭ�մϴ�.
            stream.SendNext(MasterHealthBar.fillAmount);
            stream.SendNext(remoteHealthBar.fillAmount);
            stream.SendNext(MasterMpBar.fillAmount);
            stream.SendNext(RemoteMpBar.fillAmount);
        }
        else
        {
            // ����Ʈ �÷��̾���� �ٸ� �÷��̾��κ��� �����͸� �޽��ϴ�.
            health = (float)stream.ReceiveNext();
            mp = (float)stream.ReceiveNext();
            // �߰��� healthBar�� fillAmount�� ����ȭ�մϴ�.
            MasterHealthBar.fillAmount = (float)stream.ReceiveNext();
            remoteHealthBar.fillAmount = (float)stream.ReceiveNext();
            MasterMpBar.fillAmount = (float)stream.ReceiveNext();
            RemoteMpBar.fillAmount = (float)stream.ReceiveNext();
        }
    }
    public void HPHeal(float amount)
    {
        health += amount;
        if (health >= startHealth)
            health = startHealth;
    }
}
