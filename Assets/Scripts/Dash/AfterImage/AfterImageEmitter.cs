using System.Collections;
using UnityEngine;

public class AfterImageEmitter : MonoBehaviour
{
    [Header("Source")]
    [SerializeField] private MeshFilter sourceMeshFilter;
    [SerializeField] private MeshRenderer sourceMeshRenderer;

    [Header("Pool")]
    [SerializeField] private AfterImagePool pool;

    [Header("Settings")]
    [SerializeField] private float spawnInterval = 0.04f;

    private Coroutine emitRoutine;

    public void Begin()
    {
        if (emitRoutine != null)
            StopCoroutine(emitRoutine);

        emitRoutine = StartCoroutine(EmitRoutine());
    }

    public void End()
    {
        if (emitRoutine != null)
        {
            StopCoroutine(emitRoutine);
            emitRoutine = null;
        }
    }

    private IEnumerator EmitRoutine()
    {
        while (true)
        {
            SpawnAfterImage();

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnAfterImage()
    {
        AfterImage image = pool.Get();

        if (image == null)
            return;

        CaptureCurrentMesh(image);

        image.Initialize(pool);
    }

    private void CaptureCurrentMesh(AfterImage image)
    {
        // Copy mesh
        image.MeshFilter.sharedMesh = sourceMeshFilter.sharedMesh;


        // Copy transform
        Transform source = sourceMeshFilter.transform;

        image.transform.position = source.position;
        image.transform.rotation = source.rotation;
        image.transform.localScale = source.lossyScale;
        Debug.Log($"Captured mesh: {sourceMeshFilter.sharedMesh.name}");
        Debug.Log($"Ghost mesh: {image.MeshFilter.sharedMesh.name}");
    }
}