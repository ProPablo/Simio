using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class ResourceSpawnDefinition
{
    // float spawnChance
    public int spawnTicks = 10;
    public int tickCounter = 0;
    public Actor[] spawns;
    public CellType[] placeableTiles;
}

public class ResourceRefresher : BehaviourComponent
{
    public ResourceSpawnDefinition[] resources;
    public int maxTileGuesses = 400;

    public override bool OnTick()
    {
        base.OnTick();
        foreach (var resource in resources)
        {
            if (resource.tickCounter++ >= resource.spawnTicks)
            {
                resource.tickCounter = 0;
                HexCell spawnLoc;
                int counter = 0;
                do
                {
                    spawnLoc = HexGrid.i.cells.RandomElement();
                    if (!resource.placeableTiles.Contains(spawnLoc.cellType)) spawnLoc = null;
                    counter++;
                } while (spawnLoc == null && counter < maxTileGuesses);
                var actorToSpawn = resource.spawns.ToList().RandomElement();
                BehaviourManager.i.SpawnActor(actorToSpawn, spawnLoc);
            }
        }

        return false;
    }
    // public override void OnAction() {
    //     base.OnAction();
    //     var (neighbourTile, tileDir) = actor.currentTile.SelectRandomNeighbor(actor.walkable);
    //     if (neighbourTile == null) return;
    //     actor.ChangeState(new MoveState(actor, tileDir, neighbourTile));
    // }
}