using UnityEngine;

public class WalkState : IState
{
    private readonly PlayerStateContext ctx;
    public WalkState(PlayerStateContext ctx)
    {
        this.ctx = ctx;
    }

    public void Enter()
    {
        ctx.Animator.SetInteger("AnimState", 1);
    }

    public void Exit()
    {
    }

    public void OnFixedUpdate()
    {
        ctx.Movement.Move(ctx.Input.MoveInput.x, ctx.WalkSpeed);
    }

    public void OnUpdate()
    {
        if (Mathf.Abs(ctx.Input.MoveInput.x) < 0.01f)
        {
            ctx.StateMachine.ChangeState(ctx.HandInState);
        } else if (ctx.Input.IsRun) 
        {
            ctx.StateMachine.ChangeState(ctx.RunState);
        }
    }
}
