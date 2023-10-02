using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapEffectTypeCheetah", menuName = "ScriptableObject/MapEffect/MapEffectTypeCheetah", order = 2)]
public class MapEffectTypeCheetah : MapEffectStateData, IState
{
    public void Enter()
    {
        Debug.Log("치타 타입 적용");
        //빨라지기
        Time.timeScale = 2.0f;
        //플레이어의 데미지 줄이기
    }
    public void Execute()
    {
        Debug.Log("치타 타입 진행");
    
    }
    public void Exit()
    {
        Time.timeScale = 1.0f;
        //플레이어의 속도 원래대로 만들기
    }
}
