using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveScene : MonoBehaviour
{
    public void moveSingleScene()
    {
        SceneManager.LoadScene("SingleScene");
    }

    public void moveTitleScene()
    {
        SceneManager.LoadScene("TitleScene");
    }
}
