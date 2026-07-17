using UnityEngine;

public class CombatAnimationEvents : MonoBehaviour
{
    private CombatController combatController;
    private CombatPresentation combatPresentation;

    private void Awake()
    {
        combatController = GetComponent<CombatController>();
        combatPresentation = GetComponent<CombatPresentation>();

        if (combatController == null)
            Debug.LogError("CombatController not found.");

        if (combatPresentation == null)
            Debug.LogError("CombatPresentation not found.");
    }

    //==================================================
    // Hitboxes
    //==================================================

    public void AnimationEvent_HitboxStart()
    {
        Debug.Log("<color=green>Hitbox ON</color>");

        // HitboxManager.EnableCurrentHitbox();
    }

    public void AnimationEvent_HitboxEnd()
    {
        Debug.Log("<color=red>Hitbox OFF</color>");

        // HitboxManager.DisableCurrentHitbox();
    }

    //==================================================
    // Combo
    //==================================================

    public void AnimationEvent_OpenComboWindow()
    {
        Debug.Log("<color=yellow>Combo Window Open</color>");

        combatController.OpenComboWindow();
    }

    public void AnimationEvent_CloseComboWindow()
    {
        Debug.Log("<color=yellow>Combo Window Closed</color>");

        combatController.CloseComboWindow();
    }

    //==================================================
    // Attack
    //==================================================

    public void AnimationEvent_AttackFinished()
    {
        Debug.Log("<color=cyan>Attack Finished</color>");

        combatController.FinishAttack();
    }

    //==================================================
    // VFX
    //==================================================

    //==================================================
    // Presentation
    //==================================================

    public void AnimationEvent_AttackPresentation()
    {
        Debug.Log("Attack Presentation Event");
        combatPresentation.PlayAttackPresentation();
    }

    public void AnimationEvent_HitPresentation()
    {
        combatPresentation.PlayHitPresentation();
    }
}