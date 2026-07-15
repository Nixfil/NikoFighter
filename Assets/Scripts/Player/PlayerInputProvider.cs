using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputProvider : MonoBehaviour, IInputProvider
{
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction dashAction;
    private InputAction attackAction;

    public Vector2 MoveInput { get; private set; }
    public bool JumpTriggered { get; private set; }
    public bool DashTriggered { get; private set; }
    public bool AttackTriggered { get; private set; }

    private void Awake()
    {
        if (InputSystem.actions == null)
        {
            Debug.LogError("Project-wide Input Actions asset is missing!");
            return;
        }

        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        dashAction = InputSystem.actions.FindAction("Dash");
        attackAction = InputSystem.actions.FindAction("Attack");

        if (moveAction == null) Debug.LogError("Move action not found!");
        if (jumpAction == null) Debug.LogError("Jump action not found!");
        if (dashAction == null) Debug.LogError("Dash action not found!");
        if (attackAction == null) Debug.LogError("Attack action not found!");
    }

    private void Update()
    {
        MoveInput = moveAction?.ReadValue<Vector2>() ?? Vector2.zero;

        JumpTriggered = jumpAction != null && jumpAction.WasPressedThisFrame();
        DashTriggered = dashAction != null && dashAction.WasPressedThisFrame();
        AttackTriggered = attackAction != null && attackAction.WasPressedThisFrame();
    }
}