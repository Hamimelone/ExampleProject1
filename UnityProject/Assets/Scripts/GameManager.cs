using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    public int Gold;
    public int Gem;
    [SerializeField] private Text goldText;
    [SerializeField] private Text gemText;

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
    }
    public void SpendGem(int amount)
    {
        if(amount <= Gem)
        {
            Gem -= amount;
            gemText.text = Gem.ToString();
        }
    }
    public void AffordPrice(Price p)
    {
        SpendGold(p.GoldAmount);
        SpendGem(p.GemAmount);
    }
    public void GameStart()
    {
        Gold = 100;
        Gem = 0;
        goldText.text = Gold.ToString();
        gemText.text = Gem.ToString();
    }
}
