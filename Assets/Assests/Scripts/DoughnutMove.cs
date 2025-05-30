using UnityEngine;

public class DoughnutMove : MonoBehaviour
{
    [SerializeField] private float minSpeed = 2f;
    [SerializeField] private float maxSpeed = 15f;
    [SerializeField] private float refillAmount = 2;
    private Vector3 direction;
    private DoughnutPool pool;
    private Bounds triggerBounds;
    private float speed;
    void Start()
    {
        
    }
    public void Init(Vector3 moveDirection, Bounds zoneBounds, DoughnutPool objectPool)
    {
        speed = Random.Range(minSpeed, maxSpeed);
        direction = moveDirection.normalized;
        triggerBounds = zoneBounds;
        pool = objectPool;
        enabled = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!enabled) return;

        // Quick check by component (you can optimize further using tags or layers)
        Hunger targetHunger = other.GetComponent<Hunger>();
        if (targetHunger != null)
        {
            targetHunger.Refill(refillAmount);
            ReturnToPool();
        }
    }

    // Update is called once per fram
    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
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
