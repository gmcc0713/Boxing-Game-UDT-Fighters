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
	public UnityEngine.UI.Image[] effectImages;
	public TextImageEffecter[] effecters;
	void Start()
	{
		UnityEngine.Debug.Log("Start");

	}

	// Update is called once per frame
	void Update()
	{

	}
	IEnumerator GameStartReadyFightImageAnimation()
	{
		effecters[(int)EffectImage.Ready].gameObject.SetActive(true);
		yield return new WaitForSeconds(1.3f);
		effecters[(int)EffectImage.Fight].gameObject.SetActive(true);

	}
	public void ReadyFightTextStart()
	{
		StartCoroutine(GameStartReadyFightImageAnimation());
	}
}
