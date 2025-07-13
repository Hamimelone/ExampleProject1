using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance; // 单例实例

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                // 查找场景中是否已经存在该单例
                _instance = FindObjectOfType<T>();

                // 如果场景中不存在，则创建一个新的 GameObject 并挂载单例脚本
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject(typeof(T).Name);
                    _instance = singletonObject.AddComponent<T>();
                }
            }
            return _instance;
        }
    }

    protected virtual void Awake()
    {
        // 确保单例唯一性
        if (_instance == null)
        {
            _instance = this as T;
            DontDestroyOnLoad(gameObject); // 可选：保持单例在场景切换时不销毁
        }
        else
        {
            Destroy(gameObject); // 如果已经存在单例，则销毁当前对象
        }
    }
}
