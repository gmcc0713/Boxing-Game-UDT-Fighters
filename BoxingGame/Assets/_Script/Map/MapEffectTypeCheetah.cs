using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapEffectTypeB", menuName = "ScriptableObject/MapEffect/MapEffectTypeB", order = 2)]
public class MapEffectTypeCheetah : MapEffectStateData, IState
{
    public void Enter()
    {
        Debug.Log("B타입 적용");
        Time.timeScale = 2.0f;
        //플레이어의 데미지 줄이기
    }
    public void Excute()
    {
        Debug.Log("B타입 진행");
    
    }
    public void Exit()
    {
        Time.timeScale = 1.0f;
        //플레이어의 데미지 원래대로 만들기
    }
}
