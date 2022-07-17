using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Breed : BehaviourComponent
{
    [Tooltip("Will fuck when health is above this threshold")] [Range(0, 1)]
    public float hungerThreshold = 0.95f;

    // [Tooltip("How much HP to consume per action")]
    // public int consumeRate = 5;
    public float breedChance = 0.5f;
    public int minAge = 100;

    Actor partner = null;
    public Actor spawnPrefab;

    public override bool OnTick()
    {
        base.OnTick();
        if (actor.currentHealth < actor.totalHealthScaled * hungerThreshold) return false;
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
        partner = GetClosestPartner(actor.tag);
        if (partner != null)
        {
            if (ticks >= ticksPerAction)
            {
                OnAction();
                return true;
            }
        }

        return false;
    }

    Actor GetClosestPartner(string tag)
    {
        var allTiles = actor.currentTile.neighbours.Append(actor.currentTile).Where(t => t != null);
        return allTiles
            .SelectMany(t => t.actorStack.Where(a => a.CompareTag(tag) && a.currentAge >= minAge))
            .FirstOrDefault();
    }

    public override void OnAction()
    {
        //Subscribe to if the partner dies early THERE MAY BE AN ERROR HERE
        base.OnAction();
        if (partner == null) return;
        actor.ChangeState(new BreedState(actor, partner));
        //Only complete fuck if chance
        if (Random.value < breedChance)
        {
            BehaviourManager.i.SpawnActor(spawnPrefab, actor.currentTile);
        }

        partner = null;
    }
}