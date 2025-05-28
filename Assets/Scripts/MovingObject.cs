using UnityEngine;

public class MovingObject : MonoBehaviour
{
    public float speed = 2f; // Movement speed
    public int damageAmount = 10; // Damage dealt on contact

    private Vector3 direction; // Direction to move
    private ObjectPool pool; // Reference to object pool
    private Bounds triggerBounds; // Movement boundaries
    private bool isInitialized = false; // Whether this object is active and initialized
    private Vector3 rotationAxis; // Axis to rotate around
    private float rotationSpeed; // Speed of rotation in degrees/second

    /// <summary>
    /// Initializes the object with movement direction, zone bounds, and object pool reference
    /// </summary>
    public void Init(Vector3 moveDirection, Bounds zoneBounds, ObjectPool objectPool)
    {
        direction = moveDirection.normalized; // Normalize to get consistent speed
        triggerBounds = zoneBounds; // Set area the object is allowed to move in
        pool = objectPool;
        isInitialized = true;

        // Assign random rotation behavior
        rotationAxis = Random.onUnitSphere; // Random rotation axis
        rotationSpeed = Random.Range(30f, 90f); // Rotation speed in degrees per second
    }

    private void OnTriggerEnter(Collider other)
    {
        // Try to damage any object with a Health component
        Health targetHealth = other.gameObject.GetComponent<Health>();

        if (targetHealth != null)
        {
            targetHealth.TakeDamage(damageAmount); // Apply damage
            pool.Return(gameObject); // Return this object to the pool
            isInitialized = false; // Mark as inactive
        }
    }

    void Update()
    {
        if (!isInitialized) return; // Skip update if not active

        // Move in the assigned direction
        transform.position += direction * speed * Time.deltaTime;

        // Apply rotation
        transform.Rotate(rotationAxis, rotationSpeed * Time.deltaTime, Space.Self);

        // Return to pool if out of bounds
        if (!triggerBounds.Contains(transform.position))
        {
            pool.Return(gameObject);
            isInitialized = false;
        }
    }
}
