using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    public void Enter();        //상태시작
    public void Excute();       //상태진행
    public void Exit();         // 상태탈출
}
public class MapEffectStateData : ScriptableObject
{
    public IState MapEffectNormal { get; private set; }
    public IState MapEffectTypeGiant { get; private set; }
    public IState MapEffectTypeCheetah { get; private set; }

    public void SetData(IState Normal,IState Giant, IState Cheetah)
    {
        MapEffectNormal = Normal;
        MapEffectTypeGiant = Giant;
        MapEffectTypeCheetah = Cheetah;
    }
}




