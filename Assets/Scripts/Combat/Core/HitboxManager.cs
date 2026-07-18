using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CombatController))]
public class HitboxManager : MonoBehaviour
{
    private CombatController combatController;

    private bool hitboxesEnabled;

    private readonly HashSet<IDamageable> hitTargets = new();

    private void Awake()
    {
        combatController = GetComponent<CombatController>();
    }

    private void Update()
    {
        if (!hitboxesEnabled)
            return;

        AttackData attack = combatController.CurrentAttack;

        if (attack == null)
            return;

        foreach (AttackHitbox hitbox in attack.hitboxes)
        {
            Pose hitboxPose = CombatHitboxUtility.ResolveHitboxPose(
                transform,
                attack,
                hitbox);

            Collider[] colliders = CombatHitboxUtility.Scan(
                hitbox,
                hitboxPose);

            foreach (Collider col in colliders)
            {
                IDamageable damageable =
                    col.GetComponentInParent<IDamageable>();

                if (damageable == null)
                    continue;

                if (hitTargets.Contains(damageable))
                    continue;

                hitTargets.Add(damageable);

                HitData hit = new HitData
                {
                    Damage = attack.damage,
                    HitPoint = col.ClosestPoint(transform.position),
                    Direction = transform.forward,
                    Attacker = gameObject,
                    Attack = attack
                };

                damageable.TakeHit(hit);
            }
        }
    }

    public void EnableHitboxes()
    {
        hitTargets.Clear();
        hitboxesEnabled = true;
    }

    public void DisableHitboxes()
    {
        hitboxesEnabled = false;
        hitTargets.Clear();
    }
}