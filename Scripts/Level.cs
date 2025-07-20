using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Game/Level")]
public class Level : ScriptableObject
{
    [Header("关卡地图信息")]                   //MapManager地图相关
    public Tilemap LevelMap;                   //关卡地图
    [Header("关卡游戏信息")]                   //GameManager游戏相关
    public int initialGold;                    //关卡初始金币
}
