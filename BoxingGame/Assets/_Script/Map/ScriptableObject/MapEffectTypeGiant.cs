using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapEffectTypeGiant", menuName = "ScriptableObject/MapEffect/MapEffectTypeGiant", order = 1)]
public class MapEffectTypeGiant : MapEffectStateData, IState
{
    public void Enter()
    {
        Debug.Log("거인 타입 적용");
        //캐릭터 크기 조정
        //공격력 증가
    }
    public void Execute()
    {
        Debug.Log("거인 타입 진행");
    }
    public void Exit()
    {
        //캐릭터 크기 조정 되돌리기
        //공격력 증가 되돌리기
    }
}
