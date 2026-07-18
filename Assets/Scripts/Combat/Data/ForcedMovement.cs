using UnityEngine;

public abstract class ForcedMovement
{
    protected float elapsedTime;

    public bool Finished { get; protected set; }

    public virtual void Begin(CharacterMovement3D movement)
    {
        elapsedTime = 0f;
        Finished = false;
    }

    public abstract Vector3 Tick(
        CharacterMovement3D movement,
        float deltaTime);

    public virtual void End(CharacterMovement3D movement)
    {

    }
}