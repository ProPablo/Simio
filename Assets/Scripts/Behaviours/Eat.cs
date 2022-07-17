using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Eat : BehaviourComponent
{
    [Tooltip("Will eat when health is below this threshold")]
    [Range(0, 1)] public float hungerThreshold = 0.95f;
    [Tooltip("How much HP to consume per action")]
    public int consumeRate = 5;
    Actor food = null;
    public override bool OnTick()
    {
        base.OnTick();
        if (actor.currentHealth > actor.totalHealthScaled * hungerThreshold) return false;
        ticks++;
        switch (actor.diet)
        {
            case Diet.PLANT:
                food = actor.currentTile.actorStack.FirstOrDefault(a => a.CompareTag("Plant"));
                break;
            case Diet.FISH:
                food = actor.currentTile.actorStack.FirstOrDefault(a => a.CompareTag("Fish"));
                break;
            case Diet.MEAT:
                food = actor.currentTile.actorStack.FirstOrDefault(a => a.CompareTag("Meat"));
                break;
            case Diet.CORPSE:
                food = actor.currentTile.actorStack.FirstOrDefault(a => a.CompareTag("Corpse"));
                break;
        }
        if (food != null)
        {
            if (ticks >= ticksPerAction)
            {
                OnAction();
                if (actor.currentHealth >= actor.totalHealthScaled)
                    return true;
            }
        }
        return false;
    }
    public override void OnAction()
    {
        base.OnAction();
        if (food == null) return;
        actor.currentHealth = Mathf.Clamp(actor.currentHealth + consumeRate, 0, actor.totalHealthScaled);
        food.currentHealth -= consumeRate;
        food = null;
    }
}
