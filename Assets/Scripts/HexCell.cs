using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    public TextMeshProUGUI text;
    public Canvas canvas;
    
    

    private void Awake()
    {
        //Get components to populate in here
        canvas = GetComponentInChildren<Canvas>();
        text = canvas.GetComponentInChildren<TextMeshProUGUI>();

    }

    public void Init(CellType _cellType, Vector2Int _location, int _index)
    {
        cellType = _cellType;
        location = _location;
        index = _index;
        text.text = _location.x.ToString() + "\n" + _location.y.ToString();
    }
}
