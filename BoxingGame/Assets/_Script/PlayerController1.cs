using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController1 : MonoBehaviour
{
    //�÷��̾���Ʈ�ѷ� �ν��Ͻ� ����
    public static PlayerController1 Instance { get; private set; }

    //�÷��̾���Ʈ�ѷ� �̱������� ����
    void Awake()
    {
        if (null == Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }
        Destroy(gameObject);
    }

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

	//���� �޺� ���� ����
	public int attackCombo = 0;
    //Ŭ�� Ƚ�� ����ƽ ���� ����
    public static int noOfClicks = 0;

    public float damage;

    void Start()
    {
        //���۽� ���� �ִ� �ִϸ����� ��������
        animator = GetComponent<Animator>();
    }
    private void FixedUpdate()
    {
        //�Ƚõ� ������Ʈ�� ���� üũ�ϱ�
        Idle();
    }
    void Update()
    {
        //������Ʈ�� �̵� üũ�ϱ�
        Move();
    }

    //�̵� �Լ�
    private void Move()
    {
        //���ݻ��°� �ƴ� �� �̵�
        if(!isAttack)
        {
            //ĳ������ ���� ���ϱ�
            moveVec = new Vector3(input.x, 0, input.y).normalized;
            //�̵� ���� �ο� �� ���ϱ�
            isMove = moveVec.magnitude != 0;
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

            //������ �ִϸ��̼� �۵�
            animator.SetBool("IsRun", isMove);
        }
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
            if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.5f)
            {
                //�޺����� ���� ��
                canCombo = true;

                //���൵�� 0.7�̻� �Ǹ� ���� ��ȭ(= �޺����� ���� �Ұ��� ����)
                if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f)
                {
                    //�޺� ���� ���� ����
                    canCombo = false;
                }
			}
		}
    }

    //�̵� ��ǲ�ý��� �����Լ�
    public void OnMove(InputAction.CallbackContext context)
    {
        input = context.ReadValue<Vector2>();
    }

    //A���� �Լ�
    public void OnAttackAButton(InputAction.CallbackContext context)
    {
        //Ŭ��Ƚ�� ���� ����
		noOfClicks = Mathf.Clamp(noOfClicks, 0, 3);

        //���ݰ��� ������ �� �۵�(=�⺻ �����϶� ��ư ���� �� �۵�)
		if (canAttack)
        {
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
        if(canCombo)
        {
            //�޺����� ���� ����(��ư�� �������� ���� Ʈ���Ű� �� ������ ���� ����)
            canCombo = false;

            //Ŭ�� Ƚ���� ���� ���� ��� �б�
            switch (noOfClicks)
            {
                //Ŭ��Ƚ���� 1���̾��ٸ� �۵�
                case 1:
                    //���� ���� Ŭ��Ƚ���� 1�̰�, �۵��Ǵ� �ִϸ��̼��� A�϶� ��ư�� �����ٸ� AA����
                    if(animator.GetCurrentAnimatorStateInfo(0).IsName("A"))
                    {
                        animator.SetTrigger("Attack_AA");
                        //Ŭ��Ƚ�� ����
                        noOfClicks++;
                    }
                    break;
                //Ŭ��Ƚ���� 2���̾��ٸ� �бⰡ ������
                case 2:
                    //���� ���� Ŭ��Ƚ���� 2�̰�, �۵��Ǵ� �ִϸ��̼��� OA�� �� ��ư�� �����ٸ� SSA����
                    if(animator.GetCurrentAnimatorStateInfo(0).IsName("OA"))
                    {
						animator.SetTrigger("Attack_AAA");
					}
                    //���� ���� Ŭ��Ƚ���� 2�̰�, �۵��Ǵ� �ִϸ��̼��� OS�϶� ��ư�� �����ٸ� AAA����
                    else if(animator.GetCurrentAnimatorStateInfo(0).IsName("OS"))
                    {
                        animator.SetTrigger("Attack_SSA");
                    }
					break;
                default:
                    break;
            }
        }
    }

    //S �����Լ�
    public void OnAttackSButton(InputAction.CallbackContext context)
    {
        //Ŭ��Ƚ�� ���� ����
        noOfClicks = Mathf.Clamp(noOfClicks, 0, 3);

        //���ݰ��� ������ �� �۵�(=�⺻ �����϶� ��ư ���� �� �۵�)
        if (canAttack)
        {
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
                    }
                    //���� ���� Ŭ��Ƚ���� 2�̰�, �۵��Ǵ� �ִϸ��̼��� OA�϶� ��ư�� �����ٸ� AAS����
                    else if(animator.GetCurrentAnimatorStateInfo(0).IsName("OA"))
                    {
                        animator.SetTrigger("Attack_AAS");
                    }
                    break;
                default:
                    break;
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController controller = collision.gameObject.GetComponent<PlayerController>();

            if (controller.AttackCheck())
            {
            }
        }
    }
    public bool AttackCheck()
    {
        if(isAttack)
        {
            return true;
        }
        return false;
    }
}
