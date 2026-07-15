using UnityEngine;

public class DashVFX : MonoBehaviour
{
    [Header("After Images")]
    [SerializeField] private AfterImageEmitter afterImageEmitter;

    [Header("Shockwave")]
    [SerializeField] private GameObject shockwavePrefab;
    [SerializeField] private Transform vfxPool;

    [Header("Ground Shockwave")]
    [SerializeField] private float groundDuration = 0.20f;
    [SerializeField] private Vector3 groundStartScale = Vector3.zero;
    [SerializeField] private Vector3 groundEndScale = new Vector3(8f, 0.2f, 8f);
    [SerializeField]
    private AnimationCurve groundScaleCurve =
        AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("Air Shockwave")]
    [SerializeField] private float airDuration = 0.20f;
    [SerializeField] private Vector3 airStartScale = Vector3.zero;
    [SerializeField] private Vector3 airEndScale = new Vector3(8f, 8f, 0.2f);
    [SerializeField]
    private AnimationCurve airScaleCurve =
        AnimationCurve.EaseInOut(0, 0, 1, 1);

    [SerializeField] private float airSpawnOffset = 0.35f;

    [Header("Shader")]
    [SerializeField] private float startDistortion = 0.08f;
    [SerializeField] private float endDistortion = 0f;

    [Header("Debug")]
    [SerializeField] private bool enableDebugLogs = true;

    private MaterialPropertyBlock propertyBlock;

    private class ShockwaveRuntime
    {
        public Transform Transform;
        public MeshRenderer Renderer;

        public bool Playing;
        public float Timer;
    }

    private ShockwaveRuntime groundShockwave = new();
    private ShockwaveRuntime airShockwave = new();

    private void Awake()
    {
        propertyBlock = new MaterialPropertyBlock();

        CreateShockwave(ref groundShockwave, "Ground Shockwave");
        CreateShockwave(ref airShockwave, "Air Shockwave");
    }

    private void Update()
    {
        AnimateShockwave(
            groundShockwave,
            groundDuration,
            groundStartScale,
            groundEndScale,
            groundScaleCurve);

        AnimateShockwave(
            airShockwave,
            airDuration,
            airStartScale,
            airEndScale,
            airScaleCurve);
    }

    public void DashStarted(CharacterState state, Vector3 direction)
    {
        switch (state)
        {
            case CharacterState.DashingGround:

                if (enableDebugLogs)
                    Debug.Log("<color=cyan>[DashVFX]</color> Ground Dash");

                /*PlayShockwave(
                    groundShockwave,
                    transform.position - new Vector3(0f, 0.5f, 0f),
                    Quaternion.Euler(-90f, 0f, 0f),
                    groundStartScale);*/

                break;

            case CharacterState.DashingAir:

                if (enableDebugLogs)
                    Debug.Log("<color=cyan>[DashVFX]</color> Air Dash");

              /*  PlayShockwave(
                    airShockwave,
                    transform.position - direction.normalized * airSpawnOffset,
                    Quaternion.LookRotation(direction),
                    airStartScale);*/

                break;
        }

        afterImageEmitter.Begin();
    }

    public void DashEnded()
    {
        if (enableDebugLogs)
            Debug.Log("<color=cyan>[DashVFX]</color> Dash Ended");

        afterImageEmitter.End();
    }

    private void CreateShockwave(ref ShockwaveRuntime shockwave, string objectName)
    {
        GameObject instance = Instantiate(
            shockwavePrefab,
            Vector3.zero,
            Quaternion.identity,
            vfxPool);

        instance.name = objectName;
        instance.SetActive(false);

        shockwave.Transform = instance.transform;
        shockwave.Renderer = instance.GetComponent<MeshRenderer>();

        if (shockwave.Renderer == null)
        {
            Debug.LogError($"Shockwave prefab '{objectName}' is missing a MeshRenderer.");
        }
    }

    private void PlayShockwave(
        ShockwaveRuntime shockwave,
        Vector3 position,
        Quaternion rotation,
        Vector3 startScale)
    {
        shockwave.Timer = 0f;
        shockwave.Playing = true;

        shockwave.Transform.position = position;
        shockwave.Transform.rotation = rotation;
        shockwave.Transform.localScale = startScale;

        shockwave.Transform.gameObject.SetActive(true);
    }

    private void AnimateShockwave(
        ShockwaveRuntime shockwave,
        float duration,
        Vector3 startScale,
        Vector3 endScale,
        AnimationCurve curve)
    {
        if (!shockwave.Playing)
            return;

        shockwave.Timer += Time.deltaTime;

        float t = shockwave.Timer / duration;

        if (t >= 1f)
        {
            shockwave.Playing = false;
            shockwave.Transform.gameObject.SetActive(false);
            return;
        }

        shockwave.Transform.localScale = Vector3.Lerp(
            startScale,
            endScale,
            curve.Evaluate(t));

        shockwave.Renderer.GetPropertyBlock(propertyBlock);

        propertyBlock.SetFloat("_Fade", t);

        propertyBlock.SetFloat(
            "_DistortionStrength",
            Mathf.Lerp(startDistortion, endDistortion, t));

        shockwave.Renderer.SetPropertyBlock(propertyBlock);
    }
}