using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Combat/Weapon Data")]
public class WeaponData : ScriptableObject
{
    [Header("General")]
    public string weaponName;

    [Header("Entry Attacks")]

    public AttackData groundLightAttack;
    public AttackData groundHeavyAttack;

    public AttackData airLightAttack;
    public AttackData airHeavyAttack;

    public AttackData dashAttack;

    public AttackData sprintAttack;

    public AttackData chargedAttack;

    public AttackData specialAttack;

    [Header("Animator")]
    [SerializeField]
    private string animatorStateMachine = "Sword Combat";

    public string AnimatorStateMachine => animatorStateMachine;

    public AttackData GetEntryAttack(CombatState state)
    {
        switch (state)
        {
            case CombatState.GroundLight:
                return groundLightAttack;

            case CombatState.GroundHeavy:
                return groundHeavyAttack;

            case CombatState.AirLight:
                return airLightAttack;

            case CombatState.AirHeavy:
                return airHeavyAttack;

            case CombatState.DashAttack:
                return dashAttack;

            case CombatState.SprintAttack:
                return sprintAttack;

            case CombatState.ChargedAttack:
                return chargedAttack;

            case CombatState.Special:
                return specialAttack;
        }

        return null;
    }
}