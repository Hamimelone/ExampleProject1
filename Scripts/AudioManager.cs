using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [System.Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;
        [Range(0f, 1f)] public float volume = 1f;
        [Range(0.1f, 3f)] public float pitch = 1f;
        public bool loop = false;
        public bool playOnAwake = false;

        [HideInInspector] public AudioSource source;
    }
    //[Header("��Ƶ�����")]
    //[SerializeField] private AudioMixer audioMixer; // ��Project��������������Ƶ�����
    //private const string MASTER_VOLUME_PARAM = "MasterVolume";// �����������ƣ�������AudioMixer�еĲ���һ�£�
    [Header("UIԪ��")]
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private TextMeshProUGUI volumePercentageText; // ��ѡ����ʾ�ٷֱ��ı�
    [Header("��Ч����")]
    [SerializeField] private List<Sound> sounds = new List<Sound>();
    [SerializeField] private int maxSimultaneousSounds = 10;

    private List<AudioSource> activeSources = new List<AudioSource>();

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

    public void Initialize()
    {
        foreach (Sound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
            sound.source.playOnAwake = sound.playOnAwake;
        }
        // ��ʼ������ֵ
        InitializeSlider();
        // ��������ı��¼�����
        masterVolumeSlider.onValueChanged.AddListener(SetMasterVolume);
    }
    private void InitializeSlider()
    {
        // ��PlayerPrefs���ر������������
        //float savedVolume = PlayerPrefs.GetFloat(MASTER_VOLUME_PARAM, 1f);
        float savedVolume = 1f;

        // ���û����ʼֵ
        masterVolumeSlider.value = savedVolume;

        // ����Ӧ������
        SetMasterVolume(savedVolume);
    }
    public void SetMasterVolume(float volume)
    {
        // ������Χת��Ϊ��������Ϊ��Ƶ�����ʹ��dB��
        //float volumeDB = volume > 0 ? Mathf.Log10(volume) * 20 : -80;

        // ������Ƶ���������
        //audioMixer.SetFloat(MASTER_VOLUME_PARAM, volumeDB);
        SetGlobalVolume(volume);

        // ���°ٷֱ���ʾ����ѡ��
        if (volumePercentageText != null)
        {
            volumePercentageText.text = "Volume : " + Mathf.RoundToInt(volume * 100) + "%";
        }

        // ��������
        //PlayerPrefs.SetFloat(MASTER_VOLUME_PARAM, volume);
        //PlayerPrefs.Save();
    }

    IEnumerator StopAfterDuration(string soundName)
    {
        Sound sound = sounds.Find(s => s.name == soundName);
        if (sound == null)
        {
            Debug.LogWarning($"Sound: {soundName} not found!");
            yield return null;
        }
        float duration = sound.clip.length;
        yield return new WaitForSeconds(duration);
        Stop(soundName);
    }

    public void Play(string soundName)
    {
        Sound sound = sounds.Find(s => s.name == soundName);
        if (sound == null)
        {
            Debug.LogWarning($"Sound: {soundName} not found!");
            return;
        }

        // ����ͬʱ���ŵ���Ч����
        if (activeSources.Count >= maxSimultaneousSounds)
        {
            AudioSource oldestSource = activeSources[0];
            oldestSource.Stop();
            activeSources.RemoveAt(0);
        }

        sound.source.Play();
        activeSources.Add(sound.source);
        StartCoroutine(StopAfterDuration(soundName));
    }

    public void Stop(string soundName)
    {
        Sound sound = sounds.Find(s => s.name == soundName);
        if (sound == null)
        {
            Debug.LogWarning($"Sound: {soundName} not found!");
            return;
        }

        sound.source.Stop();
        activeSources.Remove(sound.source);
    }

    public void Pause(string soundName)
    {
        Sound sound = sounds.Find(s => s.name == soundName);
        if (sound == null)
        {
            Debug.LogWarning($"Sound: {soundName} not found!");
            return;
        }

        sound.source.Pause();
    }

    public void Resume(string soundName)
    {
        Sound sound = sounds.Find(s => s.name == soundName);
        if (sound == null)
        {
            Debug.LogWarning($"Sound: {soundName} not found!");
            return;
        }

        sound.source.UnPause();
    }

    public void SetVolume(string soundName, float volume)
    {
        Sound sound = sounds.Find(s => s.name == soundName);
        if (sound == null)
        {
            Debug.LogWarning($"Sound: {soundName} not found!");
            return;
        }

        sound.source.volume = Mathf.Clamp01(volume);
    }

    public void SetGlobalVolume(float volume)
    {
        foreach (Sound sound in sounds)
        {
            sound.source.volume = Mathf.Clamp01(volume) * sound.volume;
        }
    }
}
