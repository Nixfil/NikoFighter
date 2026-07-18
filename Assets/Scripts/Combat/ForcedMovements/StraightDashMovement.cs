using UnityEngine;

public class StraightDashMovement : ForcedMovement
{
    private readonly Vector3 direction;
    private readonly float distance;
    private readonly float duration;

    private float travelled;

    public StraightDashMovement(
        Vector3 direction,
        float distance,
        float duration)
    {
        this.direction = direction.normalized;
        this.distance = distance;
        this.duration = duration;
    }

    public override void Begin(CharacterMovement3D movement)
    {
        base.Begin(movement);

        movement.SetRotationLocked(true);
    }

    public override Vector3 Tick(
        CharacterMovement3D movement,
        float deltaTime)
    {
        elapsedTime += deltaTime;

        float speed = distance / duration;
        float step = speed * deltaTime;

        travelled += step;

        if (travelled >= distance)
        {
            step -= travelled - distance;
            Finished = true;
        }

        return direction * step;
    }

    public override void End(CharacterMovement3D movement)
    {
        movement.SetRotationLocked(false);
    }
}