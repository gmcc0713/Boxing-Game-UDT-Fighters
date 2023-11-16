using System.Collections;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;


public class MultiPlayer : MonoBehaviourPunCallbacks, IPunObservable
{

    public GameObject plyCon;
    //애니메이션 컨트롤러 변수 선언
    public Animator animator;
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
    //공격 중지 판정 부울 변수 선언
    public bool useAttack = true;
    //움직임 중지 판정 부울 변수 선언
    public bool useMove = true;
    //공격 범위
    public GameObject attackCollider;
    //공격 콤보 변수 선언
    public int attackCombo = 0;
    //클릭 횟수 스테틱 변수 선언
    public int noOfClicks = 0;

    public ParticleSystem playSkill;

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
    public Image MstProfile;
    public Image RmtProfile;
    [SerializeField] private Skill skill;

    private GameManager gameManager;
    private PhotonView pv;
    private TextImageEffectManager textImageEffectManager;
    //초기위치
    private Vector3 initialPosition;
    private Quaternion initialRotation; //초기 방향
    public bool isSkill;
    public bool useReset = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        pv = GetComponent<PhotonView>();

        gameManager = FindObjectOfType<GameManager>();
        textImageEffectManager = FindObjectOfType<TextImageEffectManager>();
        health = startHealth;
        mp = startMP;
        if (PhotonNetwork.IsMasterClient)
        {
            // 마스터 클라이언트이므로 왼쪽 상단의 UI를 할당합니다.
            MasterHealthBar = transform.Find("MasterCanvas/Master/MasterHP").GetComponent<Image>();
            MasterHealthBar.fillAmount = health / startHealth;
            MasterMpBar = transform.Find("MasterMpCanvas/MasterMP/MasterMP").GetComponent<Image>();
            MasterMpBar.fillAmount = mp / startHealth;
            if (pv.IsMine)
            {
                // 현재 플레이어가 자신의 객체면 아닌 경우, RemoteCanvas를 비활성화합니다.
                RemoteCanvas.SetActive(false);
                RemoteMpCanvas.SetActive(false);
                RmtProfile.gameObject.SetActive(false);
            }
            else
            {
                // 현재 플레이어가 자신의 객체가 아니면, MasterCanvas를 비활성화합니다.
                MasterCanvas.SetActive(false);
                MasterMpCanvas.SetActive(false);
                MstProfile.gameObject.SetActive(false);
            }
        }
        else
        {
            // 일반 클라이언트이므로 오른쪽 상단의 UI를 할당합니다.
            remoteHealthBar = transform.Find("RemoteCanvas/Remote/RemoteHP").GetComponent<Image>();
            remoteHealthBar.fillAmount = health / startHealth;
            RemoteMpBar = transform.Find("RemoteMpCanvas/RemoteMP/RemoteMP").GetComponent<Image>();
            RemoteMpBar.fillAmount = mp / startHealth;
            if (!pv.IsMine)
            {
                // 현재 플레이어가 자신의 객체가 아니면 RemoteCanvas를 비활성화합니다.
                RemoteCanvas.SetActive(false);
                RemoteMpCanvas.SetActive(false);
                RmtProfile.gameObject.SetActive(false);
            }
            else
            {
                // 현재 플레이어가 자신의 객체면 , MasterCanvas를 비활성화합니다.
                MasterCanvas.SetActive(false);
                MasterMpCanvas.SetActive(false);
                MstProfile.gameObject.SetActive(false);
            }
        }
        skill.Initilize(this);
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        useAttack = false;
        useMove = false;
        useReset = true;
        if (!pv.IsMine)
        {
            plyCon.SetActive(false);
        }
    }
    private void FixedUpdate()
    {
        Idle();

        if (!pv.IsMine)
        {
            return;
        }

        //픽시드 업데이트시 상태 체크하기

    }
    void Update()
    {
        if (!pv.IsMine)
        {
            return;
        }

        if (useAttack)
        {
            OnAttack();
        }

        if (useMove)
        {
            Move();
        }
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
            Debug.Log(skill);
            if (mp >= 50)
            {
                animator.SetTrigger("IsSkill");
                photonView.RPC("PlayAttackParticle", RpcTarget.All);

                Debug.Log("스킬게이지 100!");
                skill.SkillUse();
                isSkill = true;

                TakeMp(-50);
                Debug.Log("현재스킬게이지" + mp);
            }
        }
    }
    //모바일 전용 스킬 함수
    public void OnSkillUse()
    {
        if (useAttack)
        {
            if (mp >= 50)
            {
                animator.SetBool("IsSkill", true);
                StartCoroutine(WaitForAttackEnd(1.0f));
                playSkill.Play();

                Debug.Log("스킬게이지 100!");
                skill.SkillUse();

                TakeMp(-50);
                Debug.Log("현재스킬게이지" + mp);
            }
        }
    }
    [PunRPC]
    public void PlayAttackParticle()
    {
        playSkill.Play();
    }
    public void SkillEnd()
    {
        animator.SetBool("IsSkill", false);
        isSkill = false;
    }
    public void MobieMove(Vector2 inputDirection)
    {
        if (useAttack)
        {
            //이동 값 구하기
            Vector3 movement = new Vector3(inputDirection.x, 0f, inputDirection.y).normalized;

            //이동 판정 부울 값 구하기
            bool isMobileMove = movement.magnitude != 0;

            animator.SetBool("IsRun", isMobileMove);

            //공격상태가 아닐 시 이동
            if (!isAttack)
            {
                //캐릭터의 벡터 구하기
                //이동 판정 부울 값이 참일 시 움직임 시작
                if (isMobileMove)
                {
                    //보는 방향 앵글 XZ 값
                    float targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.y) * Mathf.Rad2Deg;
                    //부드러운 움직임 유도식
                    float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                    //회전 바꾸기
                    transform.rotation = Quaternion.Euler(0f, angle, 0f);
                }

                //이동하기
                transform.position += movement * speed * Time.deltaTime;
                //움직임 애니메이션 작동

            }
        }
    }

    public void Move()
    {
        //이동 값 구하기
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");
        moveVec = new Vector3(input.x, 0, input.y).normalized;

        //이동 판정 부울 값 구하기
        isMove = moveVec.magnitude != 0;

        //움직임 애니메이션 작동
        animator.SetBool("IsRun", isMove);

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
        else
        {
            //transform.position += moveVec * speed/4 * Time.deltaTime;
        }
    }

    //상태체크 함수
    public void Idle()
    {
        //조건이 기본상태일 때 작동
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") && isAttack)
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
    //이동 인풋시스템 적용함수
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
        if (useAttack)
        {
            //클릿횟수 범위 제한
            noOfClicks = Mathf.Clamp(noOfClicks, 0, 3);

            //공격가능 상태일 시 작동(=기본 상태일때 버튼 누를 시 작동)
            if (canAttack)
            {

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
                            StartCoroutine(WaitForAttackEnd(0.5f));
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
                        }
                        //현재 누적 클릭횟수가 2이고, 작동되는 애니메이션이 OS일때 버튼을 눌렀다면 AAA진행
                        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("OS"))
                        {

                            animator.SetTrigger("Attack_SSA");
                        }
                        break;
                    default:
                        break;
                }
            }
        }
    }

    //S 공격함수
    public void OnAttackSButton()
    {
        if (useAttack)
        {
            //클릭횟수 범위 제한
            noOfClicks = Mathf.Clamp(noOfClicks, 0, 3);

            //공격가능 상태일 시 작동(=기본 상태일때 버튼 누를 시 작동)
            if (canAttack)
            {
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
                        }
                        //현재 누적 클릭횟수가 2이고, 작동되는 애니메이션이 OA일때 버튼을 눌렀다면 AAS진행
                        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("OA"))
                        {
                            animator.SetTrigger("Attack_AAS");
                        }
                        break;
                    default:
                        break;
                }
            }
        }

    }

    [PunRPC]
    public void TakeDamage(float damageAmount, Collider other)
    {
        health -= damageAmount;
        if (damageAmount == 10)
        {
            animator.SetTrigger("GetHit");
            photonView.RPC("SyncAudio", RpcTarget.All);
        }

        //HP가 0이 됐을때 실행
        if (health <= 0)
        {
            health = 0;
            //animator.SetBool("IsDead", true);
            //photonView.RPC("DeadAni", RpcTarget.All);
            Debug.Log("사망");

            foreach (MultiPlayer player in FindObjectsOfType<MultiPlayer>())
            {
                player.useAttack = false;
                player.useMove = false;
                //textImageEffectManager.KOTextStart();
            }

            if (PhotonNetwork.IsMasterClient)
            {
                gameManager.Player1Win();
                Debug.Log("플레이어1Win");
                //animator.SetBool("IsWin", true);
                photonView.RPC("ResetPlayerHealth", RpcTarget.All);
            }
            //방장에게 알림
            else
            {
                photonView.RPC("NotifyPlayerDeath", RpcTarget.MasterClient);
                photonView.RPC("ResetPlayerHealth", RpcTarget.All);
            }

        }
        // 다른 클라이언트에도 데미지를 적용
        photonView.RPC("ApplyDamage", RpcTarget.Others, damageAmount);
    }

    [PunRPC]
    public void SyncAudio()
    {
        SoundManager.Instance.PlayCharacterAttackSound(0);
    }


    [PunRPC]
    private void NotifyPlayerDeath()
    {
        // 여기서 attackerID는 공격한 플레이어의 ID
        gameManager.Player2Win();
        //animator.SetBool("IsWin", true);
        Debug.Log("플레이어2Win");
    }
   
    [PunRPC]
    private void ResetPlayerHealth()
    {
        
            StartCoroutine(ResetPlayerHP());
       
        
    }
    private IEnumerator ResetPlayerHP()
    {
        yield return new WaitForSeconds(6.0f);

        // health = startHealth;
        Debug.Log("전 플레이어 체력 초기화");

        foreach (MultiPlayer player in FindObjectsOfType<MultiPlayer>())
        {
            if(useReset != false)
            {
                player.ResetHP();
            }
           
            //textImageEffectManager.ReadyFightTextStart();
            //player.useAttack = true;
            //player.useMove = true;

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
                //photonView.RPC("DeadAni", RpcTarget.All);
                //animator.SetBool("IsDead", false);
                //animator.SetBool("IsWin", false);
                //gameManager.ChangeRoundImage();
            }
            else
            {
                remoteHealthBar.fillAmount = 100.0f;
                RemoteMpBar.fillAmount = 0f;
                //photonView.RPC("DeadAni", RpcTarget.All);
                //animator.SetBool("IsDead", false);
                //animator.SetBool("IsWin", false);
                //gameManager.ChangeRoundImage();
            }
        if (useReset != false)
        {
            transform.position = initialPosition;
            transform.rotation = initialRotation;
        }
          
        
     
        // HP초기화와 동시에 초기 위치로 이동
        //animator.SetBool("IsDead", false);
    }
    //[PunRPC]
    //public void DeadAni()
    //{
    //    if (health <= 0)
    //        animator.SetBool("IsDead", true);
    //    //else
    //    //    animator.SetBool("IsDead", false);
    //}

    [PunRPC] //중복호출 방지용
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

        if (!photonView.IsMine)
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

        // 다른 클라이언트에도 데미지를 적용합니다.
        photonView.RPC("ApplyMp", RpcTarget.Others, damageAmount);

    }
    [PunRPC] //중복호출 방지용
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
            // 로컬 플레이어의 데이터를 다른 플레이어들에게 보냅니다.
            stream.SendNext(health);
            stream.SendNext(mp);
            // 추가로 healthBar의 fillAmount도 동기화합니다.
            stream.SendNext(MasterHealthBar.fillAmount);
            stream.SendNext(remoteHealthBar.fillAmount);
            stream.SendNext(MasterMpBar.fillAmount);
            stream.SendNext(RemoteMpBar.fillAmount);
        }
        else
        {
            // 리모트 플레이어들은 다른 플레이어들로부터 데이터를 받습니다.
            health = (float)stream.ReceiveNext();
            mp = (float)stream.ReceiveNext();
            // 추가로 healthBar의 fillAmount도 동기화합니다.
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
        ApplyDamage(-amount);
    }
    IEnumerator WaitForAttackEnd(float time)
    {
        yield return new WaitForSeconds(time);
        attackCollider.SetActive(false);
    }
}
