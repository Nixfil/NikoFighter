using UnityEngine;
using System.Collections;

public class MeshFlash : MonoBehaviour
{
    [SerializeField] private float duration = 0.08f;

    private Renderer[] renderers;

    private MaterialPropertyBlock block;

    private static readonly int FlashStrength =
        Shader.PropertyToID("_FlashStrength");

    private void Awake()
    {
        renderers = GetComponentsInChildren<Renderer>();
        block = new MaterialPropertyBlock();
    }

    public void Flash()
    {
        StopAllCoroutines();
        StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        float timer = 0f;

        while (timer < duration)
        {
            float strength = 1f - timer / duration;

            foreach (Renderer renderer in renderers)
            {
                renderer.GetPropertyBlock(block);

                block.SetFloat(FlashStrength, strength);

                renderer.SetPropertyBlock(block);
            }

            timer += Time.deltaTime;

            yield return null;
        }

        foreach (Renderer renderer in renderers)
        {
            renderer.GetPropertyBlock(block);

            block.SetFloat(FlashStrength, 0);

            renderer.SetPropertyBlock(block);
        }
    }
}