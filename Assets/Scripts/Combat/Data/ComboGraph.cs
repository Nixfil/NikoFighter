using UnityEngine;

public static class ComboGraph
{
    public static AttackData GetNextAttack(
        AttackData currentAttack,
        CombatInput input)
    {
        if (currentAttack == null)
            return null;

        foreach (AttackTransition transition in currentAttack.comboTransitions)
        {
            if (transition.requiredInput == input)
                return transition.nextAttack;
        }

        return null;
    }
}