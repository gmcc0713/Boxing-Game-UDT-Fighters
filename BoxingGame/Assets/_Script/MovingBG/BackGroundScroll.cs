using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundScroll : MonoBehaviour
{
	[SerializeField]private Transform []ReapeatBG;
	private float BGOffsetX;
    private float repeatWidth;
	void Start()
    {
		BGOffsetX = ReapeatBG[0].position.x - ReapeatBG[1].position.x;
	}

    void Update()
    {

	}

}
