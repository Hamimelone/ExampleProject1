using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    public List<Level> levels = new List<Level>();
    public int CurrentLevelIndex;

    private void Start()
    {
        CurrentLevelIndex = 0;
    }

    public void NextLevel()
    {
        if (CurrentLevelIndex < levels.Count - 1)
        {
            CurrentLevelIndex++;
        }
        else CurrentLevelIndex = 0;
    }
}
