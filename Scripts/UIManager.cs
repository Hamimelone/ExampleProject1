using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private Image gameOverPanel;
    [SerializeField] private Image victoryPanel;
    [SerializeField] private Image settingPanel;
    [SerializeField] private Image textBackground;
    [SerializeField] private TextMeshProUGUI displayText;

    public void Initialize()
    {
        HideAllPanel();
    }
    public void HideAllPanel()
    {
        gameOverPanel.gameObject.SetActive(false);
        victoryPanel.gameObject.SetActive(false);
        settingPanel.gameObject.SetActive(false);
        textBackground.gameObject.SetActive(false);
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
    public void ShowText(string txt,float duration)
    {
        textBackground.gameObject.SetActive(true);
        displayText.text = txt;
        StartCoroutine(HideTextAfter(duration));
    }
    IEnumerator HideTextAfter(float duration)
    {
        yield return new WaitForSeconds(duration);
        textBackground.gameObject.SetActive(false);
    }
}
