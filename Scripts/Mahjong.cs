using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum MahjongType
{
    条,
    筒,
    万,
    箭,
    风,
    王
}
public class Mahjong : MonoBehaviour
{
    [SerializeField] private MahjongType mahjongType;
    [SerializeField] private int value;
    private bool moveable;
    private Vector2 dragOffset = Vector2.zero;
    private bool isDragging;
    private Vector2 originPos;
    private Vector2 originScale;
    public int X;
    public int Y;
    public MahjongType MahjongType
    {
        get {  return mahjongType; }
    }
    public int Value
    {
        get { return value; }
    }
    public void SetMoveable(bool a)
    {
        moveable = a;
    }
    private void Start()
    {
        isDragging = false;
    }
    private void OnMouseDown()
    {
        if (moveable)
        {
            dragOffset = new Vector2(transform.position.x, transform.position.y) - MouseManager.Instance.MousePosion;
            originPos = transform.position;
            originScale = transform.localScale;
            transform.localScale = Vector3.one;
            isDragging = true;
            MahjongManager.Instance.SetCurrentDraggingMahjong(this);
        } 
    }
    private void OnMouseDrag()
    {
        if (isDragging)
        {
            transform.position = dragOffset + MouseManager.Instance.MousePosion;
            GetComponent<SpriteRenderer>().sortingOrder = 10;
        }
    }

    private void OnMouseUp()
    {
        isDragging = false;
        if (BoardManager.Instance.IsMouseInsideTheBoard(MouseManager.Instance.MousePosion))
        {
            MahjongManager.Instance.TryToPlaceMahjong();
        }
        else
        {
            CancelMoving();
        }
    }
    public void CancelMoving()
    {
        transform.position = originPos;
        transform.localScale = originScale;
        MahjongManager.Instance.RefreshCurrentDraggingMahjong();
    }

}
