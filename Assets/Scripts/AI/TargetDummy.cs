using UnityEngine;

public class TargetDummy : MonoBehaviour, IDamageable
{
    [SerializeField] private float health = 100f;

    public void TakeHit(HitData hit)
    {
        health -= hit.Damage;

        Debug.Log(
            $"{name} took {hit.Damage} damage. " +
            $"Remaining HP: {health}");
    }
}