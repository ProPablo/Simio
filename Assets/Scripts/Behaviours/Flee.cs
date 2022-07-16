using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flee : BehaviourComponent
{
    public bool threat = true;
    public int tickDuration;
    public int distToPredatorThreshold = 3;
    Direction fleeDir;
    public bool startled = false;
    int elapsed = int.MaxValue;
    public override bool OnTick()
    {
        base.OnTick();
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
    public override void OnAction()
    {
        base.OnAction();
        elapsed++;
        actor.lastDir = Direction.S;
        actor.ChangeState(new MoveState(actor, transform.position + new Vector3(0, 0, -1)));
    }
}
