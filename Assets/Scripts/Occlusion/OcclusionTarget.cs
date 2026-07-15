using UnityEngine;

public class OcclusionTarget : MonoBehaviour
{
    private void OnEnable()
    {
        WallOcclusionManager.Register(this);
    }

    private void OnDisable()
    {
        WallOcclusionManager.Unregister(this);
    }

    public Vector3 GetTargetPosition()
    {
        Collider col = GetComponent<Collider>();

        if (col != null)
            return col.bounds.center;

        return transform.position;
    }
}