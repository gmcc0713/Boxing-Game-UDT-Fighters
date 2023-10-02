    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class Map : MonoBehaviour
    {
        MapEffectFSM fsm;
        MapEffectStateData stateData = null;
       public void SetData(MapEffectStateData data)
        {
            stateData = data;
            fsm = new MapEffectFSM();
            if (!fsm.SetCurrState(stateData.MapEffectNormal))
            {
                Debug.Log("stateData 가 null");
            }
            ResetData();
            Run();
        }
    public void ResetData()
    {
        if (fsm != null)
        {
            fsm.ChangeState(stateData.MapEffectNormal);
        }
    }
    public void ChangeMapEffect(int type)
    {
        fsm.ChangeState(stateData.IntegerToIstate(type));
    }
    void Run()
    {
         StartCoroutine(OnUpdate());
         StartCoroutine(ChangeMapEffectTimer());
    }

    IEnumerator OnUpdate()
    {
        while(true)
        {
             fsm.Update();
             yield return null;
        }
    }
    public int GetRandomMapEffect()
    {
        int newMapEffect = Random.Range(0, (int)MapEffectType.Count);
        return newMapEffect;
    }
    private IEnumerator ChangeMapEffectTimer()          //30 초마다 새로운 효과
    {
        while (true)
        {
            yield return new WaitForSeconds(3);
            ChangeMapEffect(GetRandomMapEffect());
            Debug.Log("Effect Change");
        }
    }
    // Update is called once per frame
}
