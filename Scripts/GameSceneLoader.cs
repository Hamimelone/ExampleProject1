using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneLoader : MonoBehaviour
{
    public void LoadGameScene()
    {
        StartCoroutine(ReloadGameScene());
    }

    private IEnumerator ReloadGameScene()
    {
        //// 1. 卸载当前场景
        //AsyncOperation unloadOp = SceneManager.UnloadSceneAsync("GamingScene");
        //yield return unloadOp;

        //// 2. 释放未使用资源
        //Resources.UnloadUnusedAssets();
        //yield return null; // 确保完全卸载

        // 3. 重新加载场景
        AsyncOperation loadOp = SceneManager.LoadSceneAsync("GamingScene", LoadSceneMode.Additive);
        yield return loadOp;

        // 4. 重新初始化所有管理器
        FindObjectOfType<MapManager>().Initialize();
    }
}
