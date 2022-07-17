using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public BaseState currentState;
    public event System.Action<StateMachine> deathEvent;

    protected virtual void Start()
    {
        currentState = new BaseState(this);
    }
    protected virtual void Update()
    {
        currentState.Update();
    }
    public virtual void ChangeState(BaseState newState)
    {
        currentState.OnExit();
        newState.OnEnter();
        currentState = newState;
    }
    public void Die()
    {
        deathEvent?.Invoke(this);
    }
    public virtual void Despawn(Object obj = null)
    {
        deathEvent?.Invoke(this);
        if (obj == null)
            Destroy(gameObject);
        else
            Destroy(obj);
    }
    public virtual BaseState DefaultState()
    {
        return new BaseState(this);
    }
}