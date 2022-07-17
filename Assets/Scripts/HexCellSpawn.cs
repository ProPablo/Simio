using UnityEngine;
[CreateAssetMenu(fileName = "Gen.asset", menuName = "MapGen")]
public class HexCellSpawn : ScriptableObject
{
    public Gradient regionDefinition;
    public SpawnDefinition[] spawnDefinitions;
    public float waterHeight;
}