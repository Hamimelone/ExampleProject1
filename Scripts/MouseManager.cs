using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseManager: MonoBehaviour
{
    #region 单例
    public static MouseManager Instance;

    private void Awake()
    {
        if (Instance != null)        //单例模式
        {
            Destroy(gameObject);
        }
        else
            Instance = this;
    }
    #endregion

    public Vector2 MousePosion;
    public Vector2 Offset;

    private void Update()
    {
        MousePosion = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}
