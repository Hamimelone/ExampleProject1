using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private Image gameOverPanel;
    [SerializeField] private Image victoryPanel;
    [SerializeField] private Image settingPanel;

    public void Initialize()
    {
        HideAllPanel();
    }
    public void HideAllPanel()
    {
        gameOverPanel.gameObject.SetActive(false);
        victoryPanel.gameObject.SetActive(false);
        settingPanel.gameObject.SetActive(false);
    }
    public void ShowGameOverPanel()
    {
        gameOverPanel.gameObject.SetActive(true);
    }
    public void ShowVictoryPanel()
    {
        victoryPanel.gameObject.SetActive(true);
    }
    public void ShowSettingPanel()
    {
        settingPanel.gameObject.SetActive(true);
    }
}
