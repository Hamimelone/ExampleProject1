using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VictoryPanel : MonoBehaviour
{
    [SerializeField] private Button menuBtn;
    [SerializeField] private Button restartBtn;

    private void Awake()
    {
        menuBtn.onClick.AddListener(() => SceneManager.LoadScene("MainMenu"));
        restartBtn.onClick.AddListener(NextLevel);
    }

    public void NextLevel()
    {
        if (LevelManager.Instance.CurrentLevelIndex == LevelManager.Instance.levels.Count)
        {
            SceneManager.LoadScene("MainMenu");
            LevelManager.Instance.CurrentLevelIndex = 0;
        }else
            GameManager.Instance.GameStart();
    }
}
