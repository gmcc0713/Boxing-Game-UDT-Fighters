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
            this.GetComponent<Image>().sprite = sprites[1];
        else
			this.GetComponent<Image>().sprite = sprites[0];
    }
    public void MuteImageCheck(bool isMute)
    {
        if(isMute)
        {
            Debug.Log(isMute);
			this.GetComponent<Image>().sprite = sprites[1];
		}
        
    }
}
