using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewLevel", menuName = "Levels/Level")]
public class Level : ScriptableObject
{
    public int LevelIndex;
    public int MapX;
    public int MapY;
    public List<Mahjong> LevelMahjongs = new List<Mahjong>();
}
