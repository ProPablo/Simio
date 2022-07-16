using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Eat : BehaviourComponent
{
    public int ticksToEat = 2;
    private int ticksToNextEat = int.MaxValue;
    Actor food = null;
    public override bool OnTick()
    {
        base.OnTick();
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
            if (ticksToNextEat >= ticksToEat)
            {
                OnAction();
                ticksToNextEat = 0;
                return true;
            }
            ticksToNextEat++;
        }
        return false;
    }
    public override void OnAction()
    {
        base.OnAction();
        if (food == null) return;
        actor.currentHealth = Mathf.Clamp(actor.currentHealth + 1, 0, actor.totalHealth);
        food.currentHealth--;
        food = null;
    }
}
