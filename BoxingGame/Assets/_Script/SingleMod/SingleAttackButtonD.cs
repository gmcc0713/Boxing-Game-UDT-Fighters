using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SingleAttackButtonD : MonoBehaviour
{
    Button dButtonSing;
    public SinglePlayer singlePlayerScript;

    // Start is called before the first frame update
    void Start()
    {
        dButtonSing = GetComponent<Button>();
        dButtonSing.onClick.AddListener(OnClickDButtonSing);
    }

    public void OnClickDButtonSing()
    {
        Debug.Log("skill");
        //singlePlayerScript.OnAttackSButtonSing();
    }
}
