using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : Singleton<MapManager>
{
    public TileIndicator tileIndicator;
    [SerializeField] private Grid grid;
    [SerializeField] private Tilemap backgroundMap;
    [SerializeField] private Tilemap wallMap;
    [SerializeField] private Tilemap logicMap;
    [Header("InitializeSetting")]
    [SerializeField] private SpawnPortal spawnPortal;
    [SerializeField] private EndPortal endPortal;
    [SerializeField] private LockedGate lockedGate;
    public Dictionary<Vector2,TileType>DicPosToTileType = new Dictionary<Vector2,TileType>();

    public List<Vector2> NotPlacableList = new List<Vector2>();
    private void Start()
    {
        Initialize();
        LoadMap(backgroundMap, wallMap, logicMap);
    }
    public void Initialize()
    {
        NotPlacableList.Clear();
        DicPosToTileType.Clear();
    }
    public void LoadMap(Tilemap BgMap, Tilemap Wmap, Tilemap Lmap)
    {
        foreach (var pos in Lmap.cellBounds.allPositionsWithin)
        {
            if (!Lmap.HasTile(pos)) continue;
            Vector3 worldPos = Lmap.CellToWorld(pos);
            Vector2 gridPos = new Vector2(worldPos.x + grid.cellSize.x / 2, worldPos.y + grid.cellSize.y / 2);
            GameTile GTile = Lmap.GetTile<GameTile>(pos);
            switch (GTile.TileType)
            {
                case TileType.None:
                    break;
                case TileType.Wall:
                    break;
                case TileType.BuffTile:
                    break;
                case TileType.Locked:
                    LockedGate lg = Instantiate(lockedGate, gridPos, Quaternion.identity);
                    lg.Initialize();
                    break;
                case TileType.SpawnPoint:
                    SpawnPortal sp = Instantiate(spawnPortal, gridPos, Quaternion.identity);
                    sp.Initialize();
                    break;
                case TileType.EndPoint:
                    EndPortal ep = Instantiate(endPortal, gridPos,Quaternion.identity);
                    ep.Initialize();
                    break;
                default:
                    break;
            }
        }
        Lmap.GetComponent<TilemapRenderer>().sortingOrder = -1;

        foreach (var pos in Wmap.cellBounds.allPositionsWithin)
        {
            if (!Wmap.HasTile(pos)) continue;
            Vector3 worldPos = Wmap.CellToWorld(pos);
            Vector2 gridPos = new Vector2(worldPos.x + grid.cellSize.x / 2, worldPos.y + grid.cellSize.y / 2);
            NotPlacableList.Add(gridPos);
            DicPosToTileType.Add(gridPos, TileType.Wall);
        }
    }

    public bool IsPosPlacable(Vector2 pos)
    {
        if (NotPlacableList.Contains(pos)) return false;
        return true;
    }

    public void IndicatorSetTo(Vector2 pos)
    {
        tileIndicator.SetPosTo(pos);
    }
}
