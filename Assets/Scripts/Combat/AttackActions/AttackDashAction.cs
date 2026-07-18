using UnityEngine;

[CreateAssetMenu(menuName = "Combat/Actions/Dash")]
public class AttackDashAction : AttackAction
{
    [Header("Dash")]
    [SerializeField] private float distance = 3f;
    [SerializeField] private float duration = 0.15f;

    public override void Execute(AttackContext context)
    {
        CharacterMovement3D movement =
            context.Attacker.GetComponent<CharacterMovement3D>();

        if (movement == null)
            return;

        movement.BeginForcedMovement(
            new StraightDashMovement(
                context.Transform.forward,
                distance,
                duration));
    }
}