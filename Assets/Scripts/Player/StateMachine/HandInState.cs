using UnityEngine;

public class HandInState : IState
{
    private readonly PlayerStateContext ctx;
    private float timer;

    public HandInState(PlayerStateContext ctx)
    {
        this.ctx = ctx;
    }
    public void Enter()
    {
        //main task: sisa waktu cmn sebnyak tangan yg udah keluar sekarang
        timer = (1f - ctx.HandProgress) * ctx.HandInTime; 
        ctx.Animator.SetInteger("AnimState", 3);
        ctx.Animator.Play("HandIn", 0, 1f - ctx.HandProgress);
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
        ctx.HandProgress = 1f - Mathf.Clamp01(timer / ctx.HandInTime);
        if (Mathf.Abs(ctx.Input.MoveInput.x) > 0.01f)
        {
            ctx.StateMachine.ChangeState(ctx.HandOutState);
            return;
        }

        if (timer >= ctx.HandInTime)
        {
            ctx.StateMachine.ChangeState(ctx.IdleState);
        }
    }
}
