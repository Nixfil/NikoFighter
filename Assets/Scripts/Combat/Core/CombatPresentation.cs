using UnityEngine;

[RequireComponent(typeof(CombatController))]
public class CombatPresentation : MonoBehaviour
{
    private CombatController combatController;

    private void Awake()
    {
        combatController = GetComponent<CombatController>();

        if (combatController == null)
        {
            Debug.LogError("CombatController not found.");
        }
    }

    public void PlayAttackPresentation()
    {
        AttackData attack = combatController.CurrentAttack;

        if (attack == null)
            return;

        if (attack.presentation == null)
            return;

        foreach (AttackEffect effect in attack.presentation.attackEffects)
        {
            SpawnEffect(effect, attack);
        }
    }

    public void PlayHitPresentation(HitData hit)
    {
        AttackData attack = hit.Attack;

        if (attack == null)
            return;

        if (attack.presentation == null)
            return;

        foreach (AttackEffect effect in attack.presentation.hitEffects)
        {
            SpawnEffect(effect, attack, hit);
        }
    }

    private void SpawnEffect(
    AttackEffect effect,
    AttackData attack,
    HitData? hit = null)
    {
        if (effect.prefab == null)
            return;

        Pose pose;

        switch (effect.spawnLocation)
        {
            case EffectSpawnLocation.Anchor:
                {
                    AttackAnchor anchor =
                        attack.GetAnchor(effect.anchor);

                    if (anchor == null)
                    {
                        Debug.LogWarning($"Missing anchor for effect '{effect.prefab.name}'.");
                        return;
                    }

                    pose = AnchorResolver.Resolve(
                        transform,
                        anchor);

                    break;
                }

            case EffectSpawnLocation.HitPoint:
                {
                    if (!hit.HasValue)
                        return;

                    pose = new Pose(
                        hit.Value.HitPoint,
                        Quaternion.LookRotation(hit.Value.Direction));

                    break;
                }

            default:
                return;
        }

        GameObject spawned = Instantiate(
            effect.prefab,
            pose.position,
            pose.rotation);

        if (effect.followMode == EffectFollowMode.FollowPlayer &&
            effect.spawnLocation == EffectSpawnLocation.Anchor)
        {
            spawned.transform.SetParent(transform, true);
        }
    }
}