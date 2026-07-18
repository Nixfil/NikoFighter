using System;
using UnityEngine;

[Serializable]
public class AttackAnchor
{
    [Header("Identification")]
    public AnchorType type = AnchorType.Player;

    [Tooltip("Only used when Type = Custom")]
    public string customName;

    [Header("Transform")]
    public Vector3 localPosition;

    public Vector3 localRotation;

    [Header("Behaviour")]
    public bool followPlayerRotation = true;

    public bool ignorePlayerPitch = true;

    public bool snapToGround = false;
}

public enum AnchorType
{
    Player,
    Ground,
    WeaponTip,
    Enemy,
    Custom
}