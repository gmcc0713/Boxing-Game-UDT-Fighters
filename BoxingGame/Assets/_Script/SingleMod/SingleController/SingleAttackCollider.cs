using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleAttackCollider : MonoBehaviour
{
    public SingleSendbackManager sendBack;
    public ParticleSystem attacked;

    private void Start()
    {
        sendBack = GameObject.Find("SinglePlayerSendback").GetComponent<SingleSendbackManager>();
    }

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("collider");
        if(other.CompareTag("Player"))
        {
            Debug.Log("attack");
            sendBack.TakeDamageSige();
            attacked.Play();
        }
    }
}
