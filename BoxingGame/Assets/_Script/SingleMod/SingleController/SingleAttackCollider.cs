using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleAttackCollider : MonoBehaviour
{
    public SinglePlayer playerSing;
    public ParticleSystem attacked;

    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
            attacked.Play();
    }
}
