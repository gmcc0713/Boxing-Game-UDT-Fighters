//using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    //애니메이션 컨트롤러 변수 선언
    Animator animator;
    //이동관련 평면벡터 변수 선언
    Vector2 input;
    //이동 중 회전관련 삼차원벡터 변수 선언
    Vector3 moveVec;
    //이동속력 변수 선언
    [SerializeField] float speed;
    //회전시간 변수 선언
    public float turnSmoothTime = 0.1f;
    //회전속력 변수 선언
    private float turnSmoothVelocity;
    //움직임 판정 부울 변수 선언
    private bool isMove;

    //공격중 판정 부울 변수 선언
    public bool isAttack = false;
    //공격가능 판정 부울 변수 선언
    public bool canAttack = true;
    //콤보가능 판정 부울 변수 선언
    public bool canCombo = false;
    //공격 범위
    public GameObject attackCollider;
    //공격 콤보 변수 선언
    public int attackCombo = 0;
    //클릭 횟수 스테틱 변수 선언
    public static int noOfClicks = 0;

   // private GameObject targetEnemy;
    public float damage = 10;

    //public Collider attackcol;

    //초기 생명값
    //private readonly float initHp = 100.0f;
    ////현재 생명값
    //public float currHp;
    ////HpBar 연결할 변수
    //private Image hpBar;

    [Header("Health")]
    public float startHealth = 100;
    private float health;
    public Image healthBar;
    //private PhotonView pv;
    void Start()
    {
        animator = GetComponent<Animator>();
        //currHp = startingHealth;
        //healthSlider.value = currHp;
        //hpBar = GameObject.FindGameObjectWithTag("HP_BAR")?.GetComponent<Image>();
        //currHp = initHp;
        health = startHealth;
        healthBar.fillAmount = health / startHealth;
        //DisplayHealth();
        
    }
    private void FixedUpdate()
    {
        //픽시드 업데이트시 상태 체크하기
        Idle();
        
    }
    void Update()
    {
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
    }
    //이동 함수
    private void Move()
    {
        OnMove();
        //공격상태가 아닐 시 이동
        if (!isAttack)
        {
            //캐릭터의 벡터 구하기
            //이동 판정 부울 값이 참일 시 움직임 시작
            if (isMove)
            {
                //보는 방향 앵글 XZ 값
                float targetAngle = Mathf.Atan2(moveVec.x, moveVec.z) * Mathf.Rad2Deg;
                //부드러운 움직임 유도식
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                //회전 바꾸기
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
            }

            //이동하기
            transform.position += moveVec * speed * Time.deltaTime;

        }
        //Debug.Log(isMove);
        //움직임 애니메이션 작동
        animator.SetBool("IsRun", isMove);
    }
    //상태체크 함수
    public void Idle()
    {
        //조건이 기본상태일 때 작동
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            //진행도가 0.2보다 작을 시 상태 변화(= 기본상태로 전환)
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.2f)
            {
                attackCombo = 0;

                //클릭횟수 초기화
                noOfClicks = 0;
                //공격중 상태 거짓
                isAttack = false;
                //공격 가능 상태 참
                canAttack = true;
                //콤보 가능 상태 거짓
                canCombo = false;

                //콜라이더 선언
                CapsuleCollider collider = gameObject.GetComponent<CapsuleCollider>();
                //콜라이더 반지름
                collider.radius = 0.4f;
                //콜라이더 중심
                collider.center = new Vector3(0, 1, 0f);
            }
        }

        //조건이 기본상태가 아니고, 이동 상태도 아닐 때 작동(= 공격중일때)
        else if (!(animator.GetCurrentAnimatorStateInfo(0).IsName("Run") && noOfClicks == 2))
        {
            //진행도가 0.5이상 되면 상태 변화(=콤보가능 상태 조작)
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.5f)
            {
                //콤보가능 상태 참
                canCombo = true;

                //진행도가 0.7이상 되면 상태 변화(= 콤보가능 상태 불가로 조작)
                if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f)
                {
                    //콤보 가능 상태 거짓
                    canCombo = false;
                    attackCollider.SetActive(false);
                }
            }
        }
    }

    public void OnMove()
    {
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");
        moveVec = new Vector3(input.x, 0, input.y).normalized;
        //이동 판정 부울 값 구하기
        isMove = moveVec.magnitude != 0;
    }
    //A공격 함수
    public void OnAttackAButton()
    {
        //클릿횟수 범위 제한
        noOfClicks = Mathf.Clamp(noOfClicks, 0, 3);

        //공격가능 상태일 시 작동(=기본 상태일때 버튼 누를 시 작동)
        if (canAttack)
        {
            attackCollider.SetActive(true);
                        
            //기본 A공격 애니메이션 작동
            animator.SetTrigger("Attack_A");
            //공격중 상태 참
            isAttack = true;
            //공격가능 상태 거짓(=기본상태가 아님을 의미함)
            canAttack = false;
            //클릭횟수 증가
            noOfClicks++;
           
        }
        //콤보가능 상태일 시 작동(=기본 공격을 하고 난후 일정 타이밍 때 버튼 누를 시 작동)
        if (canCombo)
        {
            //콤보가능 상태 거짓(버튼을 연속으로 눌러 트리거가 또 켜지는 현상 방지)
            canCombo = false;

            //클릭 횟수에 따른 공격 모션 분기
            switch (noOfClicks)
            {
                //클릭횟수가 1번이었다면 작동
                case 1:
                    //현재 누적 클릭횟수가 1이고, 작동되는 애니메이션이 A일때 버튼을 눌렀다면 AA진행
                    if (animator.GetCurrentAnimatorStateInfo(0).IsName("A"))
                    {
                        animator.SetTrigger("Attack_AA");
                        attackCollider.SetActive(true);
                        //클릭횟수 증가
                        noOfClicks++;
                    }
                    break;
                //클릭횟수가 2번이었다면 분기가 갈라짐
                case 2:
                    //현재 누적 클릭횟수가 2이고, 작동되는 애니메이션이 OA일 때 버튼을 눌렀다면 SSA진행
                    if (animator.GetCurrentAnimatorStateInfo(0).IsName("OA"))
                    {
                        animator.SetTrigger("Attack_AAA");
                        attackCollider.SetActive(true);
                    }
                    //현재 누적 클릭횟수가 2이고, 작동되는 애니메이션이 OS일때 버튼을 눌렀다면 AAA진행
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


    //S 공격함수
    public void OnAttackSButton()
    {
        //클릭횟수 범위 제한
        noOfClicks = Mathf.Clamp(noOfClicks, 0, 3);

        //공격가능 상태일 시 작동(=기본 상태일때 버튼 누를 시 작동)
        if (canAttack)
        {
            attackCollider.SetActive(true);
            //기본 A공격 애니메이션 작동
            animator.SetTrigger("Attack_S");
            //공격중 상태 참
            isAttack = true;
            //공격가능 상태 거짓(=기본상태가 아님을 의미함)
            canAttack = false;
            //클릭횟수 증가
            noOfClicks++;
        }
        //콤보가능 상태일 시 작동(=기본 공격을 하고 난후 일정 타이밍 때 버튼 누를 시 작동)
        if (canCombo)
        {
            //콤보가능 상태 거짓(버튼을 연속으로 눌러 트리거가 또 켜지는 현상 방지)
            canCombo = false;

            //클릭 횟수에 따른 공격 모션 분기
            switch (noOfClicks)
            {
                //클릭횟수가 1번이었다면 SS공격 작동
                case 1:
                    //현재 누적 클릭횟수가 1이고, 작동되는 애니메이션이 S일때 버튼을 눌렀다면 SS진행
                    if (animator.GetCurrentAnimatorStateInfo(0).IsName("S"))
                    {
                        animator.SetTrigger("Attack_SS");
                        attackCollider.SetActive(true);
                        //클릭횟수 증가
                        noOfClicks++;
                    }
                    break;
                //클릭횟수가 2번이었다면 분기가 갈라짐
                case 2:
                    //현재 누적 클릭횟수가 2이고, 작동되는 애니메이션이 OS일 때 버튼을 눌렀다면 SSS진행
                    if (animator.GetCurrentAnimatorStateInfo(0).IsName("OS"))
                    {
                        animator.SetTrigger("Attack_SSS");
                        attackCollider.SetActive(true);
                    }
                    //현재 누적 클릭횟수가 2이고, 작동되는 애니메이션이 OA일때 버튼을 눌렀다면 AAS진행
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
    //void OnTriggerEnter(Collider coll)
    //{
    //    if(currHp >= 0.0f && coll.CompareTag("AttackCollider"))
    //    {
    //        currHp -= 10.0f;
    //        DisplayHealth();
    //        Debug.Log($"Player hp = {currHp / initHp}");
    //        if (currHp <= 0.0f)
    //        {
    //            PlayerDie();
    //        }
    //    }
    //}
    //private void AttackOn()
    //{
    //    attackCollider.SetActive(true);
    //}

    //private void OnTriggerEnter(Collider other)
    //{

    //    if (other.CompareTag("Player"))
    //    {
    //        Enemy enemy = other.GetComponent<Enemy>();
    //        if (enemy != null)
    //        {
    //                // TakeDamage 함수를 실행하여 적 오브젝트의 HP를 깎음
    //            enemy.TakeDamage(damage);
    //        }
    //    }
        
    //}


    //public void TakeDamage(float damageAmount)
    //{
    //    health -= damageAmount;
    //    healthBar.fillAmount = health / startHealth;
    //    //DisplayHealth();

    //    if (health <= 0)
    //    {
    //        PlayerDie();
    //    }
    //}
    //void PlayerDie()
    //{
    //    Debug.Log("사망");
    //}
   
    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("AttackCollider"))
    //    {
    //        PlayerController controller = other.GetComponent<PlayerController>();

    //        if (controller.AttackCheck())
    //        {
    //            controller.TakeDamage(10f);
    //        }
    //    }
    //}

    //private void //OnCollisionStay(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("AttackCollider"))
    //    {
    //        PlayerController controller = collision.gameObject.GetComponent<PlayerController>();

    //        if (controller.AttackCheck())
    //        {
    //            controller.TakeDamage(10f);
    //        }
    //    }
    //}
    //public void TakeDamage(float damageAmount)
    //{
    //    currHp -= damageAmount;
    //    healthSlider.value = currHp / startingHealth; // HP 바 업데이트
    //    //if (currHp <= 0)
    //    //{
    //    //    //Die(); // HP가 0 이하인 경우 처리할 로직
    //    //}
    //}

    public bool AttackCheck()
    {
        if (isAttack)
        {
            return true;
        }
        return false;
    }
}