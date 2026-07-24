using UnityEngine;

public class HandOutState : IState
{
    private readonly PlayerStateContext ctx;
    private float timer;

    public HandOutState(PlayerStateContext ctx)
    {
        this.ctx = ctx;
    }
    public void Enter()
    {
        timer = 0;
        ctx.Animator.SetInteger("AnimState", 2);
        ctx.Movement.Face(ctx.Input.MoveInput.x); //hadap arah jln tnpa gerak
    }

    public void Exit()
    {
    }

    public void OnFixedUpdate()
    {
        ctx.Movement.Stop();
    }

    public void OnUpdate()
    {
        if (Mathf.Abs(ctx.Input.MoveInput.x) < 0.01f)
        {
            ctx.StateMachine.ChangeState(ctx.IdleState);
            return;
        }

        timer += Time.deltaTime;
        if (timer >= ctx.HandOutTime)
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
