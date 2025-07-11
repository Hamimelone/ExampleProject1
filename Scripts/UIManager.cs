using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Setting Window")]
    [SerializeField] private Button settingBtn;
    [SerializeField] private Image settingWindow;
    [SerializeField] private Button settingWindowExitBtn;
    [SerializeField] private Button settingWindowCancelBtn;
    [Header("GameOver Window")]
    [SerializeField] private Image GameOverWindow;
    [SerializeField] private Text GOScore;
    [SerializeField] private Button GORestartBtn;
    [SerializeField] protected Button GOExitBtn;
    #region 单例
    public static UIManager Instance;

    private void Awake()
    {
        if (Instance != null)        //单例模式
        {
            Destroy(gameObject);
        }
        else
            Instance = this;
    }
    #endregion


    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        settingBtn.onClick.AddListener(ShowSettingWindow);
        settingWindowCancelBtn.onClick.AddListener(HideSettingWindow);
        settingWindow.gameObject.SetActive(false);

        GameOverWindow.gameObject.SetActive(false);
    }
    public void ShowSettingWindow()
    {
        settingWindow.gameObject.SetActive(true);
    }

    public void HideSettingWindow()
    {
        settingWindow.gameObject.SetActive(false);
    }

    public void ShowGameOverWindow()
    {
        GameOverWindow.gameObject.SetActive(true);
        GOScore.text = GameManager.Instance.Score.ToString();
    }
}
