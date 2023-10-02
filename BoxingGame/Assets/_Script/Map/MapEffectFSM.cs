using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MapEffectFSM : MonoBehaviour
{
    IState currState;
    public void Update()
    {
        currState.Excute();
    }
    public bool SetCurrState(IState state)
    {
        if (null == state) return false;
        currState = state;
        return true;
    }
    public bool ChangeState(IState state)
    {
        if (null == state) return false;
        currState.Exit();
        currState = state;
        currState.Enter();
        return true;
    }
}
