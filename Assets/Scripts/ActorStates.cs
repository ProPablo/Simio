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
    readonly HexCell targetCell;
    readonly Direction targetDir;
    Vector3 targetPos;
    public MoveState(Actor sm, Direction _targetDir, HexCell _targetCell) : base(sm)
    {
        duration = BehaviourManager.i.tickDur;
        targetDir = _targetDir;
        targetCell = _targetCell;
    }
    public override void OnEnter()
    {
        base.OnEnter();
        currentPos = actor.transform.position;
        targetPos = targetCell.transform.position + new Vector3(0, targetCell.startingStackOffset, 0);
        actor.lastDir = targetDir;
        switch (targetDir)
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
        actor.currentTile.LeaveCell(actor);
        
    }
    public override void Update()
    {
        base.Update();
        actor.transform.position = KongrooUtils.SlerpCenter(currentPos, targetPos, (currentPos + targetPos) / 2 + new Vector3(0, -BehaviourManager.i.slerpCentre, 0), 1 - age / duration);
    }
    public override void OnExit()
    {
        base.OnExit();
        targetCell.JoinCell(actor);
        actor.currentTile = targetCell;
    }
}
public class AlertedMove : MoveState
{
    public AlertedMove(Actor sm, Direction _targetDir, HexCell _targetCell) : base(sm, _targetDir, _targetCell)
    {
        duration = BehaviourManager.i.tickDur;
    }
    public override void OnEnter()
    {
        base.OnEnter();
        // actor.ps.Emit(1);
        actor.alertSprite.enabled = true;
    }

    public override void OnExit()
    {
        base.OnExit();
        actor.alertSprite.enabled = false;
    }
}

public class AlertedStay : IdleState
{
    public AlertedStay(Actor sm) : base(sm )
    {
        duration = BehaviourManager.i.tickDur;
    }
    public override void OnEnter()
    {
        base.OnEnter();
        actor.alertSprite.enabled = true;
    }

    public override void OnExit()
    {
        base.OnExit();
        actor.alertSprite.enabled = false;
    }
}
public class AttackState : AlertedStay
{
    private Actor target;

    public AttackState(Actor sm, Actor target) : base(sm)
    {
        duration = BehaviourManager.i.tickDur;
        this.target = target;
    }
    public override void OnEnter()
    {
        base.OnEnter();
        target.ChangeState(new InjuredState(target, actor.attackScaled));
    }
}

public class InjuredState : IdleState
{
    private int _damageTaken;
    public InjuredState(Actor sm, int damageTaken) : base(sm)
    {
        duration = BehaviourManager.i.tickDur;
        _damageTaken = damageTaken;
    }
    public override void OnEnter()
    {
        base.OnEnter();
        actor.currentHealth -= _damageTaken;
        actor.ps.Emit(_damageTaken);
    }

}
