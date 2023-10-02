using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MapEffectType
{
    Normal = 0,
    Giant,
    Cheetah,
    Dark,
    Count,
}

public class MapEffectManager : MonoBehaviour
{
    [SerializeField]Map map;
    void Awake()
    {
        MapEffectInit();
    }

    private void Update()
    {
        
    }
    public void MapEffectInit()
    {
        IState Normal = (IState)Resources.Load("ScriptableObject/MapEffect/MapEffectTypeNormal");
        IState Giant = (IState)Resources.Load("ScriptableObject/MapEffect/MapEffectTypeGiant");
        IState Dark = (IState)Resources.Load("ScriptableObject/MapEffect/MapEffectTypeDark");
        IState Cheetah = (IState)Resources.Load("ScriptableObject/MapEffect/MapEffectTypeCheetah");
       
        MapEffectStateData data = ScriptableObject.CreateInstance<MapEffectStateData>();

        data.SetData(Normal, Giant, Cheetah, Dark);
        Debug.Log(map);
        map.SetData(data);
    }


}
