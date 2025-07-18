using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Tower : MapObject
{
    [Header("BuildSetting")]
    public bool IsBuildable;
    public bool IsUnlocked;
    public int TowerIndex;
    [Header("Property")]
    public string TowerName;
    public Price Price;
    [SerializeField] protected float standardHP;
    public float CurrentHealth;
    public float MaxHealth;
    [SerializeField] protected Image hpBar;
    protected Action OnHealthChange;
    [SerializeField] protected Tower upgradedTower;
    public Price UpgradePrice;
    public virtual void Initialize()
    {
        TowerManager.Instance.PlacedTowerList.Add(transform.position);
        MapManager.Instance.DicPosToGTData[transform.position] = new GameTileData(TileType.Tower, this);
        MaxHealth = standardHP;
        CurrentHealth = MaxHealth;
        OnHealthChange += UpdateHealthBar;
        UpdateHealthBar();
    }
    public bool IsUpgradable()
    {
        return upgradedTower != null;
    }
    public virtual void Upgrade()
    {
        if(UpgradePrice != null)
        {
            GameManager.Instance.AffordPrice(UpgradePrice);
            Tower newTower = Instantiate(upgradedTower, transform.position, Quaternion.identity);
            TowerManager.Instance.PlacedTowerList.Remove(transform.position);
            MapManager.Instance.DicPosToGTData[transform.position] = new GameTileData(TileType.Placable, null);
            OnHealthChange -= UpdateHealthBar;
            AudioManager.Instance.Play("TowerUpgrade");
            Destroy(gameObject);
            newTower.Initialize();
        }
    }
    public virtual void SelfDemolish()
    {
        TowerManager.Instance.PlacedTowerList.Remove(transform.position);
        MapManager.Instance.DicPosToGTData[transform.position] = new GameTileData(TileType.Placable, null);
        Die();
    }
    public virtual void Die()
    {
        OnHealthChange -= UpdateHealthBar;
        AudioManager.Instance.Play("Destruction");
        Destroy(gameObject);
    }
    public virtual void TakeDamage(float dmg)
    {
        CurrentHealth -= dmg;
        OnHealthChange?.Invoke();
        AudioManager.Instance.Play("TowerHit");
        if (CurrentHealth <0)SelfDemolish();
    }

    protected virtual void UpdateHealthBar()
    {
        hpBar.fillAmount = CurrentHealth / MaxHealth;
    }
}

[Serializable]
public class Price
{
    public int GoldAmount;
    public int GemAmount;
    public Price(int gold,int gem)
    {
        GoldAmount = gold;
        GemAmount = gem;
    }
}
