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
    // Update is called once per frame
    //void Update()
    //{

    //}

}
