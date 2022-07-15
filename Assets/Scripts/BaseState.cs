using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BaseState
{
    [HideInInspector] public StateMachine sm;
    [SerializeField] public string stateName = "Base State";
    public float duration = 0;
    public float age = float.MaxValue;
    protected float _elapsedTime => duration - age;
    public BaseState(StateMachine sm)
    {
        this.sm = sm;
        stateName = GetType().Name;
    }
    public virtual void OnEnter()
    {
        if (duration != 0)
            age = duration;
    }
    public virtual void Update()
    {
        if (duration == 0) return;
        age -= Time.deltaTime;
        if (age <= 0)
        {
            sm.ChangeState(sm.DefaultState());
        }
    }
    public virtual void OnExit()
    {
    }
}
