using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public int X;
    public int Y;
    public bool IsPlacable
    {
        get { return transform.childCount == 0; }
    }
}
