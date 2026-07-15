using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class AfterImage : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float lifetime = 0.25f;

    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;

    private AfterImagePool pool;

    private float timer;

    private MaterialPropertyBlock propertyBlock;

    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();

        propertyBlock = new MaterialPropertyBlock();
    }

    public MeshFilter MeshFilter => meshFilter;
    public MeshRenderer MeshRenderer => meshRenderer;

    public void Initialize(AfterImagePool ownerPool)
    {
        pool = ownerPool;

        timer = 0f;

        gameObject.SetActive(true);

        Vector2 noiseOffset = new Vector2(
            Random.Range(0f, 100f),
            Random.Range(0f, 100f));

        propertyBlock.SetVector("_NoiseOffset", noiseOffset);
    }

    private void Update()
    {
        timer += Time.deltaTime;

        float fade = timer / lifetime;

        meshRenderer.GetPropertyBlock(propertyBlock);

        propertyBlock.SetFloat("_Fade", fade);

        meshRenderer.SetPropertyBlock(propertyBlock);

        if (fade >= 1f)
        {
            pool.ReturnToPool(this);
        }
    }
}