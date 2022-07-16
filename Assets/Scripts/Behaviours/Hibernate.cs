using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hibernate : BehaviourComponent
{
    public int ticksTillHibernate = 100;
    public float healthThreshold = 0.25f;
    public override bool OnTick()
    {
        base.OnTick();
        if (actor.currentHealth < Mathf.FloorToInt(actor.totalHealthScaled * healthThreshold))
        {
            ticks++;
        }
        if (ticks >= ticksTillHibernate)
        {
            OnAction();
            return true;
        }
        return false;
    }
    public override void OnAction()
    {
        actor.currentHealth++;
        if (actor.currentHealth >= actor.totalHealthScaled)
            ticks = 0;
    }
}
