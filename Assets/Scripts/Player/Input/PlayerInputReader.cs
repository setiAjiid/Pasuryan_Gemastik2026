using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputReader : MonoBehaviour
{
    public Vector2 MoveInput { get; private set; }
    public Vector2 NavigateInput { get; private set; } //nilai mentah wasd for minigames
    public bool IsRun { get; private set; }

    public event System.Action OnInteractPressed;
    public event System.Action<int> OnNavigatePressed;

    private InputSystem_Actions controls;
    private int lastNavDir; //track last pencetan biar ga geser terus


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
        controls.UI.Navigate.performed += OnNavigate;
    }

    private void OnDisable()
    {
        controls.Player.Move.performed -= OnMove;
        controls.Player.Move.canceled -= OnMove;
        controls.Player.Sprint.performed -= OnRun;
        controls.Player.Sprint.canceled -= OnRun;
        controls.Player.Interact.performed -= OnInteract;
        controls.UI.Navigate.performed -= OnNavigate;
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

    private void OnNavigate(InputAction.CallbackContext ctx)
    {
        Vector2 val = ctx.ReadValue<Vector2>();
        NavigateInput = val;

        int dir = 0;
        if (val.x > 0.5f) dir = 1;
        else if (val.x < -0.5f) dir = -1;

        if (dir == lastNavDir) return;
        lastNavDir = dir;
        if (dir != 0) OnNavigatePressed?.Invoke(dir);
    }
    public void EnableMove()
    {
        controls.Player.Move.Enable();
        controls.Player.Sprint.Enable();
    }

    public void DisableMove()
    {
        controls.Player.Move.Disable();
        controls.Player.Sprint.Disable();
        MoveInput = Vector2.zero;
        IsRun = false;
    }


    public void EnableNavigate()
    {
        lastNavDir = 0;
        controls.UI.Navigate.Enable();
    }

    public void DisableNavigate()
    {
        controls.UI.Navigate.Disable();
        NavigateInput = Vector2.zero;
        lastNavDir = 0;
    }
}
