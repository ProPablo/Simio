using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wander : BehaviourComponent
{
    Direction newDir;
    public override bool OnTick()
    {
        base.OnTick();
        ticks++;
        if (ticks > ticksPerAction)
        {
            OnAction();
            return true;
        }
        return false;
    }
    public override void OnAction()
    {
        base.OnAction();
        while (newDir == actor.lastDir)
        {
            newDir = (Direction)Mathf.FloorToInt(Random.value * AssetDB.dirLength);
        }
        actor.lastDir = newDir;
        // Change transform.position to current grid position
        switch (newDir)
        {
            case Direction.N:
                actor.ChangeState(new MoveState(actor, transform.position + new Vector3(0, 0, 1)));
                break;
            case Direction.NE:
                actor.ChangeState(new MoveState(actor, transform.position + new Vector3(0.75f, 0, 0.25f)));
                break;
            case Direction.SE:
                actor.ChangeState(new MoveState(actor, transform.position + new Vector3(0.75f, 0, -0.25f)));
                break;
            case Direction.S:
                actor.ChangeState(new MoveState(actor, transform.position + new Vector3(0, 0, -1)));
                break;
            case Direction.SW:
                actor.ChangeState(new MoveState(actor, transform.position + new Vector3(-0.75f, 0, -0.25f)));
                break;
            case Direction.NW:
                actor.ChangeState(new MoveState(actor, transform.position + new Vector3(-0.75f, 0, 0.25f)));
                break;
        }
    }
}
