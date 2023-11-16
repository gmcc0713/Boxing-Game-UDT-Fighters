
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable]
public enum Sound_Type { Sound_BGM = 0, Sound_SFX,Sound_Character,Sound_Type_Count }
public enum BGM_Num { BGM_Title = 0, BGM_Lobby}
public enum SFX_Num { Click_Button = 0, CountDown,Win,Lose}
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    [SerializeField] private AudioSource[] audioSources;
    
    [SerializeField] private Sprite[] audioImage;
    [SerializeField] private AudioClip[] audioBGMClips;
    [SerializeField] private AudioClip[] audioSFXClips;
    [SerializeField] private AudioClip[] audioCharacterClips;
    private bool[] isMute = { false};
    private float[] volumeValue;
    public bool[] _isMute => isMute;
    public float GetVolumValue(Sound_Type type) => volumeValue[(int)type];
    void Start()
    {

        audioSources[(int)Sound_Type.Sound_BGM].clip = audioBGMClips[(int)BGM_Num.BGM_Title];
		audioSources[(int)Sound_Type.Sound_SFX].clip = audioSFXClips[(int)SFX_Num.Click_Button];
		audioSources[(int)Sound_Type.Sound_Character].clip = audioBGMClips[0];
		audioSources[(int)Sound_Type.Sound_BGM].Play();
        volumeValue = new float[3] { 0.5f, 0.5f, 0.5f };


    }
    public void Initialize()
    {
    }
    // Update is called once per frame
    public void PlayAudioClipOneShot(Sound_Type sound_Type,int clip_num)
    {
        switch (sound_Type)
        {
            case Sound_Type.Sound_Character:
                audioSources[(int)sound_Type].PlayOneShot(audioCharacterClips[(int)clip_num]);
                break;
            case Sound_Type.Sound_SFX:
                audioSources[(int)sound_Type].PlayOneShot(audioSFXClips[(int)clip_num]);
                break;
			case Sound_Type.Sound_BGM:
				audioSources[(int)sound_Type].PlayOneShot(audioBGMClips[(int)clip_num]);
				break;
		}
    }
    public void ChangeAudioVolume(Sound_Type sound_Type,float value)
    {
        audioSources[(int)sound_Type].volume = value;
        volumeValue[(int)sound_Type] = value;
    }
    public void StopBGM()
    {
        audioSources[(int)Sound_Type.Sound_BGM].Stop();
    }
    void OnEnable()
    {
        // 씬 매니저의 sceneLoaded에 체인을 건다.
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void Mute(Sound_Type type, bool mute)
    {
        isMute[(int)type] = mute;
	}

    // 체인을 걸어서 이 함수는 매 씬마다 호출된다.
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Initialize();

        if (scene.name == "TitleScene")
        {
            Debug.Log("TitleSceneBGM");
            audioSources[(int)Sound_Type.Sound_BGM].clip = audioBGMClips[(int)BGM_Num.BGM_Title];
        }
        else if(scene.name == "LobbyScene")
        {
            audioSources[(int)Sound_Type.Sound_BGM].clip = audioBGMClips[(int)BGM_Num.BGM_Lobby];
        }

        audioSources[(int)Sound_Type.Sound_BGM].Play();
    }

    void OnDisable()
    {

        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
	public void ClickButton()
	{
		audioSources[(int)Sound_Type.Sound_SFX].Play(0);

	}
	public void PlayCharacterAttackSound(int index)
	{
		audioSources[(int)Sound_Type.Sound_Character].clip = audioCharacterClips[index];
		audioSources[(int)Sound_Type.Sound_Character].Play(0);
	}
}
