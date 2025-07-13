using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : Singleton<MapManager>
{
    public GameObject tileIndicator;
    [SerializeField] private Grid grid;
    [SerializeField] private Tilemap backgroundMap;
    [SerializeField] private Tilemap logicMap;
    [Header("InitializeSetting")]
    [SerializeField] private GameObject spawnPortal;
    [SerializeField] private GameObject endPortal;

    private void Start()
    {
        LoadMap(logicMap);
    }
    public void LoadMap(Tilemap Lmap)
    {
        foreach (var pos in Lmap.cellBounds.allPositionsWithin)
        {
            if (!Lmap.HasTile(pos)) continue;
            Vector3 worldPos = Lmap.CellToWorld(pos);
            Vector2 gridPos = new Vector2(worldPos.x + grid.cellSize.x / 2, worldPos.y + grid.cellSize.y / 2);
            GameTile GTile = Lmap.GetTile<GameTile>(pos);
        }
    }
}
