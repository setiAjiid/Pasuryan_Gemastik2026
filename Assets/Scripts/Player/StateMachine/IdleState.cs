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
        ctx.Animator.SetInteger("AnimState", 0);
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
        }
    }

}
