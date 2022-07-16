using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunt : BehaviourComponent
{
    [Tooltip("Will hunt for food when health is below this threshold")]
    [Range(0, 1)] public float huntThreshold = 0.65f;
    public int seekDistance = 15;
    Direction newDir;
    public override bool OnTick()
    {
        base.OnTick();
        if (actor.currentHealth > huntThreshold * actor.totalHealthScaled) return false;
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
        base.OnAction();
        
    }
}
