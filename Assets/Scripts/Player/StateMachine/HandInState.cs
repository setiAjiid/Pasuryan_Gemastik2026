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
        timer = 0f;
        ctx.Animator.SetInteger("AnimState", 3);
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
            ctx.StateMachine.ChangeState(ctx.HandOutState);
            return;
        }

        timer += Time.deltaTime;
        if (timer >= ctx.HandInTime)
        {
            ctx.StateMachine.ChangeState(ctx.IdleState);
        }
    }
}
