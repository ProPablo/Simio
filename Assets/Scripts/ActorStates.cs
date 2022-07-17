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
    public IdleState(Actor sm) : base(sm)
    {
    }

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
        actor.transform.position = KongrooUtils.SlerpCenter(currentPos, targetPos,
            (currentPos + targetPos) / 2 + new Vector3(0, -BehaviourManager.i.slerpCentre, 0), 1 - age / duration);
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
    public AlertedStay(Actor sm) : base(sm)
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

public class AttackState : IdleState
{
    private Actor target;
    readonly float halfwayPoint;
    readonly float pounceDist = 0.5f;
    Vector3 startPos;

    public AttackState(Actor sm, Actor target) : base(sm)
    {
        duration = BehaviourManager.i.tickDur;
        halfwayPoint = duration * 0.5f;
        this.target = target;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        target.ChangeState(new InjuredState(target, actor.attackScaled));
        startPos = actor.transform.localPosition;
    }

    public override void Update()
    {
        base.Update();
        Vector3 targetPos = startPos + AssetDB.DirectionVectors[(int) actor.lastDir].vec * pounceDist;
        if (age > halfwayPoint)
            actor.transform.localPosition = Vector3.Lerp(startPos, targetPos,
                KongrooUtils.RemapRange(age, duration, halfwayPoint, 0, 1));
        else
            actor.transform.localPosition =
                Vector3.Lerp(targetPos, startPos, KongrooUtils.RemapRange(age, halfwayPoint, 0, 0, 1));
    }

    public override void OnExit()
    {
        base.OnExit();
        actor.transform.localPosition = startPos;
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
        actor.bloodParticles.Emit(_damageTaken);
    }

    public override void OnExit()
    {
        base.OnExit();
        actor.bloodParticles.Stop();
    }
}

public class EatingState : IdleState
{
    private readonly Actor target;
    private int healAmount;

    public EatingState(Actor sm, Actor _target, int _healAmount) : base(sm)
    {
        duration = BehaviourManager.i.tickDur;
        target = _target;
        healAmount = _healAmount;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        actor.currentHealth = Mathf.Clamp(actor.currentHealth + healAmount, 0, actor.totalHealthScaled);
        target.currentHealth -= healAmount;
        // actor.currentHealth += healAmount;
        // target.currentHealth -= healAmount;
        //actor.foodParticles.Play();
        actor.foodParticles.Emit(healAmount);
    }
    //public override void OnExit()
    //{
    //    base.OnExit();
    //    actor.foodParticles.Stop();
    //}
}

public class BreedState : IdleState
{
    private Actor target;

    // readonly float[] halfwayPoint;
    readonly float humpDist = 0.5f;
    Vector3 startPos;

    //This is normalized to total duration
    private readonly float humpInterval = 0.1f;

    public BreedState(Actor sm, Actor target) : base(sm)
    {
        duration = BehaviourManager.i.tickDur;
        // halfwayPoint = duration * 0.5f;
        this.target = target;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        target.ChangeState(new InjuredState(target, actor.attackScaled));
        startPos = actor.transform.localPosition;
    }

    public override void Update()
    {
        base.Update();
        Vector3 targetPos = startPos + AssetDB.DirectionVectors[(int) actor.lastDir].vec * humpDist;
        var currentHumpNormalized = Mathf.Sin(((2 * Mathf.PI) / (humpInterval * duration)) * age);
        actor.transform.localPosition = Vector3.LerpUnclamped(startPos, targetPos, currentHumpNormalized);

        // Vector3.Lerp(startPos, targetPos, KongrooUtils.RemapRange(age, duration, halfwayPoint, 0, 1));
        // if (age > halfwayPoint)
        //     actor.transform.localPosition = Vector3.Lerp(startPos, targetPos, KongrooUtils.RemapRange(age, duration, halfwayPoint, 0, 1));
        // else
        //     actor.transform.localPosition = Vector3.Lerp(targetPos, startPos, KongrooUtils.RemapRange(age, halfwayPoint, 0, 0, 1));
    }

    public override void OnExit()
    {
        base.OnExit();
        actor.transform.localPosition = startPos;
    }
}