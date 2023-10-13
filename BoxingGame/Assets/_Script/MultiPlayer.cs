using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;
using UnityEngine.UI;

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

    public float damage;

    [Header("Health")]
    public float startHealth = 100;
    private float health;

    public Image MasterHealthBar;
    public Image remoteHealthBar;
    public GameObject MasterCanvas;
    public GameObject RemoteCanvas;
    [SerializeField] private Skill skill;
   
    private PhotonView pv;
    void Start()
    {
        animator = GetComponent<Animator>();
        pv = GetComponent<PhotonView>();
        
        health = startHealth;
        if (PhotonNetwork.IsMasterClient)
        {
            // ������ Ŭ���̾�Ʈ�̹Ƿ� ���� ����� UI�� �Ҵ��մϴ�.
            MasterHealthBar = transform.Find("MasterCanvas/Master/MasterHP").GetComponent<Image>();
            MasterHealthBar.fillAmount = health / startHealth;
            if (pv.IsMine)
            {
                // ���� �÷��̾ �ڽ��� ��ü�� �ƴ� ���, RemoteCanvas�� ��Ȱ��ȭ�մϴ�.
                RemoteCanvas.SetActive(false);
            }
            else
            {
                // ���� �÷��̾ �ڽ��� ��ü�� �ƴϸ�, MasterCanvas�� ��Ȱ��ȭ�մϴ�.
                MasterCanvas.SetActive(false);
            }
        }
        else
        {
            // �Ϲ� Ŭ���̾�Ʈ�̹Ƿ� ������ ����� UI�� �Ҵ��մϴ�.
            remoteHealthBar = transform.Find("RemoteCanvas/Remote/RemoteHP").GetComponent<Image>();
            remoteHealthBar.fillAmount = health / startHealth;
            if (!pv.IsMine)
            {
                // ���� �÷��̾ �ڽ��� ��ü�� �ƴϸ� RemoteCanvas�� ��Ȱ��ȭ�մϴ�.
                RemoteCanvas.SetActive(false);
            }
            else
            {
                // ���� �÷��̾ �ڽ��� ��ü�� , MasterCanvas�� ��Ȱ��ȭ�մϴ�.
                MasterCanvas.SetActive(false);
            }
        }
        skill.Initilize(this);
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

        OnMove();
        OnAttack();
        Move();
    }
    void OnAttack()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("A press");
            OnAttackAButton();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("S press");
            OnAttackSButton();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            Debug.Log("D press");
            skill.SkillUse();
        }
    }
    private void Move()
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
    public void OnAttackAButton()
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
    public void OnAttackSButton()
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
    public void TakeDamage(float damageAmount, int attackerID)
    {
        health -= damageAmount;

        if (health <= 0)
        {
            Die();
        }
        // �ٸ� Ŭ���̾�Ʈ���� �������� �����մϴ�.
        photonView.RPC("ApplyDamage", RpcTarget.Others, damageAmount, attackerID);
    }

    [PunRPC] //�ߺ�ȣ�� ������
    private void ApplyDamage(float damageAmount, int attackerID)
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

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("���");
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
            // �߰��� healthBar�� fillAmount�� ����ȭ�մϴ�.
            stream.SendNext(MasterHealthBar.fillAmount);
            stream.SendNext(remoteHealthBar.fillAmount);
        }
        else
        {
            // ����Ʈ �÷��̾���� �ٸ� �÷��̾��κ��� �����͸� �޽��ϴ�.
            health = (float)stream.ReceiveNext();
            // �߰��� healthBar�� fillAmount�� ����ȭ�մϴ�.
            MasterHealthBar.fillAmount = (float)stream.ReceiveNext();
            remoteHealthBar.fillAmount = (float)stream.ReceiveNext();
        }
    }
}