using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Flee : BehaviourComponent
{
    public string evilMans = "fox";
    public Func<Actor, bool> actorPredicate = (a) => (a.name == "fox");
    public LayerMask enemyLayerMask;

    public string[] threats;
    public int maxThreatDistance = 4;
    public bool threat = true;
    public int tickDuration;
    public int distToPredatorThreshold = 3;
    Direction fleeDir;
    public bool startled = false;
    int elapsed = int.MaxValue;
    public override bool OnTick()
    {
        base.OnTick();
        
        FindThreat();
        if (threat)
        {
            startled = true;
        }
        if (startled)
        {
            elapsed = 0;
            //Set flee dir here
        }
        ticks++;
        if (ticks >= ticksPerAction)
        {
            OnAction();
            if (elapsed < tickDuration)
                ticks = 0;
            startled = false;
            return true;
        }
        return false;
    }
    

    public void FindThreat()
    {
        var threatActors = BehaviourManager.i.currentActors.Where(a=>  threats.Any(t=> a.CompareTag(t)));
        var closestThreat = threatActors
            .Where(t => t.currentTile.Distance(actor.currentTile) < maxThreatDistance)
            // .Where(t => {})
            .OrderBy(t => t.currentTile.Distance(actor.currentTile))
            .FirstOrDefault();
        if (closestThreat == null)
            return;
        threat = closestThreat;
    }
    public override void OnAction()
    {
        base.OnAction();
        elapsed++;
        actor.lastDir = Direction.S;
        actor.ChangeState(new MoveState(actor, transform.position + new Vector3(0, 0, -1)));
    }
}
