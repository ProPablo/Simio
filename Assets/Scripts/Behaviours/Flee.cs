using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Flee : BehaviourComponent
{
    [Tooltip("How many actions to flee away from after recognising a threat")]
    public int fleeActions;
    //public string evilMans = "Wolf";
    //public Func<Actor, bool> actorPredicate = (a) => (a.name == "Wolf");
    //public LayerMask enemyLayerMask;

    //public string[] threats;
    public ActorType[] threats;
    public int maxThreatDistance = 4;
    Direction tileDir;
    int elapsed = int.MaxValue;
    Actor currentThreat = null;

    public override bool OnTick()
    {
        base.OnTick();
        currentThreat = FindThreat();
        if (currentThreat == null) return false;
        ticks++;
        if (ticks >= ticksPerAction)
        {
            OnAction();
            if (elapsed < fleeActions)
                return true;
        }
        return false;
    }
    

    public Actor FindThreat()
    {
        var threatActors = BehaviourManager.i.currentActors.Where(a => threats.Any(t=> a.type == t));
        //var threatActors = BehaviourManager.i.currentActors.Where(a=>  threats.Any(t=> a.CompareTag(t)));
        var closestThreat = threatActors
            .Where(t => t.currentTile.Distance(actor.currentTile) < maxThreatDistance)
            // .Where(t => {})
            .OrderBy(t => t.currentTile.Distance(actor.currentTile))
            .FirstOrDefault();
       
        elapsed = 0;
        return closestThreat;
    }
    public override void OnAction()
    {
        if (!currentThreat) return;
        base.OnAction();
        elapsed++;

        var foxToMe = actor.transform.position - currentThreat.transform.position;

        var directions = AssetDB.DirectionVectors.OrderBy(d => Vector3.Dot(d.vec, foxToMe));
        if ()

        actor.ChangeState(new MoveState(actor, tileDir, ));
    }
}
