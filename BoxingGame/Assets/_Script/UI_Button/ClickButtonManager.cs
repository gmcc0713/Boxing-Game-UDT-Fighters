using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickButtonManager : MonoBehaviour
{
    public Sprite[] sprites;
    public bool isClicked = false;
    public GameObject buttonSprite;

    // Start is called before the first frame update
    public void BtnClick()
    {
        isClicked =!isClicked;
        if(isClicked)
            buttonSprite.GetComponent<Image>().sprite = sprites[1];
        else
            buttonSprite.GetComponent<Image>().sprite = sprites[0];
    }
}
