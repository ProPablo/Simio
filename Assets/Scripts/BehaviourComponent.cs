using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    N,
    NE,
    SE,
    S,
    SW,
    NW
}
public class BehaviourComponent : MonoBehaviour
{
    [HideInInspector] public Actor actor;
    public virtual void OnAction() { }
    public virtual void OnTick()
    {
        actor.tickCounter--;
        if (actor.tickCounter <= 0)
        {
            OnAction();
            actor.tickCounter = actor.ticksPerAction;
        }
    }
}
