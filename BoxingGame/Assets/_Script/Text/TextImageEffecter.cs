using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextImageEffecter : MonoBehaviour
{
	private float size = 1f; //원하는 사이즈
	public float speed; //커질 때의 속도

	private float time;
	private Vector2 originScale; //원래 크기

	private void Awake()
	{
		originScale = transform.localScale; //원래 크기 저장
	}
	private void OnEnable()
	{

	}
	private IEnumerator Up()
	{
		while (transform.localScale.x < size)
		{
			Debug.Log("W");
			transform.localScale = originScale * (1f + time * speed);
			time += Time.deltaTime;

			if (transform.localScale.x >= size)
			{
				Debug.Log("End");
				time = 0;
				break;
			}
			yield return null;
		}
	}
	public void StartSizeUp()
	{
		StartCoroutine(Up());
	}
	private void OnDisable()
	{
		gameObject.transform.localScale = originScale;
	}
}
