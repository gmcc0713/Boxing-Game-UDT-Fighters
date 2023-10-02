using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "MapEffectTypeNormal", menuName = "ScriptableObject/MapEffect/MapEffectTypeNormal", order = 1)]
public class MapEffectTypeNormal : MapEffectStateData, IState
{
    
    public void Enter()
    {
        Debug.Log("Normal타입 적용");
    }

    public void Execute()
    {
    }

    public void Exit()
    {

    }
}
