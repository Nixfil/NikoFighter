using System.Collections.Generic;
using UnityEngine;

public class AfterImagePool : MonoBehaviour
{
    [SerializeField] private AfterImage prefab;
    [SerializeField] private int poolSize = 20;

    private readonly Queue<AfterImage> pool = new();

    private void Awake()
    {
        for (int i = 0; i < poolSize; i++)
        {
            AfterImage image = Instantiate(prefab, transform);

            image.gameObject.SetActive(false);

            pool.Enqueue(image);
        }
    }

    public AfterImage Get()
    {
        if (pool.Count == 0)
        {
            Debug.LogWarning("AfterImage Pool Empty!");

            return null;
        }

        return pool.Dequeue();
    }

    public void ReturnToPool(AfterImage image)
    {
        image.gameObject.SetActive(false);

        pool.Enqueue(image);
    }
}