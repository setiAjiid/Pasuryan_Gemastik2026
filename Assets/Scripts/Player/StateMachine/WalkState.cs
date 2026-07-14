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
            ctx.StateMachine.ChangeState(ctx.IdleState);
        } else if (ctx.Input.IsRun) 
        {
            ctx.StateMachine.ChangeState(ctx.RunState);
        }
    }
}
