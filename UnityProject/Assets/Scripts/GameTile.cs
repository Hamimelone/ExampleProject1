using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum TileType
{
    Placable,
    Wall,
    BuffTile,
    Locked,
    SpawnPoint,
    EndPoint,
    Path,
    Tower
}

[CreateAssetMenu(menuName = "2D/Tiles/GameTile")]
public class GameTile : Tile
{
    public bool Walkable;
    public bool Placable;
    public TileType TileType;
}
