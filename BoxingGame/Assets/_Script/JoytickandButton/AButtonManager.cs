using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AButtonManager : MonoBehaviour
{
    Button aButton;
    public MultiPlayer multiPlayerScript;

    // Start is called before the first frame update
    void Start()
    {
        aButton = this.GetComponent<Button>();
        aButton.onClick.AddListener(OnClickAButton);
    }
    
    public void OnClickAButton()
    {
        Debug.Log("call A Button Funtion");
        multiPlayerScript.OnAttackAButton();
    }
}
