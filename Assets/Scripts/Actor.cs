using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : StateMachine
{
    [HideInInspector] public Animator anim;
    public Vector2 moveDir;
    public int ticksPerAction = 3;
    public TickComponent[] behaviours;
    #region Animation Keys
    public static readonly int IdleKey = Animator.StringToHash("Idle");
    public static readonly int MoveKey = Animator.StringToHash("Move");
    public static readonly int XKey = Animator.StringToHash("xLast");
    public static readonly int YKey = Animator.StringToHash("yLast");
    #endregion
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    protected override void Start()
    {
        base.Start();
        currentState = new IdleState(this);
        currentState.OnEnter();
    }
    protected override void Update()
    {
        base.Update();
        if (moveDir != Vector2.zero)
        {
            anim.SetFloat(XKey, moveDir.x);
            anim.SetFloat(YKey, moveDir.y);
        }
    }
    protected virtual void Tick()
    {
        // Put this in tick manager
        foreach (var item in behaviours)
        {
            item.OnTick();
        }
    }
}
