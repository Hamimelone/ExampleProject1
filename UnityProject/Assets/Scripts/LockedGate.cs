using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedGate : MonoBehaviour
{
    public void Initialize()
    {
        MapManager.Instance.NotPlacableList.Add(transform.position);
        MapManager.Instance.DicPosToTileType.Add(transform.position, TileType.Locked);
    }
}
