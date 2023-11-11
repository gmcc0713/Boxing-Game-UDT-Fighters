using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonAManager : SaveParentPhton
{
    Button aButton;
    public MultiPlayer multiPlayerScript;

    // Start is called before the first frame update
    void Awake()
    {
        aButton = GetComponent<Button>();
        aButton.onClick.AddListener(OnClickAButton);
    }

    private void OnClickAButton()
    {
        //Debug.Log("call A Button Funtion");
        multiPlayerScript.OnAttackAButton();
    }
}
