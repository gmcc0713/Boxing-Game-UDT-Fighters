using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinglePlayer : MonoBehaviour
{
    public GameObject plyConSing;
    //애니메이션 컨트롤러 변수 선언
    public Animator animatorSing;
    //이동관련 평면벡터 변수 선언
    Vector2 inputSing;
    //이동 중 회전관련 삼차원벡터 변수 선언
    Vector3 moveVecSing;
    //이동속력 변수 선언
    [SerializeField] private float speedSing = 6;
    //회전시간 변수 선언
    public float turnSmoothTimeSing = 0.1f;
    //회전속력 변수 선언
    private float turnSmoothVelocitySing;
    //움직임 판정 부울 변수 선언
    private bool isMoveSing;

    //공격중 판정 부울 변수 선언
    public bool isAttackSing = false;
    //공격가능 판정 부울 변수 선언
    public bool canAttackSing = true;
    //콤보가능 판정 부울 변수 선언
    public bool canComboSing = false;
    //공격 중지 판정 부울 변수 선언
    public bool useAttackSing = true;
    //움직임 중지 판정 부울 변수 선언
    public bool useMoveSing = true;
    //공격 범위
    public GameObject attackColliderSing;
    //공격 콤보 변수 선언
    public int attackComboSing = 0;
    //클릭 횟수 스테틱 변수 선언
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
            //이동 값 구하기
            Vector3 movementSing = new Vector3(inputDirectionSing.x, 0f, inputDirectionSing.y).normalized;

            //이동 판정 부울 값 구하기
            bool isMobileMoveSing = movementSing.magnitude != 0;

            animatorSing.SetBool("IsRun", isMobileMoveSing);

            //공격상태가 아닐 시 이동
            if (!isAttackSing)
            {
                //캐릭터의 벡터 구하기
                //이동 판정 부울 값이 참일 시 움직임 시작
                if (isMobileMoveSing)
                {
                    //보는 방향 앵글 XZ 값
                    float targetAngle = Mathf.Atan2(inputDirectionSing.x, inputDirectionSing.y) * Mathf.Rad2Deg;
                    //부드러운 움직임 유도식
                    float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocitySing, turnSmoothTimeSing);
                    //회전 바꾸기
                    transform.rotation = Quaternion.Euler(0f, angle, 0f);
                }

                //이동하기
                transform.position += movementSing * speedSing * Time.deltaTime;
                //움직임 애니메이션 작동

            }
        }
    }

    public void IdleSing()
    {
        //조건이 기본상태일 때 작동
        if (animatorSing.GetCurrentAnimatorStateInfo(0).IsName("Idle") && isAttackSing)
        {
            attackComboSing = 0;
            //클릭횟수 초기화
            noOfClicksSing = 0;
            //공격중 상태 거짓
            isAttackSing = false;
            //공격 가능 상태 참
            canAttackSing = true;
            //콤보 가능 상태 거짓
            canComboSing = false;

            //콜라이더 선언
            CapsuleCollider collider = gameObject.GetComponent<CapsuleCollider>();
            //콜라이더 반지름
            collider.radius = 0.4f;
            //콜라이더 중심
            collider.center = new Vector3(0, 1, 0f);
        }

        //조건이 기본상태가 아니고, 이동 상태도 아닐 때 작동(= 공격중일때)
        else if (!(animatorSing.GetCurrentAnimatorStateInfo(0).IsName("Run") && noOfClicksSing == 2))
        {
            //진행도가 0.5이상 되면 상태 변화(=콤보가능 상태 조작)
            if (animatorSing.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.5f)
            {
                //콤보가능 상태 참
                canComboSing = true;

                //진행도가 0.7이상 되면 상태 변화(= 콤보가능 상태 불가로 조작)
                if (animatorSing.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f)
                {
                    //콤보 가능 상태 거짓
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
            //클릿횟수 범위 제한
            noOfClicksSing = Mathf.Clamp(noOfClicksSing, 0, 3);

            //공격가능 상태일 시 작동(=기본 상태일때 버튼 누를 시 작동)
            if (canAttackSing)
            {

                animatorSing.SetTrigger("Attack_A");
                //공격중 상태 참
                isAttackSing = true;
                //공격가능 상태 거짓(=기본상태가 아님을 의미함)
                canAttackSing = false;
                //클릭횟수 증가
                noOfClicksSing++;
            }
            //콤보가능 상태일 시 작동(=기본 공격을 하고 난후 일정 타이밍 때 버튼 누를 시 작동)
            if (canComboSing)
            {
                //콤보가능 상태 거짓(버튼을 연속으로 눌러 트리거가 또 켜지는 현상 방지)
                canComboSing = false;

                //클릭 횟수에 따른 공격 모션 분기
                switch (noOfClicksSing)
                {
                    //클릭횟수가 1번이었다면 작동
                    case 1:
                        //현재 누적 클릭횟수가 1이고, 작동되는 애니메이션이 A일때 버튼을 눌렀다면 AA진행
                        if (animatorSing.GetCurrentAnimatorStateInfo(0).IsName("A"))
                        {
                            animatorSing.SetTrigger("Attack_AA");
                            StartCoroutine(WaitForAttackEndSing(0.5f));
                            //클릭횟수 증가
                            noOfClicksSing++;
                        }
                        break;
                    //클릭횟수가 2번이었다면 분기가 갈라짐
                    case 2:
                        //현재 누적 클릭횟수가 2이고, 작동되는 애니메이션이 OA일 때 버튼을 눌렀다면 SSA진행
                        if (animatorSing.GetCurrentAnimatorStateInfo(0).IsName("OA"))
                        {
                            animatorSing.SetTrigger("Attack_AAA");
                        }
                        //현재 누적 클릭횟수가 2이고, 작동되는 애니메이션이 OS일때 버튼을 눌렀다면 AAA진행
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

    //S 공격함수
    public void OnAttackSButtonSing()
    {
        if (useAttackSing)
        {
            //클릭횟수 범위 제한
            noOfClicksSing = Mathf.Clamp(noOfClicksSing, 0, 3);

            //공격가능 상태일 시 작동(=기본 상태일때 버튼 누를 시 작동)
            if (canAttackSing)
            {
                //기본 A공격 애니메이션 작동
                animatorSing.SetTrigger("Attack_S");
                //공격중 상태 참
                isAttackSing = true;
                //공격가능 상태 거짓(=기본상태가 아님을 의미함)
                canAttackSing = false;
                //클릭횟수 증가
                noOfClicksSing++;
            }
            //콤보가능 상태일 시 작동(=기본 공격을 하고 난후 일정 타이밍 때 버튼 누를 시 작동)
            if (canComboSing)
            {
                //콤보가능 상태 거짓(버튼을 연속으로 눌러 트리거가 또 켜지는 현상 방지)
                canComboSing = false;

                //클릭 횟수에 따른 공격 모션 분기
                switch (noOfClicksSing)
                {
                    //클릭횟수가 1번이었다면 SS공격 작동
                    case 1:
                        //현재 누적 클릭횟수가 1이고, 작동되는 애니메이션이 S일때 버튼을 눌렀다면 SS진행
                        if (animatorSing.GetCurrentAnimatorStateInfo(0).IsName("S"))
                        {
                            animatorSing.SetTrigger("Attack_SS");
                            //클릭횟수 증가
                            noOfClicksSing++;
                        }
                        break;
                    //클릭횟수가 2번이었다면 분기가 갈라짐
                    case 2:
                        //현재 누적 클릭횟수가 2이고, 작동되는 애니메이션이 OS일 때 버튼을 눌렀다면 SSS진행
                        if (animatorSing.GetCurrentAnimatorStateInfo(0).IsName("OS"))
                        {
                            animatorSing.SetTrigger("Attack_SSS");
                        }
                        //현재 누적 클릭횟수가 2이고, 작동되는 애니메이션이 OA일때 버튼을 눌렀다면 AAS진행
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
