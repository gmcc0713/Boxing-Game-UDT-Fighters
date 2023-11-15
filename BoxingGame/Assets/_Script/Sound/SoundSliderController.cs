using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundSliderController : MonoBehaviour
{
    [SerializeField] private Sound_Type soundType;
	[SerializeField] private ClickButtonManager clickBtnManger;
    private Slider soundSlider;
    private float priviousVolume;
    private bool isMute;
	private void Awake()
	{
        isMute = false;
	}
	private void Start()
	{
		isMute = SoundManager.Instance._isMute[(int)soundType];
		MuteSetting();
	}
	void OnEnable()
    {
        soundSlider = gameObject.GetComponent<Slider>();
        SetValue();
    }
    public void onChange()
    {
        if(isMute)
        {
			SoundManager.Instance.ChangeAudioVolume(soundType,0);
            return;
		}
		SoundManager.Instance.ChangeAudioVolume(soundType, soundSlider.value);
	}
    public void SetValue()
    {
		if (isMute)
		{
			soundSlider.value = priviousVolume ;
			return;
		}
		soundSlider.value = SoundManager.Instance.GetVolumValue(soundType);
	}
	public void MuteSetting()
	{
		if(isMute)
		{
			priviousVolume = 0.5f;
			soundSlider.interactable = false;
		}
		else
		{
			priviousVolume = 0.5f;
			soundSlider.interactable = true;
		}

	}
    public void MuteOrListenVolume()
    {
        if(isMute)
        {
			soundSlider.interactable = true;
            isMute = false;
			SoundManager.Instance.ChangeAudioVolume(soundType, priviousVolume);
			SoundManager.Instance.Mute(soundType,false);
			return;
        }
		isMute = true;
		priviousVolume = soundSlider.value;
        soundSlider.interactable = false;
		SoundManager.Instance.ChangeAudioVolume(soundType, 0);
		SoundManager.Instance.Mute(soundType,true);
	}

}
