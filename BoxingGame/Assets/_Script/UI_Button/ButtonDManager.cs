using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonDManager : MonoBehaviour
{
    Button dButton;
    public MultiPlayer multiPlayerScript;

    // Start is called before the first frame update
    void Start()
    {
        dButton = GetComponent<Button>();
        dButton.onClick.AddListener(OnClickDButton);
    }

    public void OnClickDButton()
    {
        Debug.Log("call A Button Funtion");
        multiPlayerScript.OnSkillUse();
    }
}
