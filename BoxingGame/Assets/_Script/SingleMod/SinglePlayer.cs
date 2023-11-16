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
    // Update is called once per frame
    //void Update()
    //{

    //}

}
