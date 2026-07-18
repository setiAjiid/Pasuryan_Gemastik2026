using UnityEngine;

[RequireComponent(typeof(PlayerInputReader), typeof(PlayerMovement))]
public class PlayerStateMachine : MonoBehaviour
{
    [SerializeField] private float walkSpeed = 3f;
    [SerializeField] private float runSpeed = 5f;
    [SerializeField] private float handOutTime = 0.12f;
    [SerializeField] private float handInTime = 0.2f;

    private readonly PlayerStateContext ctx = new();
    private StateMachine stateMachine;

    private void Awake()
    {
        stateMachine = new StateMachine();

        ctx.Animator = GetComponentInChildren<Animator>();
        ctx.HandOutTime = handOutTime;
        ctx.HandInTime = handInTime;
        ctx.StateMachine = stateMachine;
        ctx.WalkSpeed = walkSpeed;
        ctx.RunSpeed = runSpeed;
        ctx.Input = GetComponent<PlayerInputReader>();
        ctx.Movement = GetComponent<PlayerMovement>();
        ctx.IdleState = new(ctx);
        ctx.WalkState = new(ctx);
        ctx.RunState = new(ctx);
        ctx.HandInState = new(ctx);
        ctx.HandOutState = new(ctx);


    }

    void Start()
    {
        stateMachine.Init(ctx.IdleState);
    }

    void Update()
    {
        stateMachine.OnUpdate();
    }

    private void FixedUpdate()
    {
        stateMachine.OnFixedUpdate();
    }
}