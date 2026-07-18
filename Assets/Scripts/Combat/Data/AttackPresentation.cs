using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Combat/Attack Presentation")]
public class AttackPresentation : ScriptableObject
{
    public AttackEffect[] attackEffects;

    public AttackEffect[] hitEffects;
}

[Serializable]
public class AttackEffect
{
    public EffectSpawnLocation spawnLocation = EffectSpawnLocation.Anchor;

    public AnchorReference anchor;

    public EffectFollowMode followMode = EffectFollowMode.SpawnOnly;

    public GameObject prefab;
}
public enum EffectFollowMode
{
    SpawnOnly,
    FollowPlayer
}
public enum EffectSpawnLocation
{
    Anchor,
    HitPoint
}