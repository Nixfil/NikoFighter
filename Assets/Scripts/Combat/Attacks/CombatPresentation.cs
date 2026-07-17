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

        if (attack.presentation.attackVFXPrefab == null)
            return;

        PlayPresentation(attack.presentation.attackVFXPrefab);
    }

    public void PlayHitPresentation()
    {
        AttackData attack = combatController.CurrentAttack;

        if (attack == null)
            return;

        if (attack.presentation == null)
            return;

        if (attack.presentation.impactVFXPrefab == null)
            return;

        PlayPresentation(attack.presentation.impactVFXPrefab);

        // Future additions:
        // Camera Shake
        // Hit Stop
        // Audio
        // Controller Rumble
    }

    private void PlayPresentation(GameObject prefab)
    {
        Instantiate(
            prefab,
            transform.position,
            transform.rotation);
    }
}