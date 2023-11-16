using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleMapTriggerEnter : MonoBehaviour
{
    public float getForce; //���� ����

    public Rigidbody singlePlayer; //���� �޴� ���

    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Transform startArrow = other.transform;
            Vector3 lookDirect = (this.transform.position - startArrow.transform.position).normalized; //���� �ִ� ����
            singlePlayer.AddForce(lookDirect * getForce);
            Debug.Log("force");
        }

    }
    // Start is called before the first frame update
    //void Start()
    //{

    //}

    //// Update is called once per frame
    //void Update()
    //{

    //}
}
