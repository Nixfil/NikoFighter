using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class DashShockwave : MonoBehaviour
{
    [Header("Animation")]
    [SerializeField] private float duration = 0.2f;

    [SerializeField] private float startScale = 0f;
    [SerializeField] private float endScale = 8f;

    [SerializeField]
    private AnimationCurve scaleCurve =
        AnimationCurve.EaseInOut(0, 0, 1, 1);

    private MeshRenderer meshRenderer;
    private MaterialPropertyBlock propertyBlock;

    private bool playing;
    private float timer;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();

        propertyBlock = new MaterialPropertyBlock();

        gameObject.SetActive(false);
    }

    public void Play(Vector3 position, Quaternion rotation)
    {
        transform.position = position;
        transform.rotation = rotation;

        transform.localScale = Vector3.one * startScale;

        timer = 0f;
        playing = true;

        gameObject.SetActive(true);
    }

    private void Update()
    {
        if (!playing)
            return;

        timer += Time.deltaTime;

        float t = timer / duration;

        if (t >= 1f)
        {
            playing = false;
            gameObject.SetActive(false);
            return;
        }

        float curve = scaleCurve.Evaluate(t);

        float scale = Mathf.Lerp(
            startScale,
            endScale,
            curve);

        transform.localScale = Vector3.one * scale;

        meshRenderer.GetPropertyBlock(propertyBlock);

        propertyBlock.SetFloat("_Fade", t);

        meshRenderer.SetPropertyBlock(propertyBlock);
    }
}