using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public enum Effect_Type
{
	Bigger,
	BiggerStop,
	Smaller,
	Bounce,
}

public class TextImageEffecter : MonoBehaviour
{
	float time;
	public float _upSizeTime = 0.2f;
	public float _size = 5;

	[SerializeField] Effect_Type type; 
	// Update is called once per frame
	void BiggerStop()
	{
		if (time > 0.7f)
		{
			StartCoroutine(WaitDisable());
		}
		else
		{
			transform.localScale = Vector3.one * (2 + time);
			time += Time.deltaTime;
		}
	}

	void Bigger()
	{
		transform.localScale = Vector3.one * (2 + time);
		time += Time.deltaTime;
		if (time > 0.5f)
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
	void Bounce()
	{
		if(time <= _upSizeTime)
		{
			transform.localScale = Vector3.one * (1 + _size * time);
		}
		else if(time <= _upSizeTime*2)
		{
			transform.localScale = Vector3.one * (2 * _size * _upSizeTime + 1 - time * _size);
		}
		else
		{
			transform.localScale = Vector3.one;
		}
		time += Time.deltaTime;
	}
	void Update()
	{
		switch (type)
		{
			case Effect_Type.BiggerStop:
				BiggerStop();
				break;
			case Effect_Type.Bigger:
				Bigger();
				break;
			case Effect_Type.Smaller:
				Smaller();
				break;
			case Effect_Type.Bounce:
				Bounce();
				break;
		}
	}

	IEnumerator WaitDisable()
	{
		yield return new WaitForSeconds(1.0f);
		gameObject.SetActive(false);
		yield return new WaitForSeconds(1.0f);
	}

	public void resetAnim()
	{
		time = 0;
		transform.localScale = Vector3.one;
	}

}
