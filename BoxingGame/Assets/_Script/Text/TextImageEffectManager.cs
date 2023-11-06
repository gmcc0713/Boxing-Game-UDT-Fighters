using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public enum EffectImage
{
	Ready = 0,
	Fight,
	KO,
	Count,
}
public class TextImageEffectManager : MonoBehaviour
{
	public Image[] effectImages;
	public TextImageEffecter[] effecters;
	void Start()
	{
		Debug.Log("Start");
		StartCoroutine(GameStartReadyFightImageAnimation());
	}

	// Update is called once per frame
	void Update()
	{

	}
	IEnumerator GameStartReadyFightImageAnimation()
	{
		Debug.Log("Start");
		effecters[(int)EffectImage.Ready].gameObject.SetActive(true);
		yield return new WaitForSeconds(3.0f);
	}

}
