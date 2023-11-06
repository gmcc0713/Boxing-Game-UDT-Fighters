using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum EffectImage
{
    Ready=0,
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
		effecters[(int)EffectImage.Ready].StartSizeUp();
		yield return new WaitForSeconds(3.0f);
	}

}
