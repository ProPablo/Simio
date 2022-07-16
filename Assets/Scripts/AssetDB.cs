using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public enum Direction
{
    N,
    NE,
    SE,
    S,
    SW,
    NW
}
public enum Diet
{
    PLANT, FISH, MEAT, CORPSE
}
public static class SimioExtensions {
    public static Direction Opposite (this Direction direction) {
        return (int)direction < 3 ? (direction + 3) : (direction - 3);
    }
}

public class AssetDB : MonoBehaviour
{
    public static AssetDB i;
    public static int dirLength => Enum.GetNames(typeof(Direction)).Length;
    private void Awake()
    {
        if (i == null)
        {
            i = this;
            DontDestroyOnLoad(i);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
