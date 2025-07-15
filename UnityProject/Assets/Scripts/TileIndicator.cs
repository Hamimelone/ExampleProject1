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
    [SerializeField] private Button towerBuildBtnPrefab;
    [SerializeField] private Transform buildBtnLayout;
    [SerializeField] private Transform mapObjectBtnLayout;
    [SerializeField] private Button btn_Upgrade;
    [SerializeField] private Button btn_Demolish;
    [SerializeField] private Button btn_Unlock;
    private bool IsClickable;

    private SpriteRenderer sr;

    private void Start()
    {
        IsDisplayingHUD = false;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // 如果点击的是 UI，直接返回
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
        // 获取鼠标位置对应的格子
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
    }
    public bool CheckPlacable(Vector2 pos)
    {
        if (MapManager.Instance.DicPosToGTData[pos] == null)return false;
        if (MapManager.Instance.DicPosToGTData[pos].gtMO == null)
        {
            TileType tt = MapManager.Instance.DicPosToGTData[pos].gtType;
            if (tt == TileType.Placable || tt == TileType.BuffTile) return true;
            else return false;
        }else return false;
    }
    public bool CheckMapObject(Vector2 pos)
    {
        return (MapManager.Instance.DicPosToGTData[pos] != null && MapManager.Instance.DicPosToGTData[pos].gtMO != null) ;
    }
    public bool CheckClickable(Vector2 pos)
    {
        if(!MapManager.Instance.DicPosToGTData.ContainsKey(pos))return false;
        switch (MapManager.Instance.DicPosToGTData[pos].gtType)
        {
            case TileType.Placable:
                return true;
            case TileType.Wall:
                return false;
            case TileType.BuffTile:
                return true;
            case TileType.Locked:
                return true;
            case TileType.SpawnPoint:
                return false;
            case TileType.EndPoint:
                return false;
            case TileType.Tower:
                return true;
            default: return false;
        }

    }

    public void DisplayHUDOn(Vector2 pos)
    {
        IsDisplayingHUD = true;
        transform.position = pos;
        canvas.gameObject.SetActive(true);
        sr.color = (CheckPlacable(pos)||CheckMapObject(pos)) ? Color.white : Color.red;
        canvas.gameObject.SetActive(true);
        buildBtnLayout.gameObject.SetActive(CheckPlacable(pos));
        if (CheckPlacable(pos)) RefreshBuildBtn(pos);
        mapObjectBtnLayout.gameObject.SetActive(CheckMapObject(pos));
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
                Button tb = Instantiate(towerBuildBtnPrefab, buildBtnLayout);
                tb.GetComponent<Image>().sprite = t.GetComponent<SpriteRenderer>().sprite;
                tb.onClick.AddListener(()=>TowerManager.Instance.PlaceTowerOn(t.TowerIndex,pos));
            }
        }
    }
}
