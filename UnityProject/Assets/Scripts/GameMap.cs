using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMap : MonoBehaviour
{
    [SerializeField] private Grid grid;
    private void OnMouseOver()
    {
        // 获取鼠标位置对应的格子
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPosition = grid.WorldToCell(mousePosition);
        Vector3 alignedPosition = cellPosition + grid.cellSize / 2;
        MapManager.Instance.tileIndicator.transform.position = alignedPosition;
    }
}
