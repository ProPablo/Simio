using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
