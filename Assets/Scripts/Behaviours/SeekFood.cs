using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SeekFood : BehaviourComponent
{
    public int seekDistance = 5;
    public Actor currentTarget = null;
    public float minDot = -0.1f;
    [Range(0, 1)] public float hungerThreshold;

    public override bool OnTick()
    {
        base.OnTick();
        if (actor.currentHealth > hungerThreshold * actor.totalHealthScaled) return false;
        currentTarget = FindTarget();
        if (currentTarget == null) return false;
        if (currentTarget.currentTile.Distance(actor.currentTile) <= 1) return false;
        ticks++;
        if (ticks >= ticksPerAction)
        {
            OnAction();
            return true;
        }
        return false;
    }
    public override void OnAction()
    {
        if (currentTarget == null) return;
        base.OnAction();
        var toTarget = currentTarget.transform.position - actor.transform.position;
        toTarget.y = 0;
        var directions = AssetDB.DirectionVectors
            .Select(d => (dot: Vector3.Dot(d.vec, toTarget), dirVec: d))
            .Where(v => v.dot > minDot)
            .OrderBy(v => -v.dot)
            .ToList();
        foreach (var (dot, dirVec) in directions)
        {
            var neighbour = actor.currentTile.GetNeighbor(dirVec.dir);
            if (neighbour == null) continue;
            if (!actor.walkable.Contains(neighbour.cellType)) continue;
            actor.ChangeState(new MoveState(actor, dirVec.dir, neighbour));
            break;
        }
    }
    public Actor FindTarget()
    {
        var targetActors = BehaviourManager.i.currentActors.Where(a => a.type == ActorType.RESOURCE && a.diet == actor.diet);
        //var threatActors = BehaviourManager.i.currentActors.Where(a=>  threats.Any(t=> a.CompareTag(t)));
        var closestTarget = targetActors
            .Where(t => t.currentTile.Distance(actor.currentTile) < seekDistance)
            // .Where(t => {})
            .OrderBy(t => t.currentTile.Distance(actor.currentTile))
            .FirstOrDefault();
        return closestTarget;
    }
}
