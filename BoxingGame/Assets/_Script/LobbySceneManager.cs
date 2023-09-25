using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbySceneManager : MonoBehaviour
{
    [SerializeField] Button Player1Button;
    [SerializeField] Button Player2Button;
    void Start()
    {
        Player1Button.interactable = false;
        Player2Button.interactable = false;
    }

}
