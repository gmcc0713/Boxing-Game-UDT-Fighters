//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//using Photon.Pun;

//public class PlayerHealth : LivingEntity
//{
//    public Slider healthSlider; //체력을 표시할 UI 슬라이더

//    private PlayerController player;

//    private void Awake()
//    {
//        player = GetComponent<PlayerController>();
//    }

//    protected override void OnEnable()
//    {
//        //LivingEntity의 OnEnable()실행(상태 초기화)
//        base.OnEnable();
//        healthSlider.gameObject.SetActive(true);
//        //체력 슬라이더의 최댓값을 기본 체력값으로 변경
//        healthSlider.maxValue = startingHealth;
//        //체력 슬라이더의 값을 현재 체력값으로 변경
//        healthSlider.value = health;

//        //플레이어 조작을 받는 컴포넌트 활성화
//        player.enabled = true;
//    }
//    //체력회복 < 이건 추후에
//    //데미지 처리
//    [PunRPC]
//    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
//    {
//        //LivingEntity의 OnDamage()실행(데미지 적용)
//        base.OnDamage(damage, hitPoint, hitNormal);
//        //갱신된 체력을 체력 슬라이더에 반영
//        healthSlider.value = health;
//    }
//    //사망처리
//    public override void Die()
//    {
//        //LivingEntity의 Die()실행(사망 적용)
//        base.Die();
//        //체력슬라이더 비활성화
//        healthSlider.gameObject.SetActive(false);

//        //플레이어 조작을 받는 컴포넌트 비할성화
//        player.enabled = false;
//    }

//}
