using UnityEngine;

public class IdleState : IState
{
    private readonly PlayerStateContext ctx;
    public IdleState(PlayerStateContext ctx)
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
    }

    public void OnUpdate()
    {
        if (Mathf.Abs(ctx.Input.MoveInput.x) > 0.01f)
        {
            if (ctx.Input.IsRun)
            {
                ctx.StateMachine.ChangeState(ctx.RunState);
            } else
            {
                ctx.StateMachine.ChangeState(ctx.WalkState);
            }
        }
    }

}
