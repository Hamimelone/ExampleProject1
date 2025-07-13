using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum TileType
{
    None,
    Wall,
    BuffTile,
    Locked,
    SpawnPoint,
    EndPoint
}

[CreateAssetMenu(menuName = "2D/Tiles/GameTile")]
public class GameTile : Tile
{
    public bool Walkable;
    public bool Placable;
    public TileType TileType;
}
