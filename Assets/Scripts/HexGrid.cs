using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexGrid : MonoBehaviour
{
    public float outerRad = 5f;

    public float innerRad => outerRad * 0.866025404f;

    //using second configuration
    public Vector3[] corners;

    // Start is called before the first frame update
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
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        for (int i = 0; i < corners.Length; i++)
        {
            var lastIndex = i - 1 >= 0 ? i - 1 : corners.Length-1;
            var start = corners[lastIndex];
            var end = corners[i];
            Gizmos.DrawLine(start, end);
        }
    }
}