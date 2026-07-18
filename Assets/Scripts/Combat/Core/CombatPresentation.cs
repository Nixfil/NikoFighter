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

    public void PlayHitPresentation()
    {
        AttackData attack = combatController.CurrentAttack;

        if (attack == null)
            return;

        if (attack.presentation == null)
            return;

        foreach (AttackEffect effect in attack.presentation.hitEffects)
        {
            SpawnEffect(effect, attack);
        }

        // Future:
        // Camera Shake
        // Audio
        // Hit Stop
        // Controller Rumble
    }

    private void SpawnEffect(AttackEffect effect, AttackData attack)
    {
        if (effect.prefab == null)
            return;

        AttackAnchor anchor =
            attack.GetAnchor(effect.anchor);

        if (anchor == null)
        {
            Debug.LogWarning($"Missing anchor for effect '{effect.prefab.name}'.");
            return;
        }

        Pose pose =
            AnchorResolver.Resolve(
                transform,
                anchor);

        GameObject spawned = Instantiate(
    effect.prefab,
    pose.position,
    pose.rotation);

        if (effect.followMode == EffectFollowMode.FollowPlayer)
        {
            spawned.transform.SetParent(transform, true);
        }
    }
}