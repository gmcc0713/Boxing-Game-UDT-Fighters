using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBG : MonoBehaviour
{
	private Vector3 startPos;
	void Start()
	{
		startPos = new Vector3(68, -2.5f, 0);
	}

	void Update()
	{
		if (transform.position.x < -68)
		{
			transform.position = startPos;
		}
		transform.Translate(-0.05f, 0, 0);
	}

}
