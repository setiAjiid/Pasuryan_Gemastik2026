
public class StateMachine
{
    public IState currState;
    public void Init(IState startState)
    {
        currState = startState;
    }

    public void ChangeState(IState newState)
    {
        currState?.Exit();
        currState = newState;
        currState?.Enter();
    }

    public void OnUpdate()
    {
        currState?.OnUpdate();
    }

    public void OnFixedUpdate()
    {
        currState?.OnFixedUpdate();
    }
}
