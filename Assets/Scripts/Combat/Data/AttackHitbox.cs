using System;
using UnityEngine;


[System.Serializable]
public class AttackHitbox
{
    [Header("Anchor")]
    public AnchorReference anchor;

    [Header("Transform")]
    public Vector3 localPosition;
    public Vector3 localRotation;

    [Header("Shape")]
    public HitboxShape shape = HitboxShape.Box;

    public Vector3 size = Vector3.one;
    public float radius = 0.5f;

    [Header("Collision")]
    public LayerMask targets;
}

public enum HitboxShape
{
    Box,
    Sphere
}