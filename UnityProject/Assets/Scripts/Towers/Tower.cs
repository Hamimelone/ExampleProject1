using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public virtual void Initialize()
    {
        TowerManager.Instance.PlacedTowerList.Add(transform.position);
        MapManager.Instance.DicPosToGTData[transform.position] = new GameTileData(TileType.Tower, this);
        MaxHealth = standardHP;
        CurrentHealth = MaxHealth;
    }
    public virtual void Upgrade()
    {

    }
    public virtual void SelfDemolish()
    {
        TowerManager.Instance.PlacedTowerList.Remove(transform.position);
        MapManager.Instance.DicPosToGTData[transform.position] = new GameTileData(TileType.Placable, null);
        Destroy(gameObject);
    }
}

[Serializable]
public class Price
{
    public int GoldAmount;
    public int GemAmount;
}
