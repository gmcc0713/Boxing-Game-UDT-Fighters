using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    public void Enter();        //상태시작
    public void Execute();       //상태진행
    public void Exit();         // 상태탈출
}
public class MapEffectStateData : ScriptableObject
{
    public IState MapEffectNormal { get; private set; }
    public IState MapEffectTypeGiant { get; private set; }
    public IState MapEffectTypeCheetah { get; private set; }
    public IState MapEffectTypeDark { get; private set; }

    public void SetData(IState Normal,IState Giant, IState Cheetah, IState Dark)
    {
        MapEffectNormal = Normal;
        MapEffectTypeGiant = Giant;
        MapEffectTypeCheetah = Cheetah;
        MapEffectTypeDark = Dark;
    }
    public IState IntegerToIstate(int i)
    {
        switch (i)
        {
            case 0:
                return MapEffectNormal;

            case 1:
                return MapEffectTypeGiant;
            case 2:
                return MapEffectTypeCheetah;
            case 3:
                return MapEffectTypeDark;
            default:
                return MapEffectNormal;
        }

    }
}




