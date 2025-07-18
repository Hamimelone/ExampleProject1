using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TowerManager : Singleton<TowerManager>
{
    public List<Tower> availableTowers = new List<Tower>();
    public List<Vector2>PlacedTowerList = new List<Vector2>();
    [SerializeField] private TileIndicator ti;
    [SerializeField] private Transform towerParent;
    public void Initialize()
    {
        PlacedTowerList.Clear();
        GameObjectExtensions.DestroyAllChildren(towerParent.gameObject);
    }
    public void PlaceTowerOn(int Tindex ,Vector2 pos) 
    {
        // 1. 在可用塔列表中查找匹配类型的预制体
        Tower towerPrefab = FindTowerPrefab(Tindex);
        if (towerPrefab == null)
        {
            Debug.LogError($"No tower of index {Tindex} found in available towers!");
            return;
        }
        if (CheckAfford(towerPrefab.Price))
        {
            GameManager.Instance.AffordPrice(towerPrefab.Price);
            // 2. 实例化塔
            Tower newTower = Instantiate(towerPrefab, pos, Quaternion.identity, towerParent);

            // 3. 初始化塔
            newTower.Initialize();
            AudioManager.Instance.Play("Construction");
        }
        ti.HideHUD();
    }

    // 从可用塔列表中查找指定类型的预制体
    private Tower FindTowerPrefab(int Tindex)
    {
        foreach (Tower tower in availableTowers)
        {
            if (tower.TowerIndex == Tindex)
            {
                return tower;
            }
        }
        return null;
    }

    public bool CheckAfford(Price p)
    {
        return (GameManager.Instance.Gold >= p.GoldAmount && GameManager.Instance.Gem >= p.GemAmount);
    }
}
