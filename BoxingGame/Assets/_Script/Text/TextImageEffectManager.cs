using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;


public enum EffectImage
{
    Ready = 0,
    Fight,
    K,
    O,
    Count,
}
public class TextImageEffectManager : MonoBehaviour
{
    public UnityEngine.UI.Image[] effectImages;
    public TextImageEffecter[] effecters;

    private MultiPlayer multy;

    public bool canStartReadyFight = true;
    void Start()
    {
        multy = FindObjectOfType<MultiPlayer>();
        UnityEngine.Debug.Log("Start");
    }

    // Update is called once per frame
    void Update()
    {

    }
    IEnumerator GameStartReadyFightImageAnimation()
    {
        //multy.useAttack = false;
        //multy.useMove = false;
        effecters[(int)EffectImage.Ready].gameObject.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        effecters[(int)EffectImage.Fight].gameObject.SetActive(true);
        yield return new WaitForSeconds(1.8f);
        multy.useAttack = true;
        multy.useMove = true;
        Debug.Log(multy.useAttack);

    }
    IEnumerator KOAnimation()
    {
        multy.useAttack = false;
        multy.useMove = false;
        effecters[(int)EffectImage.K].gameObject.SetActive(true);
        yield return new WaitForSeconds(0.7f);
        effecters[(int)EffectImage.O].gameObject.SetActive(true);
        //multy.useAttack = true;
        //multy.useMove = true;
    }
    public void KOTextStart()
    {
        StartCoroutine(KOAnimation());
        StartCoroutine(KOAnimation());
    }
    public void ReadyFightTextStart()
    {
        if (canStartReadyFight)
        {
            StartCoroutine(GameStartReadyFightImageAnimation());
        }
    }
}
