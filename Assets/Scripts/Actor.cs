using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : StateMachine
{
    [HideInInspector] public Animator anim;
    public ParticleSystem bloodParticles, foodParticles;
    public RuntimeAnimatorController[] spriteVariants;
    [Header("Stat Allocation")]
    public int baseHealth;
    public int maxAge = int.MaxValue;
    public int baseAttack;
    public Diet diet;
    public ActorType type;
    public CellType[] walkable;
    [Header("Current Stats")]
    public int totalHealthScaled;
    public int currentHealth;
    public int currentAge = 0;
    public int attackScaled;
    [Header("Debug Info")]
    public Direction lastDir;
    public HexCell currentTile;
    public string currentBehaviour;
    public BehaviourComponent[] behaviours;
    public SpriteRenderer alertSprite;
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
        // alertSprite = GetComponentInChildren<SpriteRenderer>();
    }
    protected override void Start()
    {
        base.Start();
        currentState = new IdleState(this);
        currentState.OnEnter();
        alertSprite.enabled = false;
    }
    private void LateUpdate()
    {
        transform.LookAt(transform.position + Camera.main.transform.forward);
        if (currentHealth <= 0)
            Despawn();
    }
    public override void Despawn(Object obj = null)
    {
        BehaviourManager.i.SpawnCorpse(currentTile);
        currentTile.LeaveCell(this);
        base.Despawn(obj);
    }
}
