using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public List<Level> AllLevels = new List<Level>();
    private Level currentLevel;


    private void Start()
    {
        currentLevel = null;
    }
    public void LoadLevel(int levelIndex)
    {
        if (levelIndex > AllLevels.Count) return;
        currentLevel = AllLevels[levelIndex];
    }

    public void ClearCurrentLevel()
    {
        MahjongManager.Instance.Initialize();

    }
}
