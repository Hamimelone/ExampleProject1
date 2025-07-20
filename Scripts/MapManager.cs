using NavMeshPlus.Components;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : Singleton<MapManager>
{
    [SerializeField] private Grid grid;
    [SerializeField] private Tilemap logicMap;
    public Tilemap CurrentLogicMap;
    [SerializeField] private Tilemap gameMapWall;
    [SerializeField] private Tilemap gameMapBG;
    [SerializeField] private GameTile gT_BG;
    [SerializeField] private NavMeshSurface nms;
    [SerializeField] private Transform mapObjectTransform;
    [Header("InitializeSetting")]
    [SerializeField] private SpawnPortal spawnPortal;
    [SerializeField] private T_MainCollector mainCollector;
    [SerializeField] private LockedGate lockedGate;
    public Dictionary<Vector2,GameTileData>DicPosToGTData = new Dictionary<Vector2,GameTileData>();

    public List<Vector2> NotPlacableList = new List<Vector2>();
    public void Initialize()
    {
        NotPlacableList.Clear();
        DicPosToGTData.Clear();
        GameObjectExtensions.DestroyAllChildren(mapObjectTransform.gameObject);
        gameMapBG.ClearAllTiles();
        gameMapWall.ClearAllTiles();
        nms.RemoveData();
        CurrentLogicMap = null;
        //LoadMap(logicMap);
    }
    public void LoadMap(Tilemap Lmap)
    {
        CurrentLogicMap = Instantiate(Lmap,grid.GetComponent<Transform>());
        foreach (var pos in CurrentLogicMap.cellBounds.allPositionsWithin)
        {
            if (!CurrentLogicMap.HasTile(pos)) continue;
            Vector3 worldPos = CurrentLogicMap.CellToWorld(pos);
            Vector2 gridPos = new Vector2(worldPos.x + grid.cellSize.x / 2, worldPos.y + grid.cellSize.y / 2);
            GameTile GTile = CurrentLogicMap.GetTile<GameTile>(pos);
            switch (GTile.TileType)
            {
                case TileType.Placable:
                    gameMapBG.SetTile(pos, GTile);
                    DicPosToGTData[gridPos] = new GameTileData(TileType.Placable, null);
                    break;
                case TileType.Wall:
                    gameMapWall.SetTile(pos, GTile);
                    DicPosToGTData[gridPos] = new GameTileData(TileType.Wall, null);
                    break;
                case TileType.Path:
                    gameMapBG.SetTile(pos, GTile);
                    DicPosToGTData[gridPos] = new GameTileData(TileType.Path, null);
                    break;
                case TileType.BuffTile:
                    gameMapBG.SetTile(pos, GTile);
                    DicPosToGTData[gridPos] = new GameTileData(TileType.BuffTile, null);
                    break;
                case TileType.Locked:
                    gameMapBG.SetTile(pos, gT_BG);
                    LockedGate lg = Instantiate(lockedGate, gridPos, Quaternion.identity, mapObjectTransform);
                    lg.Initialize();
                    break;
                case TileType.SpawnPoint:
                    gameMapBG.SetTile(pos, gT_BG);
                    SpawnPortal sp = Instantiate(spawnPortal, gridPos, Quaternion.identity, mapObjectTransform);
                    sp.Initialize();
                    break;
                case TileType.EndPoint:
                    gameMapBG.SetTile(pos, gT_BG);
                    TowerManager.Instance.PlaceTowerOn(100,gridPos);
                    break;
                default:
                    break;
            }
        }
        CurrentLogicMap.GetComponent<TilemapRenderer>().enabled = false;
        nms.BuildNavMesh();
        
    }

    public bool IsPosPlacable(Vector2 pos)
    {
        if (NotPlacableList.Contains(pos)) return false;
        return true;
    }

}

public class GameTileData
{
    public TileType gtType;
    public MapObject gtMO;

    public GameTileData(TileType tt,MapObject mo)
    {
        gtType = tt;
        gtMO = mo;
    }
}
