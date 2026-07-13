using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputReader : MonoBehaviour
{
    public Vector2 MoveInput { get; private set; }
    public bool IsRun { get; private set; }

    private InputSystem_Actions controls;


    private void Awake()
    {
        controls = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        controls.Player.Enable();
        controls.Player.Move.performed += OnMove;
        controls.Player.Move.canceled += OnMove;
        controls.Player.Sprint.performed += OnRun;
        controls.Player.Sprint.canceled += OnRun;
    }

    private void OnDisable()
    {
        controls.Player.Move.performed -= OnMove;
        controls.Player.Move.canceled -= OnMove;
        controls.Player.Sprint.performed -= OnRun;
        controls.Player.Sprint.canceled -= OnRun;
        controls.Player.Disable();
    }

    private void OnMove(InputAction.CallbackContext ctx)
    {
        MoveInput = ctx.ReadValue<Vector2>();
    }

    private void OnRun(InputAction.CallbackContext ctx)
    {
        IsRun = ctx.performed;
    }
}
