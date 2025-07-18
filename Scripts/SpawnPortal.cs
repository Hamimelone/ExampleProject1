using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPortal : MapObject
{
    public void Initialize()
    {
        MapManager.Instance.NotPlacableList.Add(transform.position);
        MapManager.Instance.DicPosToGTData[transform.position] = new GameTileData(TileType.SpawnPoint, this);
        MonsterSpawner.Instance.spawnPortals.Add(this);
    }
}
