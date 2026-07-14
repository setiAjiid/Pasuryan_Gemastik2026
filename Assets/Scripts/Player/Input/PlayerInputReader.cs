using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputReader : MonoBehaviour
{
    public Vector2 MoveInput { get; private set; }
    public bool IsRun { get; private set; }

    public event System.Action OnInteractPressed;

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
        controls.Player.Interact.performed += OnInteract;
    }

    private void OnDisable()
    {
        controls.Player.Move.performed -= OnMove;
        controls.Player.Move.canceled -= OnMove;
        controls.Player.Sprint.performed -= OnRun;
        controls.Player.Sprint.canceled -= OnRun;
        controls.Player.Interact.performed -= OnInteract;
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

    private void OnInteract(InputAction.CallbackContext ctx)
    {
        OnInteractPressed?.Invoke();
    }

    public void DisableMove()
    {
        controls.Player.Move.Disable();
        controls.Player.Sprint.Disable();
        MoveInput = Vector2.zero;
        IsRun = false;
    }

    public void EnableMove()
    {
        controls.Player.Move.Enable();
        controls.Player.Sprint.Enable();
    }
}
