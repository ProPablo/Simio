using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Hunt : BehaviourComponent
{
    [Tooltip("Will hunt for food when health is below this threshold")]
    [Range(0, 1)] public float huntThreshold = 0.65f;
    public ActorType[] targets;
    public int seekDistance = 5;
    public Actor currentTarget = null;
    public float minDot = -0.1f;
    public override bool OnTick()
    {
        base.OnTick();
        if (actor.currentHealth > huntThreshold * actor.totalHealthScaled) return false;
        currentTarget = FindTarget();
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
            actor.ChangeState(new AlertedMove(actor, dirVec.dir, neighbour));
            break;
        }
    }
    public Actor FindTarget()
    {
        var targetActors = BehaviourManager.i.currentActors.Where(a => targets.Any(t => a.type == t));
        //var threatActors = BehaviourManager.i.currentActors.Where(a=>  threats.Any(t=> a.CompareTag(t)));
        var closestTarget = targetActors
            .Where(t => t.currentTile.Distance(actor.currentTile) < seekDistance)
            // .Where(t => {})
            .OrderBy(t => t.currentTile.Distance(actor.currentTile))
            .FirstOrDefault();
        return closestTarget;
    }
}
