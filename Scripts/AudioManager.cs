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
    //[Header("音频混合器")]
    //[SerializeField] private AudioMixer audioMixer; // 从Project面板拖入你的主音频混合器
    //private const string MASTER_VOLUME_PARAM = "MasterVolume";// 音量参数名称（必须与AudioMixer中的参数一致）
    [Header("UI元素")]
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private TextMeshProUGUI volumePercentageText; // 可选：显示百分比文本
    [Header("音效设置")]
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
        // 初始化滑块值
        InitializeSlider();
        // 添加音量改变事件监听
        masterVolumeSlider.onValueChanged.AddListener(SetMasterVolume);
    }
    private void InitializeSlider()
    {
        // 从PlayerPrefs加载保存的音量设置
        //float savedVolume = PlayerPrefs.GetFloat(MASTER_VOLUME_PARAM, 1f);
        float savedVolume = 1f;

        // 设置滑块初始值
        masterVolumeSlider.value = savedVolume;

        // 立即应用音量
        SetMasterVolume(savedVolume);
    }
    public void SetMasterVolume(float volume)
    {
        // 音量范围转换为对数（因为音频混合器使用dB）
        //float volumeDB = volume > 0 ? Mathf.Log10(volume) * 20 : -80;

        // 设置音频混合器参数
        //audioMixer.SetFloat(MASTER_VOLUME_PARAM, volumeDB);
        SetGlobalVolume(volume);

        // 更新百分比显示（可选）
        if (volumePercentageText != null)
        {
            volumePercentageText.text = "Volume : " + Mathf.RoundToInt(volume * 100) + "%";
        }

        // 保存设置
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

        // 限制同时播放的音效数量
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
