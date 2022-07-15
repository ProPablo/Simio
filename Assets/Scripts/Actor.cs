using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : StateMachine
{
    [HideInInspector] public Animator anim;
    public RuntimeAnimatorController[] spriteVariants;
    public Direction lastDir;
    public int ticksPerAction = 3;
    public int tickCounter;
    public BehaviourComponent[] behaviours;
    #region Animation Keys
    public static readonly int IdleDownKey = Animator.StringToHash("IdleDown");
    public static readonly int IdleLeftKey = Animator.StringToHash("IdleLeft");
    public static readonly int IdleRightKey = Animator.StringToHash("IdleRight");
    public static readonly int IdleUpKey = Animator.StringToHash("IdleUp");
    public static readonly int MoveDownKey = Animator.StringToHash("MoveDown");
    public static readonly int MoveLeftKey = Animator.StringToHash("MoveLeft");
    public static readonly int MoveRightKey = Animator.StringToHash("MoveRight");
    public static readonly int MoveUpKey = Animator.StringToHash("MoveUp");
    #endregion
    private void Awake()
    {
        anim = GetComponent<Animator>();
        behaviours = GetComponents<BehaviourComponent>();
        anim.runtimeAnimatorController = spriteVariants[Random.Range(0, spriteVariants.Length)];
        foreach (var behaviour in behaviours)
            behaviour.actor = this;
    }
    protected override void Start()
    {
        base.Start();
        tickCounter = ticksPerAction;
        currentState = new IdleState(this);
        currentState.OnEnter();
    }
}
