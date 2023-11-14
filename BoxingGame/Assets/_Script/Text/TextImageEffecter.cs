using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public enum Effect_Type
{
	Bigger,
	Smaller,
}

public class TextImageEffecter : MonoBehaviour
{
	float time;
	[SerializeField] Effect_Type type; 
	// Update is called once per frame
	void Bigger()
	{
		transform.localScale = Vector3.one * (2 + time);
		time += Time.deltaTime;
		if (time > 0.7f)
		{
			StartCoroutine(WaitDisable());
		}
	}
	void Smaller()
	{
		transform.localScale = Vector3.one * (1 - time);
		if(time > 1f)
		{
			time = 0;
			gameObject.SetActive(false);
		}
		time += Time.deltaTime;
	}
	void Update()
	{
		switch (type)
		{
			case Effect_Type.Bigger:
				Bigger();
				break;
			case Effect_Type.Smaller:
				Smaller();
				break;

		}
	}
	IEnumerator WaitDisable()
	{
		yield return new WaitForSeconds(0.3f);
		gameObject.SetActive(false);
	}
	public void resetAnim()
	{
		time = 0;
		transform.localScale = Vector3.one;
	}

}
