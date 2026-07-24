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
        //main task: lanjut dari kondisi tgn sekarang
        timer = ctx.HandProgress * ctx.HandOutTime;
        ctx.Animator.SetInteger("AnimState", 2);
        ctx.Animator.Play("HandOut", 0, ctx.HandProgress);
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
        timer += Time.deltaTime;
        ctx.HandProgress = Mathf.Clamp01(timer / ctx.HandOutTime);
        if (Mathf.Abs(ctx.Input.MoveInput.x) < 0.01f) //klo berhenti
        {
            ctx.StateMachine.ChangeState(ctx.HandInState);
            return;
        }

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
