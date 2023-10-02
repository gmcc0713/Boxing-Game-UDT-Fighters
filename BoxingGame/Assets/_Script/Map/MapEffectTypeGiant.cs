using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapEffectTypeA", menuName = "ScriptableObject/MapEffect/MapEffectTypeA", order = 1)]
public class MapEffectTypeGiant : MapEffectStateData, IState
{
    public void Enter()
    {
        Debug.Log("A타입 적용");
        //캐릭터 크기 조정
        //공격력 증가
    }
    public void Excute()
    {
        Debug.Log("A타입 진행");
    }
    public void Exit()
    {
        //캐릭터 크기 조정 되돌리기
        //공격력 증가 되돌리기
    }
}
