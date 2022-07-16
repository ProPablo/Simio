using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;


public enum Direction
{
    N,
    NE,
    SE,
    S,
    SW,
    NW
}

public static class SimioExtensions {
    public static Direction Opposite (this Direction direction) {
        return (int)direction < 3 ? (direction + 3) : (direction - 3);
    }
}

public class AssetDB : MonoBehaviour
{
    public Vector3[] DirectionVectors = new[]
    {
        new Vector3(0,0,1),
        new Vector3(1, 0, 1),
        new Vector3(1, 0, -1),
        new Vector3(0,0,-1),
        new Vector3(-1, 0, -1),
        new Vector3(-1, 0, 1)
        
    };
    
    public static AssetDB i;
    public static int dirLength => Enum.GetNames(typeof(Direction)).Length;
    private void Awake()
    {
        if (i == null)
        {
            i = this;
            DontDestroyOnLoad(i);
            RunOnce();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void RunOnce()
    {
        DirectionVectors = DirectionVectors.Select(v => v.normalized).ToArray();
    }
}
