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

[System.Serializable]
public struct SpawnDefinition
{
    //For now its randomised but soon each mesh will store information about each type of side
    public List<Mesh> spawns;
    public Color colorDef;
}

public class HexCell : MonoBehaviour
{
    public CellType cellType;
    public float elevation = 0f;
    public Gradient regionDefinition;
    public SpawnDefinition[] spawnRegions;

    // public Vector2Int location;
    public HexCoordinates coords;
    public int index;
    public TextMeshProUGUI text;
    public Canvas canvas;
    public float minStackHeight;

    public Stack<Actor> actorStack;

    public Image selectImage;

    private void Awake()
    {
        //Get components to populate in here
        canvas = GetComponentInChildren<Canvas>();
        text = canvas.GetComponentInChildren<TextMeshProUGUI>();
        selectImage = canvas.GetComponentInChildren<Image>();
        // selectImage.color = Color.clear;
        selectImage.enabled = false;
    }

    public void Init(CellType _cellType, Vector2Int _location, int _index)
    {
        cellType = _cellType;
        // location = _location;
        index = _index;
        text.text = _location.x.ToString() + "\n" + _location.y.ToString();
    }

    public void ToggleCell()
    {
        selectImage.enabled = !selectImage.enabled;
    }

    public void SelectCell(bool isSelected)
    {
        selectImage.enabled = isSelected;
    }


    public int JoinCell(Actor actor)
    {
        //return stackid
        return 0;
    }

    public void LeaveCell(Actor actor)
    {
        //readjust all animals in stack
    }
}