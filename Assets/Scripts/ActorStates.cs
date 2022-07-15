using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorStates : BaseState
{
    protected readonly Actor actor;
    public ActorStates(Actor sm) : base(sm)
    {
        actor = sm;
    }
}
public class IdleState : ActorStates
{
    public IdleState(Actor sm) : base(sm) { }
    public override void OnEnter()
    {
        base.OnEnter();
        //actor.anim.PlayInFixedTime(Actor.IdleKey);
    }
    public override void Update()
    {
        base.Update();
        //actor.ChangeState(new MoveState(actor));
    }
}
public class MoveState : ActorStates
{
    public MoveState(Actor sm) : base(sm) { }
    public override void OnEnter()
    {
        base.OnEnter();
        //actor.anim.PlayInFixedTime(Actor.MoveKey);
    }
    public override void Update()
    {
        base.Update();
        //actor.ChangeState(new IdleState(actor));
    }
}
