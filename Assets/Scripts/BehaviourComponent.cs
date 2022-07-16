using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BehaviourComponent : MonoBehaviour
{
    //public Actor actor { get; set; }
    [HideInInspector] public Actor actor;
    [HideInInspector] public string Name;
    [HideInInspector] public int ticks = 0;
    public int ticksPerAction;
    public virtual void Start()
    {
        Name = GetType().Name;
    }
    public virtual bool OnTick()
    {
        return false;
    }
    public virtual void OnAction()
    {
        actor.currentHealth--;
    }
}
