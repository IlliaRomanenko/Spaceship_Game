using UnityEngine;

public class MovingObject : MonoBehaviour
{
    public float speed = 2f;
    public int damageAmount = 10;

    private Vector3 direction;
    private ObjectPool pool;
    private Bounds triggerBounds;
    private Vector3 rotationAxis;
    private float rotationSpeed;

    private bool initialized = false; // дополнительная защита

    public void Init(Vector3 moveDirection, Bounds zoneBounds, ObjectPool objectPool)
    {
        direction = moveDirection.normalized;
        triggerBounds = zoneBounds;
        pool = objectPool;

        rotationAxis = Random.onUnitSphere;
        rotationSpeed = Random.Range(30f, 90f);

        initialized = true;
        enabled = true; // включаем Update()
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!initialized || pool == null) return;

        Health targetHealth = other.GetComponent<Health>();
        if (targetHealth != null)
        {
            targetHealth.TakeDamage(damageAmount);
            ReturnToPool();
        }
    }

    private void Update()
    {
        if (!initialized || pool == null) return;

        transform.position += direction * speed * Time.deltaTime;
        transform.Rotate(rotationAxis, rotationSpeed * Time.deltaTime, Space.Self);

        if (!triggerBounds.Contains(transform.position))
        {
            ReturnToPool();
        }
    }

    private void ReturnToPool()
    {
        initialized = false;
        enabled = false;

        if (pool != null)
            pool.Return(gameObject);
        else
            Debug.LogWarning($"{name}: Tried to return to null pool.");
    }
}