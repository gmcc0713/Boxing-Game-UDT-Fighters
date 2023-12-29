using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundSliderController : MonoBehaviour
{
	[SerializeField] private Sound_Type soundType;
	[SerializeField] GameObject[] muteImage;
	[SerializeField] private ClickButtonManager clickBtnManger;
	private Slider soundSlider;
	[SerializeField] private float priviousVolume;
	private bool isMute;
	private void Awake()
	{
		isMute = false;
	}
	private void Start()
	{
		soundSlider = gameObject.GetComponent<Slider>();
		Initialized();
	}
	private void Initialized()
	{
		Debug.Log("Init");
		isMute = SoundManager.Instance._isMute[(int)soundType];
		soundSlider.value = SoundManager.Instance.GetVolumValue(soundType);
		priviousVolume = soundSlider.value;
		if (isMute)
		{
			soundSlider.interactable = false;
		}
	}
	public void ChangeVolume()
	{
		SoundManager.Instance.ChangeAudioVolume(soundType,soundSlider.value);
	}
	public void SetValue()
	{
		if (isMute)
		{
			soundSlider.value = priviousVolume;
			return;
		}
		soundSlider.value = SoundManager.Instance.GetVolumValue(soundType);
	}

	public void MuteOrListenVolume()
	{
		Debug.Log("Initmute");
		if (isMute)
		{
			soundSlider.interactable = true;
			isMute = false;
			SoundManager.Instance.ChangeAudioVolume(soundType, priviousVolume);
			SoundManager.Instance.Mute(soundType, false);
			muteImage[0].SetActive(true);
			muteImage[1].SetActive(false);
			return;
		}
		isMute = true;
		priviousVolume = soundSlider.value;
		soundSlider.interactable = false;
		SoundManager.Instance.Mute(soundType, true);
		SoundManager.Instance.ChangeAudioVolume(soundType, 0);
		muteImage[0].SetActive(false);
		muteImage[1].SetActive(true);
	}

}
