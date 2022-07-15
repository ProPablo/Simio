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
    public override void Update()
    {
        if (duration == 0) return;
        age -= Time.deltaTime;
        if (age <= 0)
            actor.ChangeState(new IdleState(actor));
    }
}
public class IdleState : ActorStates
{
    public IdleState(Actor sm) : base(sm) { }
    public override void OnEnter()
    {
        base.OnEnter();
        switch (actor.lastDir)
        {
            case Direction.N:
                actor.anim.PlayInFixedTime(Actor.IdleUpKey);
                break;
            case Direction.NE:
                actor.anim.PlayInFixedTime(Actor.IdleRightKey);
                break;
            case Direction.SE:
                actor.anim.PlayInFixedTime(Actor.IdleRightKey);
                break;
            case Direction.S:
                actor.anim.PlayInFixedTime(Actor.IdleDownKey);
                break;
            case Direction.SW:
                actor.anim.PlayInFixedTime(Actor.IdleLeftKey);
                break;
            case Direction.NW:
                actor.anim.PlayInFixedTime(Actor.IdleLeftKey);
                break;
        }
    }
    public override void Update()
    {
        base.Update();
        //actor.ChangeState(new MoveState(actor));
    }
}
public class MoveState : ActorStates
{
    Vector3 currentPos;
    readonly Vector3 targetPos;
    public MoveState(Actor sm, Vector3 _targetPos) : base(sm)
    {
        duration = BehaviourManager.i.tickDur;
        targetPos = _targetPos;
    }
    public override void OnEnter()
    {
        base.OnEnter();
        currentPos = actor.transform.position;
        switch (actor.lastDir)
        {
            case Direction.N:
                actor.anim.PlayInFixedTime(Actor.MoveUpKey);
                break;
            case Direction.NE:
                actor.anim.PlayInFixedTime(Actor.MoveRightKey);
                break;
            case Direction.SE:
                actor.anim.PlayInFixedTime(Actor.MoveRightKey);
                break;
            case Direction.S:
                actor.anim.PlayInFixedTime(Actor.MoveDownKey);
                break;
            case Direction.SW:
                actor.anim.PlayInFixedTime(Actor.MoveLeftKey);
                break;
            case Direction.NW:
                actor.anim.PlayInFixedTime(Actor.MoveLeftKey);
                break;
        }
    }
    public override void Update()
    {
        base.Update();
        actor.transform.position = KongrooUtils.SlerpCenter(currentPos, targetPos, (currentPos + targetPos) / 2 + new Vector3(0, -BehaviourManager.i.slerpCentre, 0), 1 - age / duration);
    }
    public override void OnExit()
    {
        base.OnExit();
        actor.transform.position = targetPos;
    }
}
