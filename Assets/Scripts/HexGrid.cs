using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[ExecuteInEditMode]
public class HexGrid : MonoBehaviour
{
    public float outerRad = 5f;
    public float innerRad => outerRad * 0.866025404f;

    public static Vector3[] Corners => instance.corners;
    public static float OuterRad => instance.outerRad;

    public static float InnerRad => instance.innerRad;

    //using second configuration
    public Vector3[] corners;
    public int width = 6;
    public int height = 6;

    public HexCell cellPrefab;
    private HexCell[] cells;

    public HexMesh mesh;

    public static HexGrid instance;

    private Camera _cam;

    void Awake()
    {
        instance = this;
        _cam = Camera.main;
        corners = new[]
        {
            new Vector3(-0.5f * outerRad, 0, innerRad),
            new Vector3(0.5f * outerRad, 0, innerRad),
            new Vector3(outerRad, 0, 0),
            new Vector3(0.5f * outerRad, 0, -innerRad),
            new Vector3(-0.5f * outerRad, 0, -innerRad),
            new Vector3(-outerRad, 0, 0),
        };
        mesh = GetComponent<HexMesh>();
    }

    public void Init()
    {
        corners = new[]
        {
            new Vector3(-0.5f * outerRad, 0, innerRad),
            new Vector3(0.5f * outerRad, 0, innerRad),
            new Vector3(outerRad, 0, 0),
            new Vector3(0.5f * outerRad, 0, -innerRad),
            new Vector3(-0.5f * outerRad, 0, -innerRad),
            new Vector3(-outerRad, 0, 0),
        };
        ClearGrid();
        CreateGrid();
        mesh.Triangulate(cells);
    }

    private void Start()
    {
        CreateGrid();
        mesh.Triangulate(cells);
    }

    public void CreateGrid()
    {
        cells = new HexCell[width * height];
        int cellIndex = 0;
        for (int i = 0; i < width; i++)
        for (int j = 0; j < height; j++)
        {
            CreateCell(i, j, cellIndex++);
        }
    }

    public void CreateCell(int x, int z, int index)
    {
        //Swapping the x and z from the tutorial gives the right coords for flat headed hexagons
        Vector3 position;
        position.x = x * outerRad * 1.5f;
        position.y = 0f;
        position.z = (z + x * 0.5f - x / 2) * innerRad * 2f;
        
        //Using perlin generate the elevation

        var cell = cells[index] = Instantiate<HexCell>(cellPrefab);
        cell.cellType = 0;
        cell.coords = HexCoordinates.FromOffsetCoordinates(x, z);
        cell.index = index;
        // cell.text.text = cell.coords.ToStringOnSeparateLines();
        cell.text.text = index.ToString();
        // cell.elevation = Mathf.PerlinNoise()
        // cell.Init(0, new Vector2Int(x, z), index);

        cell.transform.SetParent(transform, false);
        cell.transform.localPosition = position;
    }

    public void ClearGrid()
    {
        foreach (var go in GameObject.FindGameObjectsWithTag("Cell"))
        {
            DestroyImmediate(go);
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleInput();
        }
    }

    void HandleInput()
    {
        Ray inputRay = _cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(inputRay, out RaycastHit hit))
        {
            Vector3 pos = hit.point;
            //This returns the localposition if the vector was a child of the transform
            pos = transform.InverseTransformPoint(pos);
            HexCoordinates coords = HexCoordinates.FromPosition(pos);
            // int index = coords.X + coords.Z * width + coords.Z / 2;
            int index = coords.Z + coords.X * height + coords.X / 2;
            cells[index].ToggleCell();
            Debug.Log("touched at " + coords.ToString() + "at :" + index);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        // for (int i = 0; i < corners.Length; i++)
        // {
        //     // var lastIndex = i - 1 >= 0 ? i - 1 : corners.Length - 1;
        //     var nextIndex = i + 1 < corners.Length ? i + 1 : 0;
        //     // var start = corners[lastIndex];
        //     // var end = corners[i];
        //     var start = corners[i];
        //     var end = corners[nextIndex];
        //     Gizmos.DrawLine(start, end);
        // }

        foreach (var cell in cells)
        {
            var center = cell.transform.position;
            for (int i = 0; i < corners.Length; i++)
            {
                var nextIndex = i + 1 < corners.Length ? i + 1 : 0;
                var start = corners[i] + center;
                var end = corners[nextIndex] + center;
                Gizmos.DrawLine(start, end);
            }
        }
    }
}

[System.Serializable]
public struct HexCoordinates
{
    public int X { get; private set; }

    public int Z { get; private set; }

    //If you add all coords, you get zero
    public int Y
    {
        get { return -X - Z; }
    }

    public HexCoordinates(int x, int z)
    {
        X = x;
        Z = z;
    }

    public static HexCoordinates FromOffsetCoordinates(int x, int z)
    {
        //axial coordinates
        return new HexCoordinates(x, z - x / 2);
    }

    public override string ToString()
    {
        return "(" +
               X.ToString() + ", " + Y.ToString() + ", " + Z.ToString() + ")";
    }

    public string ToStringOnSeparateLines()
    {
        return X.ToString() + "\n" + Y.ToString() + "\n" + Z.ToString();
    }

    public static HexCoordinates FromPosition(Vector3 pos)
    {
        //Vertical width of hexagon
        float z = pos.z / (HexGrid.InnerRad * 2f);
        //y is mirror of z
        float y = -z;

        //Every two cols shift entire unit to top
        float offset = pos.x / (HexGrid.OuterRad * 3f);
        z -= offset;
        y -= offset;
        int iX = Mathf.RoundToInt(-z - y);
        int iY = Mathf.RoundToInt(y);
        int iZ = Mathf.RoundToInt(z);
        if (iX + iY + iZ != 0)
        {
            Debug.LogWarning("rounding error!");
        }

        return new HexCoordinates(iX, iZ);
    }
}