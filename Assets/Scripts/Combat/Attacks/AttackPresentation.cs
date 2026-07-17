using UnityEngine;
using UnityEngine.VFX;

[CreateAssetMenu(
    fileName = "AttackPresentation",
    menuName = "Combat/Attack Presentation")]
public class AttackPresentation : ScriptableObject
{
    [Header("Attack")]
    public GameObject attackVFXPrefab;

    [Header("Hit")]
    public GameObject impactVFXPrefab;
}