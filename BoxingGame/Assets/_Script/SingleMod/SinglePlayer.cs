using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinglePlayer : MonoBehaviour
{
    public GameObject plyConSing;
    //�ִϸ��̼� ��Ʈ�ѷ� ���� ����
    public Animator animatorSing;
    //�̵����� ��麤�� ���� ����
    Vector2 inputSing;
    //�̵� �� ȸ������ ���������� ���� ����
    Vector3 moveVecSing;
    //�̵��ӷ� ���� ����
    [SerializeField] private float speedSing = 6;
    //ȸ���ð� ���� ����
    public float turnSmoothTimeSing = 0.1f;
    //ȸ���ӷ� ���� ����
    private float turnSmoothVelocitySing;
    //������ ���� �ο� ���� ����
    private bool isMoveSing;

    //������ ���� �ο� ���� ����
    public bool isAttackSing = false;
    //���ݰ��� ���� �ο� ���� ����
    public bool canAttackSing = true;
    //�޺����� ���� �ο� ���� ����
    public bool canComboSing = false;
    //���� ���� ���� �ο� ���� ����
    public bool useAttackSing = true;
    //������ ���� ���� �ο� ���� ����
    public bool useMoveSing = true;
    //���� ����
    public GameObject attackColliderSing;
    //���� �޺� ���� ����
    public int attackComboSing = 0;
    //Ŭ�� Ƚ�� ����ƽ ���� ����
    public int noOfClicksSing = 0;

    public ParticleSystem playSkill;

    // Start is called before the first frame update
    void Start()
    {
        animatorSing = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        IdleSing();
    }

    public void MobieMoveSing(Vector2 inputDirectionSing)
    {
        if (useAttackSing)
        {
            //�̵� �� ���ϱ�
            Vector3 movementSing = new Vector3(inputDirectionSing.x, 0f, inputDirectionSing.y).normalized;

            //�̵� ���� �ο� �� ���ϱ�
            bool isMobileMoveSing = movementSing.magnitude != 0;

            animatorSing.SetBool("IsRun", isMobileMoveSing);

            //���ݻ��°� �ƴ� �� �̵�
            if (!isAttackSing)
            {
                //ĳ������ ���� ���ϱ�
                //�̵� ���� �ο� ���� ���� �� ������ ����
                if (isMobileMoveSing)
                {
                    //���� ���� �ޱ� XZ ��
                    float targetAngle = Mathf.Atan2(inputDirectionSing.x, inputDirectionSing.y) * Mathf.Rad2Deg;
                    //�ε巯�� ������ ������
                    float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocitySing, turnSmoothTimeSing);
                    //ȸ�� �ٲٱ�
                    transform.rotation = Quaternion.Euler(0f, angle, 0f);
                }

                //�̵��ϱ�
                transform.position += movementSing * speedSing * Time.deltaTime;
                //������ �ִϸ��̼� �۵�

            }
        }
    }

    public void IdleSing()
    {
        //������ �⺻������ �� �۵�
        if (animatorSing.GetCurrentAnimatorStateInfo(0).IsName("Idle") && isAttackSing)
        {
            attackComboSing = 0;
            //Ŭ��Ƚ�� �ʱ�ȭ
            noOfClicksSing = 0;
            //������ ���� ����
            isAttackSing = false;
            //���� ���� ���� ��
            canAttackSing = true;
            //�޺� ���� ���� ����
            canComboSing = false;

            //�ݶ��̴� ����
            CapsuleCollider collider = gameObject.GetComponent<CapsuleCollider>();
            //�ݶ��̴� ������
            collider.radius = 0.4f;
            //�ݶ��̴� �߽�
            collider.center = new Vector3(0, 1, 0f);
        }

        //������ �⺻���°� �ƴϰ�, �̵� ���µ� �ƴ� �� �۵�(= �������϶�)
        else if (!(animatorSing.GetCurrentAnimatorStateInfo(0).IsName("Run") && noOfClicksSing == 2))
        {
            //���൵�� 0.5�̻� �Ǹ� ���� ��ȭ(=�޺����� ���� ����)
            if (animatorSing.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.5f)
            {
                //�޺����� ���� ��
                canComboSing = true;

                //���൵�� 0.7�̻� �Ǹ� ���� ��ȭ(= �޺����� ���� �Ұ��� ����)
                if (animatorSing.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f)
                {
                    //�޺� ���� ���� ����
                    canComboSing = false;
                    attackColliderSing.SetActive(false);
                }
            }
        }
    }

    public void OnAttackAButtonSing()
    {
        if (useAttackSing)
        {
            //Ŭ��Ƚ�� ���� ����
            noOfClicksSing = Mathf.Clamp(noOfClicksSing, 0, 3);

            //���ݰ��� ������ �� �۵�(=�⺻ �����϶� ��ư ���� �� �۵�)
            if (canAttackSing)
            {

                animatorSing.SetTrigger("Attack_A");
                //������ ���� ��
                isAttackSing = true;
                //���ݰ��� ���� ����(=�⺻���°� �ƴ��� �ǹ���)
                canAttackSing = false;
                //Ŭ��Ƚ�� ����
                noOfClicksSing++;
            }
            //�޺����� ������ �� �۵�(=�⺻ ������ �ϰ� ���� ���� Ÿ�̹� �� ��ư ���� �� �۵�)
            if (canComboSing)
            {
                //�޺����� ���� ����(��ư�� �������� ���� Ʈ���Ű� �� ������ ���� ����)
                canComboSing = false;

                //Ŭ�� Ƚ���� ���� ���� ��� �б�
                switch (noOfClicksSing)
                {
                    //Ŭ��Ƚ���� 1���̾��ٸ� �۵�
                    case 1:
                        //���� ���� Ŭ��Ƚ���� 1�̰�, �۵��Ǵ� �ִϸ��̼��� A�϶� ��ư�� �����ٸ� AA����
                        if (animatorSing.GetCurrentAnimatorStateInfo(0).IsName("A"))
                        {
                            animatorSing.SetTrigger("Attack_AA");
                            StartCoroutine(WaitForAttackEndSing(0.5f));
                            //Ŭ��Ƚ�� ����
                            noOfClicksSing++;
                        }
                        break;
                    //Ŭ��Ƚ���� 2���̾��ٸ� �бⰡ ������
                    case 2:
                        //���� ���� Ŭ��Ƚ���� 2�̰�, �۵��Ǵ� �ִϸ��̼��� OA�� �� ��ư�� �����ٸ� SSA����
                        if (animatorSing.GetCurrentAnimatorStateInfo(0).IsName("OA"))
                        {
                            animatorSing.SetTrigger("Attack_AAA");
                        }
                        //���� ���� Ŭ��Ƚ���� 2�̰�, �۵��Ǵ� �ִϸ��̼��� OS�϶� ��ư�� �����ٸ� AAA����
                        else if (animatorSing.GetCurrentAnimatorStateInfo(0).IsName("OS"))
                        {

                            animatorSing.SetTrigger("Attack_SSA");
                        }
                        break;
                    default:
                        break;
                }
            }
        }
    }

    //S �����Լ�
    public void OnAttackSButtonSing()
    {
        if (useAttackSing)
        {
            //Ŭ��Ƚ�� ���� ����
            noOfClicksSing = Mathf.Clamp(noOfClicksSing, 0, 3);

            //���ݰ��� ������ �� �۵�(=�⺻ �����϶� ��ư ���� �� �۵�)
            if (canAttackSing)
            {
                //�⺻ A���� �ִϸ��̼� �۵�
                animatorSing.SetTrigger("Attack_S");
                //������ ���� ��
                isAttackSing = true;
                //���ݰ��� ���� ����(=�⺻���°� �ƴ��� �ǹ���)
                canAttackSing = false;
                //Ŭ��Ƚ�� ����
                noOfClicksSing++;
            }
            //�޺����� ������ �� �۵�(=�⺻ ������ �ϰ� ���� ���� Ÿ�̹� �� ��ư ���� �� �۵�)
            if (canComboSing)
            {
                //�޺����� ���� ����(��ư�� �������� ���� Ʈ���Ű� �� ������ ���� ����)
                canComboSing = false;

                //Ŭ�� Ƚ���� ���� ���� ��� �б�
                switch (noOfClicksSing)
                {
                    //Ŭ��Ƚ���� 1���̾��ٸ� SS���� �۵�
                    case 1:
                        //���� ���� Ŭ��Ƚ���� 1�̰�, �۵��Ǵ� �ִϸ��̼��� S�϶� ��ư�� �����ٸ� SS����
                        if (animatorSing.GetCurrentAnimatorStateInfo(0).IsName("S"))
                        {
                            animatorSing.SetTrigger("Attack_SS");
                            //Ŭ��Ƚ�� ����
                            noOfClicksSing++;
                        }
                        break;
                    //Ŭ��Ƚ���� 2���̾��ٸ� �бⰡ ������
                    case 2:
                        //���� ���� Ŭ��Ƚ���� 2�̰�, �۵��Ǵ� �ִϸ��̼��� OS�� �� ��ư�� �����ٸ� SSS����
                        if (animatorSing.GetCurrentAnimatorStateInfo(0).IsName("OS"))
                        {
                            animatorSing.SetTrigger("Attack_SSS");
                        }
                        //���� ���� Ŭ��Ƚ���� 2�̰�, �۵��Ǵ� �ִϸ��̼��� OA�϶� ��ư�� �����ٸ� AAS����
                        else if (animatorSing.GetCurrentAnimatorStateInfo(0).IsName("OA"))
                        {
                            animatorSing.SetTrigger("Attack_AAS");
                        }
                        break;
                    default:
                        break;
                }
            }
        }

    }

    IEnumerator WaitForAttackEndSing(float time)
    {
        yield return new WaitForSeconds(time);
        attackColliderSing.SetActive(false);
    }
    // Update is called once per frame
    //void Update()
    //{

    //}

}
