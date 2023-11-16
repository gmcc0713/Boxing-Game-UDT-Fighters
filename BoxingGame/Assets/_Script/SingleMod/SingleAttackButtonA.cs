using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SingleAttackButtonA : MonoBehaviour
{
    Button aButtonSing;
    public SinglePlayer singlePlayerScript;

    // Start is called before the first frame update
    void Start()
    {
        aButtonSing = GetComponent<Button>();
        aButtonSing.onClick.AddListener(OnClickAButtonSing);
    }

    public void OnClickAButtonSing()
    {
        singlePlayerScript.OnAttackAButtonSing();
    }
}
