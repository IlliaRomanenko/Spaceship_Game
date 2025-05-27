using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Generic object pool for reusing GameObjects to reduce instantiation overhead.
/// </summary>
public class ObjectPool : MonoBehaviour
{
    [Header("Pool Settings")]
    public GameObject prefab;         // Prefab to instantiate
    public int initialSize = 10;      // Number of objects to preload
    public bool expandable = true;    // Whether the pool can grow if exhausted

    private readonly Queue<GameObject> pool = new(); // Internal queue for pooled objects

    void Awake()
    {
        // Pre-instantiate objects and add them to the pool
        for (int i = 0; i < initialSize; i++)
        {
            GameObject obj = CreateNewObject();
            pool.Enqueue(obj);
        }
    }

    /// <summary>
    /// Returns an object from the pool. Expands pool if allowed and needed.
    /// </summary>
    public GameObject Get()
    {
        if (pool.Count > 0)
        {
            GameObject obj = pool.Dequeue();
            obj.SetActive(true);
            return obj;
        }

        if (expandable)
        {
            GameObject newObj = CreateNewObject();
            newObj.SetActive(true);
            return newObj;
        }

        Debug.LogWarning("Object pool is exhausted and not expandable.");
        return null;
    }

    /// <summary>
    /// Returns the object back into the pool.
    /// </summary>
    public void Return(GameObject obj)
    {
        obj.SetActive(false);
        pool.Enqueue(obj);
    }

    /// <summary>
    /// Creates a new instance of the pooled prefab.
    /// </summary>
    private GameObject CreateNewObject()
    {
        GameObject obj = Instantiate(prefab, transform);
        obj.SetActive(false);
        return obj;
    }

    /// <summary>
    /// Clears all objects in the pool and destroys them.
    /// </summary>
    public void Clear()
    {
        while (pool.Count > 0)
        {
            GameObject obj = pool.Dequeue();
            if (obj != null)
                Destroy(obj);
        }
    }
}
