using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage;
    public float rotateSpeed;
    public Vector3 lookForward;
    public void StartSkill(Vector3 skillLookForward)
    {
        lookForward = skillLookForward;
        StartCoroutine(Shoot());
    }
	private void Update()
	{
		transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);
	}
	IEnumerator Shoot() 
    {
        while (true) 
        {
            transform.Translate(lookForward * 5 * Time.deltaTime);
		    transform.Rotate(Vector3.up*rotateSpeed * Time.deltaTime);

            yield return null;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(this);


        }
        //c충돌했을때
    }
}
