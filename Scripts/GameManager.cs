using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    public int Gold;
    public int Gem;
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private TextMeshProUGUI gemText;

    private void Start()
    {
        GameStart();
    }
    public void AddGold(int amount)
    {
        Gold += amount;
        goldText.text = Gold.ToString();
    }
    public void SpendGold(int amount)
    {
        if(amount<= Gold)
        {
            Gold -= amount;
            goldText.text = Gold.ToString();
        }
    }
    public void AddGem(int amount)
    {
        Gem += amount;
        gemText.text = Gem.ToString();
        CheckVictory();
    }
    public void SpendGem(int amount)
    {
        if(amount <= Gem)
        {
            Gem -= amount;
            gemText.text = Gem.ToString();
        }
    }
    public bool CanAffordPrice(Price p)
    {
        return Gold>=p.GoldAmount && Gem>=p.GemAmount;
    }
    public void AffordPrice(Price p)
    {
        SpendGold(p.GoldAmount);
        SpendGem(p.GemAmount);
    }
    public void GameStart()
    {
        InitializeGame();
        EffectManager.Instance.InitializeEffect();
        MonsterSpawner.Instance.Initialize();
        AudioManager.Instance.Initialize();
        TowerManager.Instance.Initialize();
        MapManager.Instance.Initialize();
        MapManager.Instance.LoadMap(LevelManager.Instance.levels[LevelManager.Instance.CurrentLevelIndex].LevelMap);
        UIManager.Instance.Initialize();
        GameContinue();
    }
    public void InitializeGame()
    {
        Gold = LevelManager.Instance.levels[LevelManager.Instance.CurrentLevelIndex].initialGold;
        Gem = 0;
        goldText.text = Gold.ToString();
        gemText.text = Gem.ToString();
    }
    public void GamePause()
    {
        Time.timeScale = 0;
    }
    public void GameContinue()
    {
        UIManager.Instance.HideAllPanel();
        Time.timeScale = 1;
    }
    public void GameOver()
    {
        GamePause();
        UIManager.Instance.Initialize();
        UIManager.Instance.ShowGameOverPanel();
    }
    public void ShowVictory()
    {
        GamePause();
        UIManager.Instance.Initialize();
        UIManager.Instance.ShowVictoryPanel();
        LevelManager.Instance.CurrentLevelIndex += 1;
    }
    public void CheckVictory()
    {
        //if(MonsterPool.Instance.CheckMonsterClear() 
        //    && Gem >=100 
        //    && MonsterSpawner.Instance.SpawnFinished)
        //    ShowVictory();
        if(Gem >= 100)
            ShowVictory();
    }

}

public static class GameObjectExtensions
{
    public static void DestroyAllChildren(GameObject parent)
    {
        // 获取所有子物体的Transform组件
        Transform[] children = new Transform[parent.transform.childCount];

        // 先将所有子物体引用存储到数组中
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            children[i] = parent.transform.GetChild(i);
        }

        // 删除所有子物体
        foreach (Transform child in children)
        {
            // 在编辑器中使用DestroyImmediate，在运行时使用Destroy
            if (Application.isPlaying)
            {
                Object.Destroy(child.gameObject);
            }
            else
            {
                Object.DestroyImmediate(child.gameObject);
            }
        }
    }
}