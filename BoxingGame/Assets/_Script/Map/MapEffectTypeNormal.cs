using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEffectTypeNormal : MapEffectStateData, IState
{
    
    public void Enter()
    {
        Debug.Log("Normal타입 적용");
    }

    public void Excute()
    {
        Debug.Log("Normal타입 진행");
    }

    public void Exit()
    {

    }
}
