using UnityEngine;
using UnityEngine.Rendering;

public class PlayerStateContext
{
    public PlayerInputReader Input;
    public PlayerMovement Movement;
    public StateMachine StateMachine;
    public Animator Animator;

    public float WalkSpeed;
    public float RunSpeed;

    //durasi
    public float HandOutTime;
    public float HandInTime;
    public float HandProgress;
    
    //state
    public IdleState IdleState;
    public WalkState WalkState;
    public RunState RunState;
    public HandInState HandInState;
    public HandOutState HandOutState;
}
