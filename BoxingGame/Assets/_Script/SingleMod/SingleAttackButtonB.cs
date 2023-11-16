using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SingleAttackButtonB : MonoBehaviour
{
    Button sButtonSing;
    public SinglePlayer singlePlayerScript;

    // Start is called before the first frame update
    void Start()
    {
        sButtonSing = GetComponent<Button>();
        sButtonSing.onClick.AddListener(OnClickSButtonSing);
    }

    public void OnClickSButtonSing()
    {
        singlePlayerScript.OnAttackSButtonSing();
    }
}
