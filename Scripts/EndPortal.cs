using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPortal : MonoBehaviour
{
    public void Initialize()
    {
        MapManager.Instance.NotPlacableList.Add(transform.position);
    }
}
