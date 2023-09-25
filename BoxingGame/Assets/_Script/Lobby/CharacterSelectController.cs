using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public enum Character
{
    Empty = 0,
    Horse,
    Zombie,
    Count ,
}
public class CharacterSelectController : MonoBehaviour
{
    [SerializeField] private Button leftButton;
    [SerializeField] private Button rightButton;
    [SerializeField] private GameObject[]characters;
    private Character curCharacter;

    private void Start()
    {
        curCharacter = Character.Horse;
        leftButton.interactable = false;
    }
    public void ClickLeftButton()
    {
        if((int)curCharacter > (int)Character.Empty+1)
        {
            rightButton.interactable = true;
            curCharacter--;
            ChangeCharacter((int)curCharacter);
            if((int)curCharacter <= 1)
            {
                leftButton.interactable = false;
            }
        }

        
    }
    public void ClickRightButton()
    {
        if((int)curCharacter < (int)Character.Count-1)
        {
            leftButton.interactable = true;
            curCharacter++;
            ChangeCharacter((int)curCharacter);
            if ((int)curCharacter >= (int)Character.Count - 1)
            {
                rightButton.interactable = false;
            }
        }

    }
    void ChangeCharacter(int curCharacter)
    {
        Debug.Log(curCharacter);
        if (curCharacter == 0)
        {
            return;
        }
        foreach(GameObject c in characters) 
        {
            c.SetActive(false);
        }
        characters[curCharacter].SetActive(true);
    }
}
