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
        ChangeMapEffectTimer();
    }

    private void Update()
    {
        
    }
    public void MapEffectInit()
    {
        IState Normal = (IState)Resources.Load("ScriptableObject/MapEffectState/Normal");
        IState A = (IState)Resources.Load("ScriptableObject/MapEffectState/A");
        IState B = (IState)Resources.Load("ScriptableObject/MapEffectState/B");
       
        MapEffectStateData data = ScriptableObject.CreateInstance<MapEffectStateData>();

        data.SetData(Normal, A, B);
        Debug.Log(map);
        map.SetData(data);
    }
    public void MapEffectChange()
    {

    }
    public int GetRandomMapEffect()
    {
        int newMapEffect = Random.Range(0,(int)MapEffectType.Count);
        return newMapEffect;
    }
    private IEnumerator ChangeMapEffectTimer()          //30 초마다 새로운 효과
    {
        Debug.Log("Map Effect Timer change");
        yield return new WaitForSeconds(30);
        Debug.Log("Effect Change");
    }

}
