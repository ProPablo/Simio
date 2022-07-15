using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class HexGrid : MonoBehaviour
{
    public float outerRad = 5f;
    public float innerRad => outerRad * 0.866025404f;

    //using second configuration
    public Vector3[] corners;
    public int width = 6;
    public int height = 6;

    public HexCell cellPrefab;
    private HexCell[] cells;

    
    Mesh hexMesh;
    List<Vector3> vertices;
    List<int> triangles;
    void Awake()
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
        GetComponent<MeshFilter>().mesh = hexMesh = new Mesh();
        hexMesh.name = "Hex Mesh";
        vertices = new List<Vector3>();
        triangles = new List<int>();
    }

    private void Start()
    {
        cells = new HexCell[width * height];
        CreateGrid();
    }

    public void CreateGrid()
    {
        int cellIndex = 0;
        for (int i = 0; i < width; i++)
        for (int j = 0; j < height; j++)
        {
            CreateCell(i, j, cellIndex++);
        }
    }

    public void CreateCell(int x, int z, int index)
    {
        Vector3 position;
        // position.x = x * outerRad * 1.5f;
        position.x = (x + z * 0.5f - z / 2) * outerRad * 1.5f;
        position.y = 0f;
        position.z = z * innerRad * 2f;

        // position.x = x * (innerRadius * 2f);
        // position.y = 0f;
        // position.z = z * (HexMetrics.outerRadius * 1.5f);

        var cell = cells[index] = Instantiate<HexCell>(cellPrefab);
        cell.cellType = 0;
        cell.location = new Vector2Int(x, z);
        cell.index = index;
        cell.text.text = x.ToString() + "\n" + z.ToString();
        // cell.Init(0, new Vector2Int(x, z), index);

        cell.transform.SetParent(transform, false);
        cell.transform.localPosition = position;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        for (int i = 0; i < corners.Length; i++)
        {
            var lastIndex = i - 1 >= 0 ? i - 1 : corners.Length - 1;
            var start = corners[lastIndex];
            var end = corners[i];
            Gizmos.DrawLine(start, end);
        }
    }
}