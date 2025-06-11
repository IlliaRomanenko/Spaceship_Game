using UnityEngine;

public class DoughnutMove : MonoBehaviour
{
    [SerializeField] private float minSpeed = 2f;
    [SerializeField] private float maxSpeed = 15f;
    [SerializeField] private float refillAmount = 2f;

    private Vector3 direction;
    private DoughnutPool pool;
    private Bounds triggerBounds;
    private Vector3 rotationAxis;
    private float rotationSpeed;
    private float speed;

    /// <summary>
    /// Called when object is spawned from pool
    /// </summary>
    public void Init(Vector3 moveDirection, Bounds zoneBounds, DoughnutPool objectPool)
    {
        direction = moveDirection.normalized;
        triggerBounds = zoneBounds;
        pool = objectPool;

        rotationAxis = Random.onUnitSphere;
        rotationSpeed = Random.Range(30f, 90f);
        speed = Random.Range(minSpeed, maxSpeed);

        enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!enabled) return;

        if (!other.CompareTag("Player")) return;
        
        Hunger targetHunger = other.GetComponent<Hunger>();
        if (targetHunger != null)
        {
            targetHunger.Refill(refillAmount);
            ReturnToPool();
        }
    }

    private void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
        transform.Rotate(rotationAxis, rotationSpeed * Time.deltaTime, Space.Self);

        if (!triggerBounds.Contains(transform.position))
        {
            ReturnToPool();
        }
    }

    private void ReturnToPool()
    {
        enabled = false;
        pool.Return(gameObject);
    }
}