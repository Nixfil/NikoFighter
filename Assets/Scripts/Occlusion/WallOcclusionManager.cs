using System.Collections.Generic;
using UnityEngine;

public class WallOcclusionManager : MonoBehaviour
{
    public static readonly List<OcclusionTarget> Targets = new();

    private Camera cam;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }
    public static void Register(OcclusionTarget target)
    {
        if (!Targets.Contains(target))
            Targets.Add(target);
    }

    public static void Unregister(OcclusionTarget target)
    {
        Targets.Remove(target);
    }

    [Header("Detection")]
    [SerializeField] private LayerMask wallMask;
    [SerializeField] private float sphereRadius = 0.35f;

    private readonly HashSet<WallOccludable> currentWalls = new();

    void LateUpdate()
    {
        if (!cam.isActiveAndEnabled)
            return;
        // Restore walls from previous frame
        foreach (var wall in currentWalls)
        {
            if (wall != null)
                wall.SetTargetOcclusion(0f);
        }

        currentWalls.Clear();

        Vector3 cameraPos = transform.position;

        foreach (var target in Targets)
        {
            if (target == null)
                continue;

            Vector3 targetPos = target.GetTargetPosition();

            Vector3 direction = targetPos - cameraPos;

            float distance = direction.magnitude;

            if (distance < 0.01f)
                continue;

            RaycastHit[] hits = Physics.SphereCastAll(
                cameraPos,
                sphereRadius,
                direction.normalized,
                distance,
                wallMask,
                QueryTriggerInteraction.Ignore);

            foreach (RaycastHit hit in hits)
            {
                WallOccludable wall =
                    hit.collider.GetComponentInParent<WallOccludable>();

                if (wall == null)
                    continue;

                wall.SetTargetOcclusion(1f);

                currentWalls.Add(wall);
            }
            Debug.DrawLine(cameraPos, targetPos, Color.green);
        }
    }
}