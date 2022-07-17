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
public enum Diet
{
    PLANT, FISH, MEAT, CORPSE
}
public enum ActorType
{
    RESOURCE, PREY, PREDATOR, APEX
}
public static class SimioExtensions
{
    public static Direction Opposite(this Direction direction)
    {
        return (int)direction < 3 ? (direction + 3) : (direction - 3);
    }
}

public struct EuclidianDistanceVec
{
    public Vector3 vec;
    public Direction dir;
    public EuclidianDistanceVec(Vector3 vec, Direction dir)
    {
        this.dir = dir;
        this.vec = vec;
    }
}

public enum CellType
{
    GRASS,
    SAND,
    WATER,
    SEA,
    ROCK,
}

[System.Serializable]
public struct SpawnDefinition
{
    //For now its randomised but soon each mesh will store information about each type of side
    //public List<GameObject> spawns;
    //TODO: use wave function collapse with scriptable object defintion of spawnable tiles
    public GameObject tilePrefab;
    public Color colorDef;
    //public string name;
    public CellType type;
}


public class AssetDB : MonoBehaviour
{
    //public static  (Vector3, Direction)[] DirectionVectors = new[]
    //{
    //    (new Vector3(0,0,1), Direction.N),
    //    (new Vector3(1, 0, 1), Direction.NE),
    //    (new Vector3(1, 0, -1), Direction.SE),e.
    //    (new Vector3(0,0,-1), Direction.S),
    //    (new Vector3(-1, 0, -1), Direction.SW),
    //    (new Vector3(-1, 0, 1), Direction.NW)
    //};
    public HexCellSpawn spawnGenDefinition;

    public static EuclidianDistanceVec[] DirectionVectors =
    {
       new EuclidianDistanceVec(new Vector3(0,0,1), Direction.N ),
         new EuclidianDistanceVec(new Vector3(1, 0, 1), Direction.NE),
         new EuclidianDistanceVec(new Vector3(1, 0, -1), Direction.SE),
         new EuclidianDistanceVec(new Vector3(0,0,-1), Direction.S),
         new EuclidianDistanceVec(new Vector3(-1, 0, -1), Direction.SW),
         new EuclidianDistanceVec(new Vector3(-1, 0, 1), Direction.NW)
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
        DirectionVectors = DirectionVectors.Select(v => new EuclidianDistanceVec(v.vec.normalized, v.dir)).ToArray();
    }
    public static SpawnDefinition GetTileDefinition(float evaluation)
    {
        Color definition = i.spawnGenDefinition.regionDefinition.Evaluate(evaluation);
        try
        {
            return i.spawnGenDefinition.spawnDefinitions.First(sd => sd.colorDef == definition);
        }
        catch
        {
            Debug.LogError($"No color definition for {definition}");
            throw;
        }
        
    }

    //public static float LowestWaterPointNormalized ()
    //{
    //    return i.spawnGenDefinition.regionDefinition.colorKeys[CellType.SAND];
    //}
}
