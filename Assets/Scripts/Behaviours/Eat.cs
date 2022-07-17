using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Eat : BehaviourComponent
{
    [Tooltip("Will eat when health is below this threshold")] [Range(0, 1)]
    public float hungerThreshold = 0.95f;

    [Tooltip("How much HP to consume per action")]
    public int consumeRate = 5;

    Actor food = null;

    public override bool OnTick()
    {
        base.OnTick();
        if (actor.currentHealth > actor.totalHealthScaled * hungerThreshold) return false;
        ticks++;
        //switch (actor.diet)
        //{
        //    case Diet.PLANT:
        //        //food = GetClosestResource()
        //        //food = actor.currentTile.actorStack.FirstOrDefault(a => a.CompareTag("Plant"));
        //        break;
        //    case Diet.FISH:
        //        food = GetClosestResource(Diet.FISH);
        //        food = actor.currentTile.actorStack.FirstOrDefault(a => a.CompareTag("Fish"));
        //        break;
        //    case Diet.MEAT:
        //        food = actor.currentTile.actorStack.FirstOrDefault(a => a.CompareTag("Meat"));
        //        break;
        //    case Diet.CORPSE:
        //        food = actor.currentTile.actorStack.FirstOrDefault(a => a.CompareTag("Corpse"));
        //        break;

        //}
        food = GetClosestResource(actor.diet);
        if (food != null)
        {
            if (ticks >= ticksPerAction)
            {
                OnAction();
                return true;
                //if (actor.currentHealth >= actor.totalHealthScaled)
                //    return true;
            }
        }

        return false;
    }

    Actor GetClosestResource(Diet resType)
    {
        var allTiles = actor.currentTile.neighbours.Append(actor.currentTile).Where(t => t != null);
        return allTiles
            .SelectMany(t => t.actorStack.Where(a => a.type == ActorType.RESOURCE && a.diet == resType))
            .FirstOrDefault();
    }

    public override void OnAction()
    {
        //Subscribe to if the food dies early THERE MAY BE AN ERROR HERE
        base.OnAction();
        if (food == null) return;
        actor.ChangeState(new EatingState(actor, food, consumeRate));
        food = null;
    }
}