using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Text scoreTxt;
    
    public int Score;
    #region 单例
    public static GameManager Instance;

    private void Awake()
    {
        if(Instance != null)        //单例模式
        {
            Destroy(gameObject);
        }else
        Instance = this;
    }
    #endregion

    public void GetScore(bool IsSame,int num,MahjongType mType)
    {
        int score;
        if (IsSame)
        {
            switch (num)
            {
                case 2: score = 10;break;
                case 3: score = 20; break;
                case 4: score = 50; break;
                case 5: score = 100; break;
                case 6: score = 250;break;
                case 7: score = 750;break;
                case 8: score = 2500;break;
                case 9: score = 10000;break;
                default: score = 0; break;
            }
        }
        else
        {
            switch (num)
            {
                case 2:score = 5; break;
                case 3: score = 15; break;
                case 4: score = 40; break;
                case 5: score = 75; break;
                case 6: score = 200; break;
                case 7: score = 500; break;
                case 8: score = 1500; break;
                case 9: score = 5000; break;
                default: score = 0; break;
            }
        }

        if(mType == MahjongType.箭 || mType == MahjongType.风)
        {
            score *= 2;
        }else if(mType == MahjongType.王)
        {
            score *= 5;
        }
        Score += score;
        UpdateScoreText();
    }

    public void UpdateScoreText()
    {
        scoreTxt.text = Score.ToString();
    }

    public void InitializeGame()
    {
        Score = 0;
        UpdateScoreText();
        
    }

    private void Start()
    {
        InitializeGame();
    }

    public void GameOver()
    {
        UIManager.Instance.ShowGameOverWindow();
    }
}
