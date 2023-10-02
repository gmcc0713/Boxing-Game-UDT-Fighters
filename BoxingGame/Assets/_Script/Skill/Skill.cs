using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : MonoBehaviour
{
    // Start is called before the first frame update
    public MultiPlayer playerController;
    public void Initilize(MultiPlayer player)
    {
        playerController = player;
    }
    public bool CanSkillUse()
    {
        return true;
    }
    public abstract void SkillUse();
}
