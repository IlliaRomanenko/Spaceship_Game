using UnityEngine;
using System.Collections;
public class TriggerSpawner : MonoBehaviour
{
    public ObjectPool pool; // Object pool used to reuse prefabs
    public BoxCollider triggerZone; // Should have 'Is Trigger' checked
    public int minSpawn = 1; // Minimum number of objects to spawn each interval
    public int maxSpawn = 5; // Maximum number of objects to spawn each interval
    public float spawnInterval = 3f; // Time in seconds between spawns
    public Vector3 moveDirection = Vector3.right; // Direction in which spawned objects will move

    [Header("Prefab Scale Settings")]
    public bool uniformScale = true; // Use a single scale value for all axes
    public Vector3 prefabScaleMin = Vector3.one; // Minimum possible scale
    public Vector3 prefabScaleMax = Vector3.one * 1.5f; // Maximum possible scale

    private Bounds zoneBounds; // The bounds of the spawn area

    void Start()
    {
        // If no trigger zone is assigned, try to get it from the current GameObject
        if (triggerZone == null)
        {
            triggerZone = GetComponent<BoxCollider>();
        }

        zoneBounds = triggerZone.bounds;

        StartCoroutine(SpawnLoop());
    }

    void SpawnObjects()
    {
        int count = Random.Range(minSpawn, maxSpawn + 1); // Random number of objects to spawn

        for (int i = 0; i < count; i++)
        {
            GameObject obj = pool.Get(); // Get an object from the pool

            Vector3 spawnPos = GetSpawnPositionOnStartSide(); // Calculate spawn position
            obj.transform.position = spawnPos;

            // ✅ Apply random scale to the spawned object
            obj.transform.localScale = GetRandomScale();

            // Initialize the moving object with direction, bounds, and pool reference
            obj.GetComponent<MovingObject>().Init(moveDirection, zoneBounds, pool);
        }
    }
    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            SpawnObjects();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    // Returns a random scale, either uniform or axis-specific
    private Vector3 GetRandomScale()
    {
        if (uniformScale)
        {
            float scale = Random.Range(prefabScaleMin.x, prefabScaleMax.x);
            return new Vector3(scale, scale, scale);
        }
        else
        {
            return new Vector3(
                Random.Range(prefabScaleMin.x, prefabScaleMax.x),
                Random.Range(prefabScaleMin.y, prefabScaleMax.y),
                Random.Range(prefabScaleMin.z, prefabScaleMax.z)
            );
        }
    }

    /// <summary>
    /// Calculates a spawn position on the starting edge of the trigger zone,
    /// based on the dominant direction of movement.
    /// </summary>
    private Vector3 GetSpawnPositionOnStartSide()
    {
        Vector3 center = zoneBounds.center;
        Vector3 size = zoneBounds.size;

        Vector3 pos = center;

        // Determine which axis (X, Y, Z) has the dominant direction
        if (Mathf.Abs(moveDirection.x) > Mathf.Abs(moveDirection.y) && Mathf.Abs(moveDirection.x) > Mathf.Abs(moveDirection.z))
        {
            // Spawn on the X axis edge
            pos.x = moveDirection.x < 0 ? zoneBounds.max.x : zoneBounds.min.x;
            pos.y = Random.Range(zoneBounds.min.y, zoneBounds.max.y);
            pos.z = Random.Range(zoneBounds.min.z, zoneBounds.max.z);
        }
        else if (Mathf.Abs(moveDirection.y) > Mathf.Abs(moveDirection.z))
        {
            // Spawn on the Y axis edge
            pos.y = moveDirection.y < 0 ? zoneBounds.max.y : zoneBounds.min.y;
            pos.x = Random.Range(zoneBounds.min.x, zoneBounds.max.x);
            pos.z = Random.Range(zoneBounds.min.z, zoneBounds.max.z);
        }
        else
        {
            // Spawn on the Z axis edge
            pos.z = moveDirection.z < 0 ? zoneBounds.max.z : zoneBounds.min.z;
            pos.x = Random.Range(zoneBounds.min.x, zoneBounds.max.x);
            pos.y = Random.Range(zoneBounds.min.y, zoneBounds.max.y);
        }

        return pos;
    }
}
