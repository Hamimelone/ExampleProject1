using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileIndicator : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private Transform buildBtnLayout;
    [SerializeField] private Button btn_Upgrade;
    [SerializeField] private Button btn_Demolish;
    [SerializeField] private Button btn_Unlock;

    private SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }
    public void SetPosTo(Vector2 pos)
    {
        transform.position = pos;
        sr.color = MapManager.Instance.IsPosPlacable(pos)? Color.white : Color.red;
    }
}
