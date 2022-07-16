using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Random = UnityEngine.Random;

public enum CellType
{
    GRASS,
    SAND,
    WATER,
    SEA,
    ROCK
}

[System.Serializable]
public struct SpawnDefinition
{
    //For now its randomised but soon each mesh will store information about each type of side
    public List<GameObject> spawns;
    public Color colorDef;
    public string name;
}


public class HexCell : MonoBehaviour
{
    public CellType cellType;
    public float elevation = 0f;
    public Gradient regionDefinition;
    public SpawnDefinition[] spawnDefinitions;
    public Image selectImage;

    // public Vector2Int location;
    public HexCoordinates coords;
    public int index;
    public TextMeshProUGUI text;
    public Canvas canvas;
    public float normalStackHeight = 5f;
    public float startingStackOffset = 1f;
    public float minStackHeight;
    //public float stackHeight;
    public HexCell[] neighbours = new HexCell[6];

    // public Stack<Actor> actorStack;
    public List<Actor> actorStack = new List<Actor>();

    public int Distance(HexCell other)
    {
        return coords.DistanceTo(other.coords);
    }


    private void Awake()
    {
        //Get components to populate in here
        canvas = GetComponentInChildren<Canvas>();
        text = canvas.GetComponentInChildren<TextMeshProUGUI>();
        selectImage = canvas.GetComponentInChildren<Image>();
        // selectImage.color = Color.clear;
        selectImage.enabled = false;
    }

    public void Init()
    {
        Color definition = regionDefinition.Evaluate(elevation);
        try
        {
            var newSpawn = spawnDefinitions.First(sd => sd.colorDef == definition);
            var tilePrefab = newSpawn.spawns.RandomElement();
            var instanced = Instantiate(tilePrefab, Vector3.zero, Quaternion.identity);
            instanced.transform.SetParent(transform, false);
            instanced.transform.localPosition = Vector3.zero;
            instanced.transform.rotation = Quaternion.Euler(0, 30, 0);
            instanced.transform.localScale = Vector3.one;
        }
        catch
        {
            Debug.LogError($"No color definition for {definition}");
        }
    }

    //public void FindNeighbours(HexGrid grid)
    //{
    //    var directions = AssetDB.i.DirectionVectors;
    //    for (int i = 0; i < directions.Length; i++)
    //    {
    //        var neighbour = grid.GetCell(this, directions[i]);
    //        neighbours[i] = neighbour;
    //    }
    //}

    public HexCell GetNeighbor(Direction direction)
    {
        return neighbours[(int)direction];
    }
    public (HexCell, Direction) SelectRandomNeighbor(CellType[] cellTypes)
    {
        Direction tileDir;
        HexCell neighbourTile;
        int nCount = 0;
        do
        {
            tileDir = (Direction)Random.Range(0, AssetDB.dirLength);
            neighbourTile = GetNeighbor(tileDir);
            if (!cellTypes.Contains(neighbourTile.cellType)) neighbourTile = null;
        }
        while (neighbourTile == null && nCount++ < 6);
        return (neighbourTile, tileDir);
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
    private void Update()
    {
        if (actorStack.Count == 0) return;
        RearrangeStack();
        //var actorRegion = KongrooUtils.RemapRange(maxStackHeight / actorStack.Count, 0, 1, minStackHeight, maxStackHeight);
        //var actors = actorStack.ToArray();
        //for (int i = 0; i < actors.Length; i++)
        //{
        //    var actor = actors[i];
        //    var startingPoint = startingStackOffset + transform.position.y;
        //    var pos = actor.transform.localPosition;
        //    pos.y = startingPoint + actorRegion * i;
        //    actor.transform.localPosition = pos;
        //}
        //RearrangeStack();
    }
    private void RearrangeStack()
    {
        var actors = actorStack.ToArray();

        for (int i = 0; i < actors.Length; i++)
        {
            var pos = Vector3.zero;
            pos.y = startingStackOffset + i * minStackHeight;
            actors[i].transform.localPosition = pos; 
        }

        //var actorRegion = Mathf.Clamp(normalStackHeight / actorStack.Count, minStackHeight, float.MaxValue);
        //for (int i = 0; i < actors.Length; i++)
        //{
        //    var actor = actors[i];
        //    var startingPoint = actorRegion * i + startingStackOffset;
        //    var pos = actor.transform.localPosition;
        //    pos.y = startingPoint + actorRegion / 2;

            //    actor.transform.localPosition = pos;
            //}
    }

    public void SetNeighbor(Direction direction, HexCell cell)
    {
        neighbours[(int)direction] = cell;
        cell.neighbours[(int)direction.Opposite()] = this;
    }

    public void JoinCell(Actor actor)
    {
        // actorStack.Push(actor);
        actor.transform.SetParent(transform, false);
        actorStack.Add(actor);
        //RearrangeStack();
    }

    public void LeaveCell(Actor actor)
    {
        //readjust all animals in stack
        actor.transform.SetParent(null);
        actorStack.Remove(actor);
        //RearrangeStack();
    }
}