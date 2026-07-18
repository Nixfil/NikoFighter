using UnityEngine;

public static class AnchorResolver
{
    public static Pose Resolve(
        Transform player,
        AttackAnchor anchor)
    {
        Quaternion rotation = GetRotation(player, anchor);

        Vector3 position =
            player.position +
            rotation * anchor.localPosition;

        // Snap to the ground if requested
        if (anchor.snapToGround)
        {
            if (Physics.Raycast(
                    position + Vector3.up * 5f,
                    Vector3.down,
                    out RaycastHit hit,
                    20f))
            {
                position.y = hit.point.y;
            }
        }

        rotation *= Quaternion.Euler(anchor.localRotation);

        return new Pose(position, rotation);
    }

    private static Quaternion GetRotation(
        Transform player,
        AttackAnchor anchor)
    {
        // Ignore player rotation completely
        if (!anchor.followPlayerRotation)
            return Quaternion.identity;

        Quaternion rotation = player.rotation;

        // Remove pitch & roll if requested
        if (anchor.ignorePlayerPitch)
        {
            Vector3 euler = rotation.eulerAngles;
            rotation = Quaternion.Euler(0f, euler.y, 0f);
        }

        return rotation;
    }
}