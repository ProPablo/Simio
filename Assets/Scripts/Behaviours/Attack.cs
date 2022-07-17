using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : BehaviourComponent
{
    public ActorType[] targets;
    //public override bool OnTick()
    //{
    //    base.OnTick();
    //    if (actor.currentTile.GetNeighbor()) return false;
    //    currentTarget = FindTarget();
    //    ticks++;
    //    if (ticks >= ticksPerAction)
    //    {
    //        OnAction();
    //        return true;
    //    }
    //    return false;
    //}
}
