using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage;
    public Vector3 lookForward;
    private void Start()
    {
       
    }
    public void StartSkill(Vector3 skillLookForward)
    {
        lookForward = skillLookForward;
        StartCoroutine(Shoot());
    }
    IEnumerator Shoot() 
    {
        while (true) 
        {
            transform.Translate(lookForward * 5 * Time.deltaTime);
            yield return null;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(this);
            //데미지 주기

        }
        //c충돌했을때
    }
}
