using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseManager: MonoBehaviour
{
    #region ����
    public static MouseManager Instance;

    private void Awake()
    {
        if (Instance != null)        //����ģʽ
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
