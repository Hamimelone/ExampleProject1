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
        // 1. �ڿ������б��в���ƥ�����͵�Ԥ����
        Tower towerPrefab = FindTowerPrefab(Tindex);
        if (towerPrefab == null)
        {
            Debug.LogError($"No tower of index {Tindex} found in available towers!");
            return;
        }
        if (CheckAfford(towerPrefab.Price))
        {
            GameManager.Instance.AffordPrice(towerPrefab.Price);
            // 2. ʵ������
            Tower newTower = Instantiate(towerPrefab, pos, Quaternion.identity, towerParent);

            // 3. ��ʼ����
            newTower.Initialize();
            AudioManager.Instance.Play("Construction");
        }
        ti.HideHUD();
    }

    // �ӿ������б��в���ָ�����͵�Ԥ����
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
