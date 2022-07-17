using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Flee : BehaviourComponent
{
    //public string evilMans = "Wolf";
    //public Func<Actor, bool> actorPredicate = (a) => (a.name == "Wolf");
    //public LayerMask enemyLayerMask;

    //public string[] threats;
    public ActorType[] threats;
    public int maxThreatDistance = 4;
    public Actor currentThreat = null;
    public float minDot = -0.1f;

    public override bool OnTick()
    {
        base.OnTick();
        currentThreat = FindThreat();
        if (currentThreat == null) return false;
        ticks++;
        if (ticks >= ticksPerAction)
        {
            OnAction();
            return true;
            //if (elapsed < fleeActions)
            //    return true;
        }
        return false;
    }


    public Actor FindThreat()
    {
        var threatActors = BehaviourManager.i.currentActors.Where(a => threats.Any(t => a.type == t));
        //var threatActors = BehaviourManager.i.currentActors.Where(a=>  threats.Any(t=> a.CompareTag(t)));
        var closestThreat = threatActors
            .Where(t => t.currentTile.Distance(actor.currentTile) < maxThreatDistance)
            // .Where(t => {})
            .OrderBy(t => t.currentTile.Distance(actor.currentTile))
            .FirstOrDefault();

        return closestThreat;
    }
    public override void OnAction()
    {
        if (currentThreat == null) return;
        base.OnAction();
        var foxToMe = actor.transform.position - currentThreat.transform.position;
        foxToMe.y = 0;
        var directions = AssetDB.DirectionVectors
            .Select(d => (dot: Vector3.Dot(d.vec, foxToMe), dirVec: d))
            .Where(v => v.dot > minDot)
            .OrderBy(v => -v.dot)
//#if UNITY_EDITOR
            .ToList();
//#endif
        foreach (var dirDot in directions)
        {
            var neighbour = actor.currentTile.GetNeighbor(dirDot.dirVec.dir);
            if (neighbour == null) continue;
            if (!actor.walkable.Contains(neighbour.cellType)) continue;
            actor.ChangeState(new AlertedMove(actor, dirDot.dirVec.dir, neighbour));
            break;
        }
    }
}
