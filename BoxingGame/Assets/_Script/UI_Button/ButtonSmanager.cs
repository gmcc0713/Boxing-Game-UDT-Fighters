using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSmanager : SaveParentPhton
{
    Button sButton;
    public MultiPlayer multiPlayerScript;

    // Start is called before the first frame update
    void Awake()
    {
        sButton = GetComponent<Button>();
        sButton.onClick.AddListener(OnClickSButton);
    }

    private void OnClickSButton()
    {
        //Debug.Log("call S Button Funtion");
        multiPlayerScript.OnAttackSButton();
    }
}
