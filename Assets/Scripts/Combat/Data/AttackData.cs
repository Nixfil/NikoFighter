using System;
using UnityEngine;
using UnityEngine.VFX;


[CreateAssetMenu(fileName = "Attack", menuName = "Combat/Attack Data")]
public class AttackData : ScriptableObject
{
    [Header("General")]
    public string attackName;

    [Header("Animator")]
    public AnimatorStateReference animatorState;

    [Header("Combat")]

    [Min(0)]
    public float damage = 10f;

    public float knockback = 5f;

    public float hitstop = 0.05f;

    public HitReaction hitReaction = HitReaction.Light;

    [SerializeField]
    private AttackActionGroup[] actionGroups;

    public AttackActionGroup[] ActionGroups => actionGroups;

    [Header("Presentation")]

    public AttackPresentation presentation;

    [Header("Anchors")]
    public AttackAnchor[] anchors;

    [Header("Hitboxes")]
    public AttackHitbox[] hitboxes;

    [Header("Movement")]

    public bool allowRotation = true;

    public bool interruptible = false;

    [Header("Transitions")]

    public AttackTransition[] comboTransitions;

    public AttackAnchor GetAnchor(AnchorReference reference)
    {
        if (reference == null)
            return null;

        foreach (AttackAnchor anchor in anchors)
        {
            if (reference.type == AnchorType.Custom)
            {
                if (anchor.type == AnchorType.Custom &&
                    anchor.customName == reference.customName)
                {
                    return anchor;
                }
            }
            else
            {
                if (anchor.type == reference.type)
                {
                    return anchor;
                }
            }
        }

        return null;
    }
}

[Serializable]
public class AttackTransition
{
    public CombatInput requiredInput;

    public AttackData nextAttack;
}

public enum HitReaction
{
    None,
    Light,
    Medium,
    Heavy,
    Launch
}

public enum CombatInput
{
    Light,
    Heavy,
    Dash,
    Special
}

public enum CombatState
{
    None,

    GroundLight,
    GroundHeavy,

    AirLight,
    AirHeavy,

    DashAttack,

    SprintAttack,

    ChargedAttack,

    Special
}

[System.Serializable]
public class AttackActionGroup
{
    public string eventName;

    public AttackAction[] actions;
}