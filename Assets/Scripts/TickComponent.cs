using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    N,
    NE,
    SE,
    S,
    SW,
    NW
}
public class TickComponent : MonoBehaviour
{
    public virtual void OnTick() { }
}
