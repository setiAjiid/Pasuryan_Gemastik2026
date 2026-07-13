public class PlayerStateContext
{
    public PlayerInputReader Input;
    public PlayerMovement Movement;
    public StateMachine StateMachine;

    public float WalkSpeed;
    public float RunSpeed;
    
    //state
    public IdleState IdleState;
    public WalkState WalkState;
    public RunState RunState;
}
