using UnityEngine;

public interface IInputProvider
{
    Vector2 MoveInput { get; }
    bool JumpTriggered { get; }

    bool DashTriggered { get; }
    bool AttackTriggered { get; }
}