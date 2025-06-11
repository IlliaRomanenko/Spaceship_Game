using UnityEngine;

public class MovingObject : MonoBehaviour
{
    public float minSpeed = 2f;
    public float maxSpeed = 15f;          // Movement speed
    public int damageAmount = 10;       // Damage dealt on contact

    private Vector3 direction;          // Movement direction
    private ObjectPool pool;            // Pool to return to
    private Bounds triggerBounds;       // Bounds to stay within
    private Vector3 rotationAxis;       // Rotation axis
    private float rotationSpeed;
    private float speed;       // Rotation speed

    /// <summary>
    /// Called when object is spawned from pool
    /// </summary>
    public void Init(Vector3 moveDirection, Bounds zoneBounds, ObjectPool objectPool)
    {
        direction = moveDirection.normalized;
        triggerBounds = zoneBounds;
        pool = objectPool;

        // Setup random rotation
        rotationAxis = Random.onUnitSphere;
        rotationSpeed = Random.Range(30f, 90f);

        enabled = true; // Enable movement and collision
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!enabled) return;

        if (!other.CompareTag("Player")) return; // ✅ Проверка тега

        Health targetHealth = other.GetComponent<Health>();
        if (targetHealth != null)
        {
            targetHealth.TakeDamage(damageAmount);
            ReturnToPool();
        }
    }


    private void Update()
    {
        // Move and rotate the object
        transform.position += direction * Random.Range(minSpeed,maxSpeed) * Time.deltaTime;
        transform.Rotate(rotationAxis, rotationSpeed * Time.deltaTime, Space.Self);

        if (!triggerBounds.Contains(transform.position))
        {
            ReturnToPool();
        }
    }

    /// <summary>
    /// Returns this object to the pool and disables updates
    /// </summary>
    private void ReturnToPool()
    {
        enabled = false;
        pool.Return(gameObject);
    }
}