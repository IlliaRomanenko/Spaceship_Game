using UnityEngine;

public class TriggerSpawner : MonoBehaviour
{
    public ObjectPool pool;
    public BoxCollider triggerZone; // должен быть isTrigger = true
    public int minSpawn = 1;
    public int maxSpawn = 5;
    public float spawnInterval = 3f;
    public Vector3 moveDirection = Vector3.right; // направление движения (например, вправо)
    
    [Header("Prefab Scale Settings")]
    public bool uniformScale = true; // Один масштаб для всех осей
    public Vector3 prefabScaleMin = Vector3.one;
    public Vector3 prefabScaleMax = Vector3.one * 1.5f;

    private Bounds zoneBounds;

    void Start()
    {
        if (triggerZone == null)
        {
            triggerZone = GetComponent<BoxCollider>();
        }

        zoneBounds = triggerZone.bounds;
        InvokeRepeating(nameof(SpawnObjects), 0f, spawnInterval);
    }

    void SpawnObjects()
    {
        int count = Random.Range(minSpawn, maxSpawn + 1);

        for (int i = 0; i < count; i++)
        {
            GameObject obj = pool.Get();

            Vector3 spawnPos = GetSpawnPositionOnStartSide();
            obj.transform.position = spawnPos;

            // ✅ Установка масштаба
            obj.transform.localScale = GetRandomScale();

            obj.GetComponent<MovingObject>().Init(moveDirection, zoneBounds, pool);
        }
    }
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
    /// Вычисляет позицию спавна на одной стороне зоны, адаптированную под её размер.
    /// </summary>
    private Vector3 GetSpawnPositionOnStartSide()
    {
        Vector3 center = zoneBounds.center;
        Vector3 size = zoneBounds.size;

        Vector3 pos = center;

        // Вычисляем сторону спавна по основной оси движения
        if (Mathf.Abs(moveDirection.x) > Mathf.Abs(moveDirection.y) && Mathf.Abs(moveDirection.x) > Mathf.Abs(moveDirection.z))
        {
            pos.x = moveDirection.x < 0 ? zoneBounds.max.x : zoneBounds.min.x;
            pos.y = Random.Range(zoneBounds.min.y, zoneBounds.max.y);
            pos.z = Random.Range(zoneBounds.min.z, zoneBounds.max.z);
        }
        else if (Mathf.Abs(moveDirection.y) > Mathf.Abs(moveDirection.z))
        {
            pos.y = moveDirection.y < 0 ? zoneBounds.max.y : zoneBounds.min.y;
            pos.x = Random.Range(zoneBounds.min.x, zoneBounds.max.x);
            pos.z = Random.Range(zoneBounds.min.z, zoneBounds.max.z);
        }
        else
        {
            pos.z = moveDirection.z < 0 ? zoneBounds.max.z : zoneBounds.min.z;
            pos.x = Random.Range(zoneBounds.min.x, zoneBounds.max.x);
            pos.y = Random.Range(zoneBounds.min.y, zoneBounds.max.y);
        }

        return pos;
    }
}
