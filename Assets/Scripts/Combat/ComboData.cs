using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Combo", menuName = "Combat/Combo Data")]
public class ComboData : ScriptableObject
{
    [Header("General")]
    public string comboName;

    [Header("Attacks")]
    public AttackData[] attacks;
}

[Serializable]
public class CombatCombo
{
    public CombatState combatState;

    public ComboData combo;
}