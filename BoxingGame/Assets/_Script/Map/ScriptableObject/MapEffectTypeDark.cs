using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapEffectTypeDark", menuName = "ScriptableObject/MapEffect/MapEffectTypeDark", order = 1)]
public class MapEffectTypeDark : MapEffectStateData, IState
{
    public void Enter()
    {
        Debug.Log("어둠 타입 적용");
        //캐릭터에게 어둠 효과 적용
    }
    public void Execute()
    {
        Debug.Log("어둠 타입 진행");
    }
    public void Exit()
    {
        //캐릭터 크기 조정 되돌리기
        //공격력 증가 되돌리기
    }
}
