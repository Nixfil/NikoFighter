using UnityEngine;

public interface IInputProvider
{
    Vector2 MoveInput { get; }
    bool JumpTriggered { get; }

    bool DashTriggered { get; }
    bool LightAttackTriggered { get; }

    bool HeavyAttackTriggered { get; }
}