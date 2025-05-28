using UnityEngine;
using System.Collections;

public class TriggerSpawner : MonoBehaviour
{
    [Header("Spawning")]
    public ObjectPool pool; // Reference to the object pool
    public BoxCollider triggerZone; // Zone where objects will spawn (must be set as 'Is Trigger')
    public int minSpawn = 1; // Minimum number of objects per spawn
    public int maxSpawn = 5; // Maximum number of objects per spawn
    public float spawnInterval = 3f; // Time between spawn waves
    public Vector3 moveDirection = Vector3.right; // Direction the objects will move

    [Header("Prefab Scale Settings")]
    public bool uniformScale = true; // Use the same scale on all axes
    public Vector3 prefabScaleMin = Vector3.one; // Minimum scale for spawned objects
    public Vector3 prefabScaleMax = Vector3.one ; // Maximum scale for spawned objects

    private Bounds zoneBounds; // Calculated bounds of the trigger zone
    private Coroutine spawnRoutine; // Reference to the spawn coroutine

    void Start()
    {
        // Auto-assign the trigger zone if not manually set
        if (triggerZone == null)
            triggerZone = GetComponent<BoxCollider>();

        zoneBounds = triggerZone.bounds;

        // Start the spawning loop
        spawnRoutine = StartCoroutine(SpawnLoop());
    }

    /// <summary>
    /// Main coroutine that handles timed object spawning
    /// </summary>
    private IEnumerator SpawnLoop()
    {
        // Infinite loop, can be stopped externally via StopCoroutine
        while (true)
        {
            SpawnObjects();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    /// <summary>
    /// Spawns a random number of objects from the pool and initializes them
    /// </summary>
    void SpawnObjects()
    {
        int count = Random.Range(minSpawn, maxSpawn + 1);

        for (int i = 0; i < count; i++)
        {
            GameObject obj = pool.Get();
            obj.transform.position = GetSpawnPositionOnStartSide();
            obj.transform.localScale = GetRandomScale();

            MovingObject mover = obj.GetComponent<MovingObject>();
            if (mover != null)
                mover.Init(moveDirection, zoneBounds, pool);
        }
    }

    /// <summary>
    /// Returns a scale vector for the spawned object, either uniform or random per axis
    /// </summary>
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
    /// Calculates the starting position on the edge of the spawn zone,
    /// based on the dominant movement axis
    /// </summary>
    private Vector3 GetSpawnPositionOnStartSide()
    {
        Vector3 pos = zoneBounds.center;

        if (Mathf.Abs(moveDirection.x) > Mathf.Abs(moveDirection.y) && Mathf.Abs(moveDirection.x) > Mathf.Abs(moveDirection.z))
        {
            // X-axis is dominant
            pos.x = moveDirection.x < 0 ? zoneBounds.max.x : zoneBounds.min.x;
            pos.y = Random.Range(zoneBounds.min.y, zoneBounds.max.y);
            pos.z = Random.Range(zoneBounds.min.z, zoneBounds.max.z);
        }
        else if (Mathf.Abs(moveDirection.y) > Mathf.Abs(moveDirection.z))
        {
            // Y-axis is dominant
            pos.y = moveDirection.y < 0 ? zoneBounds.max.y : zoneBounds.min.y;
            pos.x = Random.Range(zoneBounds.min.x, zoneBounds.max.x);
            pos.z = Random.Range(zoneBounds.min.z, zoneBounds.max.z);
        }
        else
        {
            // Z-axis is dominant
            pos.z = moveDirection.z < 0 ? zoneBounds.max.z : zoneBounds.min.z;
            pos.x = Random.Range(zoneBounds.min.x, zoneBounds.max.x);
            pos.y = Random.Range(zoneBounds.min.y, zoneBounds.max.y);
        }

        return pos;
    }

    /// <summary>
    /// Public method to stop the spawner (e.g. on game over or pause)
    /// </summary>
    public void StopSpawning()
    {
        if (spawnRoutine != null)
        {
            StopCoroutine(spawnRoutine);
            spawnRoutine = null;
        }
    }
}