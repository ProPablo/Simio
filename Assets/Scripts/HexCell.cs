using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CellType
{
    GRASS,
    SAND,
    WATER,
}
public class HexCell : MonoBehaviour
{
    public CellType cellType;
    public Vector2Int location;
    public int index;

    private void Awake()
    {
        //Get components to populate in here
    }

    public void Init(CellType _cellType, Vector2Int _location, int _index)
    {
        cellType = _cellType;
        location = _location;
        index = _index;
    }
}
