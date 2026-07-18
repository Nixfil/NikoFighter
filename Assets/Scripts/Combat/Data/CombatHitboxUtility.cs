using System;
using UnityEngine;

public static class CombatHitboxUtility
{
    public static Collider[] Scan(
        AttackHitbox hitbox,
        Pose pose)
    {
        switch (hitbox.shape)
        {
            case HitboxShape.Box:

                return Physics.OverlapBox(
                    pose.position,
                    hitbox.size * 0.5f,
                    pose.rotation,
                    hitbox.targets);

            case HitboxShape.Sphere:

                return Physics.OverlapSphere(
                    pose.position,
                    hitbox.radius,
                    hitbox.targets);

            default:
                return Array.Empty<Collider>();
        }
    }

    public static void DrawGizmo(
        AttackHitbox hitbox,
        Pose pose)
    {
        Gizmos.matrix = Matrix4x4.TRS(
            pose.position,
            pose.rotation,
            Vector3.one);

        switch (hitbox.shape)
        {
            case HitboxShape.Box:

                Gizmos.DrawWireCube(
                    Vector3.zero,
                    hitbox.size);

                break;

            case HitboxShape.Sphere:

                Gizmos.DrawWireSphere(
                    Vector3.zero,
                    hitbox.radius);

                break;
        }

        Gizmos.matrix = Matrix4x4.identity;
    }
    public static Pose ResolveHitboxPose(
    Transform owner,
    AttackData attack,
    AttackHitbox hitbox)
    {
        AttackAnchor anchor = attack.GetAnchor(hitbox.anchor);

        if (anchor == null)
            return new Pose(owner.position, owner.rotation);

        Pose anchorPose = AnchorResolver.Resolve(owner, anchor);

        Quaternion rotation =
            anchorPose.rotation *
            Quaternion.Euler(hitbox.localRotation);

        Vector3 position =
            anchorPose.position +
            anchorPose.rotation * hitbox.localPosition;

        return new Pose(position, rotation);
    }
}