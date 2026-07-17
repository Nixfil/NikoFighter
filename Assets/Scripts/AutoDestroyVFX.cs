using UnityEngine;

public class AutoDestroyVFX : MonoBehaviour
{
    [SerializeField]
    private float lifetime = 2f;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }
}