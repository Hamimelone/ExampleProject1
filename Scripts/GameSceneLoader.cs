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
        //// 1. ж�ص�ǰ����
        //AsyncOperation unloadOp = SceneManager.UnloadSceneAsync("GamingScene");
        //yield return unloadOp;

        //// 2. �ͷ�δʹ����Դ
        //Resources.UnloadUnusedAssets();
        //yield return null; // ȷ����ȫж��

        // 3. ���¼��س���
        AsyncOperation loadOp = SceneManager.LoadSceneAsync("GamingScene", LoadSceneMode.Additive);
        yield return loadOp;

        // 4. ���³�ʼ�����й�����
        FindObjectOfType<MapManager>().Initialize();
    }
}
