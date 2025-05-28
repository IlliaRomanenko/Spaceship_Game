using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Generic object pool for reusing GameObjects to reduce instantiation overhead.
/// </summary>
public class ObjectPool : MonoBehaviour
{public GameObject prefab;
    public int initialSize = 10;
    public int expandAmount = 5; // How many objects to add when expanding
    public int maxPoolSize = 100; // Prevent infinite expansion and memory leaks

    private Queue<GameObject> pool = new Queue<GameObject>();
    private List<GameObject> allPooledObjects = new List<GameObject>(); // To keep track of all instantiated objects

    void Awake() // Changed to Awake, ensures pool is ready before other scripts try to get objects
    {
        ExpandPool(initialSize);
    }

    /// <summary>
    /// Returns an object from the pool. Expands pool if allowed and needed.
    /// </summary>
    public GameObject Get()
    {
        if (pool.Count == 0)
        {
            if (allPooledObjects.Count < maxPoolSize) // Only expand if we haven't hit the max size
            {
                Debug.LogWarning($"ObjectPool for {prefab.name} is expanding. Consider increasing initialSize or maxPoolSize. Current active objects: {allPooledObjects.Count - pool.Count}");
                ExpandPool(expandAmount);
            }
            else
            {
                Debug.LogError($"ObjectPool for {prefab.name} has reached its maximum size ({maxPoolSize}) and is empty. Cannot get more objects.");
                return null; // Or throw an exception, depending on desired behavior
            }
        }

        GameObject obj = pool.Dequeue();
        if (obj != null)
        {
            obj.SetActive(true);
            obj.transform.SetParent(null, true); // Unparent to avoid transform inheritance
        }
        return obj;
    }

    /// <summary>
    /// Returns the object back into the pool.
    /// </summary>
    public void Return(GameObject obj)
    {
        if (obj == null) return; // Prevent errors if a null object is returned

        // Reset object state before returning to pool
        obj.SetActive(false);
        obj.transform.SetParent(transform); // Keep things organized under the pool manager
        // Optionally, reset physics properties if they were altered (e.g., Rigidbody velocity)
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        pool.Enqueue(obj);
    }

    private void ExpandPool(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject obj = Instantiate(prefab, transform); // Instantiate as a child of the pool manager
            obj.SetActive(false);
            pool.Enqueue(obj);
            allPooledObjects.Add(obj); // Keep track of all created objects
        }
    }

    // Optional: A method to clear the pool (e.g., on scene change)
    public void ClearPool()
    {
        foreach (GameObject obj in allPooledObjects)
        {
            if (obj != null)
            {
                Destroy(obj);
            }
        }
        pool.Clear();
        allPooledObjects.Clear();
    }
}
