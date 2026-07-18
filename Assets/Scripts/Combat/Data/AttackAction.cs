using UnityEngine;

public abstract class AttackAction : ScriptableObject
{
    public abstract void Execute(AttackContext context);
}