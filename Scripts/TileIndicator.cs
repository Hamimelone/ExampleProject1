using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TileIndicator : MonoBehaviour
{
    [SerializeField] private Grid grid;
    public bool IsDisplayingHUD;
    [SerializeField] private Canvas canvas;
    [SerializeField] private TowerButtonPrefab towerBuildBtnPrefab;
    [SerializeField] private Transform buildBtnLayout;
    [SerializeField] private Transform mapObjectBtnLayout;
    [SerializeField] private Button btn_Upgrade;
    [SerializeField] private Button btn_Demolish;
    [SerializeField] private Button btn_Unlock;
    [SerializeField] private GameObject towerRange;
    private MapObject currentMO;
    private bool IsClickable;

    private SpriteRenderer sr;

    private void Start()
    {
        IsDisplayingHUD = false;
        currentMO = null;
        btn_Unlock.onClick.AddListener(() => UnlockSelectedBlock());
        btn_Demolish.onClick.AddListener(() => DemolishSelectedTower());
        btn_Upgrade.onClick.AddListener(() => UpgradeSelectedTower());
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // ���������� UI��ֱ�ӷ���
            if (EventSystem.current.IsPointerOverGameObject()) return;
            HandleMouseDown();
        }
        if (Input.GetMouseButtonDown(1) && IsDisplayingHUD)
        {
            HideHUD();
        }
    }
    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }
    public void HandleMouseDown()
    {
        // ��ȡ���λ�ö�Ӧ�ĸ���
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPosition = grid.WorldToCell(mousePosition);
        Vector2 alignedPosition = cellPosition + grid.cellSize / 2;
        HideHUD();
        DisplayHUDOn(alignedPosition);
    }
    public void HideHUD()
    {
        IsDisplayingHUD = false;
        canvas.gameObject.SetActive(false);
        towerRange.gameObject.SetActive(false);
    }
    public bool CheckPlacable(Vector2 pos)
    {
        if (!MapManager.Instance.DicPosToGTData.ContainsKey(pos) ||
            MapManager.Instance.DicPosToGTData[pos] == null) return false;
        if (MapManager.Instance.DicPosToGTData[pos].gtMO == null)
        {
            TileType tt = MapManager.Instance.DicPosToGTData[pos].gtType;
            if (tt == TileType.Placable || tt == TileType.BuffTile) return true;
            else return false;
        }else return false;
    }
    public bool CheckMapObject(Vector2 pos)
    {
        if(!MapManager.Instance.DicPosToGTData.ContainsKey(pos))return false;
        return (MapManager.Instance.DicPosToGTData[pos] != null && MapManager.Instance.DicPosToGTData[pos].gtMO != null) ;
    }

    public void DisplayHUDOn(Vector2 pos)
    {
        IsDisplayingHUD = true;
        transform.position = pos;
        AudioManager.Instance.Play("TileHover1");
        canvas.gameObject.SetActive(true);
        sr.color = (CheckPlacable(pos)||CheckMapObject(pos)) ? Color.white : Color.red;
        canvas.gameObject.SetActive(true);
        buildBtnLayout.gameObject.SetActive(CheckPlacable(pos));
        if (CheckPlacable(pos)) RefreshBuildBtn(pos);
        mapObjectBtnLayout.gameObject.SetActive(CheckMapObject(pos));
        if (CheckMapObject(pos))
        {
            currentMO = MapManager.Instance.DicPosToGTData[pos].gtMO;
            RefreshTowerBtn(pos);
            if(currentMO is AttackTower at)
            {
                towerRange.gameObject.SetActive(true);
                towerRange.transform.localScale = Vector2.one * at.CurrentAttackRange*2;
            }
        }
    }
    private void RefreshBuildBtn(Vector2 pos)
    {
        foreach (Transform child in buildBtnLayout)
        {
            Destroy(child.gameObject);
        }
        foreach (var t in TowerManager.Instance.availableTowers)
        {
            if(t.IsBuildable && t.IsUnlocked)
            {
                TowerButtonPrefab tb = Instantiate(towerBuildBtnPrefab, buildBtnLayout);
                tb.ChildIcon.sprite = t.GetComponent<SpriteRenderer>().sprite;
                tb.GetComponent<Button>().onClick.AddListener(()=>TowerManager.Instance.PlaceTowerOn(t.TowerIndex,pos));
            }
        }
    }
    private void RefreshTowerBtn(Vector2 pos)
    {
        btn_Unlock.gameObject.SetActive(MapManager.Instance.DicPosToGTData[pos].gtMO is LockedGate);
        btn_Demolish.gameObject.SetActive(MapManager.Instance.DicPosToGTData[pos].gtMO is Tower);
        btn_Upgrade.gameObject.SetActive(MapManager.Instance.DicPosToGTData[pos].gtMO is Tower t && t.IsUpgradable());
    }
    public void UnlockSelectedBlock()
    {
        if(currentMO!=null && currentMO is LockedGate lg)
        {
            if (GameManager.Instance.Gold >= 250)
            {
                GameManager.Instance.SpendGold(250);
                lg.UnlockSelf();
                HideHUD();
            }
        }
    }
    public void DemolishSelectedTower()
    {
        if(currentMO != null && currentMO is Tower t)
        {
            t.SelfDemolish();
            HideHUD();
        }
    }
    public void UpgradeSelectedTower()
    {
        if (currentMO != null && currentMO is Tower t )
        {
            if (t.IsUpgradable() && GameManager.Instance.CanAffordPrice(t.UpgradePrice))
            {
                t.Upgrade();
                HideHUD();
            }
        }
    }
}
