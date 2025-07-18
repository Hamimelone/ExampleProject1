using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : Singleton <MainMenu>
{
    [SerializeField] private Button gameStartButton;

    private void Start()
    {
        gameStartButton.onClick.AddListener(() =>SceneManager.LoadScene("GamingScene"));
    }
}
