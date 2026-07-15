using UnityEngine;

public class WallOccludable : MonoBehaviour
{
    // Shader Property IDs
    private static readonly int OcclusionID = Shader.PropertyToID("_Occlusion");
    private static readonly int VisibleHeightID = Shader.PropertyToID("_VisibleHeight");
    private static readonly int DitherStrengthID = Shader.PropertyToID("_DitherStrength");

    [Header("Fade Settings")]
    [SerializeField]
    private float fadeSpeed = 5f;

    [Header("Shader Settings")]
    [SerializeField, Range(0.01f, 0.5f)]
    private float visibleHeight = 0.4f;

    [SerializeField, Range(0f, 2f)]
    private float ditherStrength = 1f;

    private Renderer[] renderers;
    private MaterialPropertyBlock propertyBlock;

    private float currentOcclusion = 0f;
    private float targetOcclusion = 0f;

    public void SetTargetOcclusion(float value)
    {
        value = Mathf.Clamp01(value);

        if (Mathf.Approximately(targetOcclusion, value))
            return;

        targetOcclusion = value;

        // Wake up only when we actually need to animate.
        enabled = true;
    }

    private void Awake()
    {
        renderers = GetComponentsInChildren<Renderer>();

        propertyBlock = new MaterialPropertyBlock();

        // Initialize all shader properties.
        propertyBlock.SetFloat(VisibleHeightID, visibleHeight);
        propertyBlock.SetFloat(DitherStrengthID, ditherStrength);
        propertyBlock.SetFloat(OcclusionID, currentOcclusion);

        foreach (Renderer rend in renderers)
        {
            rend.SetPropertyBlock(propertyBlock);
        }

        // Don't waste Updates until needed.
        enabled = false;
    }

    private void Update()
    {
        currentOcclusion = Mathf.MoveTowards(
            currentOcclusion,
            targetOcclusion,
            fadeSpeed * Time.deltaTime);

        foreach (Renderer rend in renderers)
        {
            rend.GetPropertyBlock(propertyBlock);

            propertyBlock.SetFloat(OcclusionID, currentOcclusion);

            rend.SetPropertyBlock(propertyBlock);
        }

        // Stop updating when we've reached the target.
        if (Mathf.Approximately(currentOcclusion, targetOcclusion))
        {
            enabled = false;
        }
    }
}