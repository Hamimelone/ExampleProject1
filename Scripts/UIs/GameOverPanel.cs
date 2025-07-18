using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverPanel : MonoBehaviour
{
    [SerializeField] private Button menuBtn;
    [SerializeField] private Button restartBtn;

    private void Awake()
    {
        menuBtn.onClick.AddListener(()=> SceneManager.LoadScene("MainMenu"));
        restartBtn.onClick.AddListener(() => GameManager.Instance.GameStart());
    }
}
