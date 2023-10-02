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
            Debug.Log(fsm);
            if (!fsm.SetCurrState(stateData.MapEffectNormal))
            {
                Debug.Log("stateData °¡ null");
            }
            ResetData();
            Debug.Log("run");
            Run();
        }
    public void ResetData()
    {
        if (fsm != null)
        {
            fsm.ChangeState(stateData.MapEffectNormal);
        }
    }
    public void ChangeMapEffect()
    {

    }
    void Run()
    {
         StartCoroutine(OnUpdate());
    }

    IEnumerator OnUpdate()
    {
       Debug.Log(fsm);
        while(true)
        {
             fsm.Update();
             yield return null;
        }
    }
        // Update is called once per frame
}
