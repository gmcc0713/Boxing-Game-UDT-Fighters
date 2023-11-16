using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public abstract class SingleSkill : MonoBehaviourPunCallbacks
{
    public SinglePlayer playerControllerSing;
    public void InitilizeSing(SinglePlayer player)
    {
        playerControllerSing = player;
    }
    public bool CanSkillUseSing()
    {
        return true;
    }
    public abstract void SkillUseSing();
}
