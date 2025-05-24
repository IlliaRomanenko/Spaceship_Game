using UnityEngine;

public class MovingObject : MonoBehaviour
{
    public float speed = 2f;

    private Vector3 direction;
    private ObjectPool pool;
    private Bounds triggerBounds;
    private bool isInitialized = false;

    private Vector3 rotationAxis;
    private float rotationSpeed;

    /// <summary>
    /// Инициализирует объект, задаёт направление, границы зоны и ссылку на пул
    /// </summary>
    public void Init(Vector3 moveDirection, Bounds zoneBounds, ObjectPool objectPool)
    {
        direction = moveDirection.normalized;
        triggerBounds = zoneBounds;
        pool = objectPool;
        isInitialized = true;

        // Случайное вращение
        rotationAxis = Random.onUnitSphere; // случайное направление вращения
        rotationSpeed = Random.Range(30f, 90f); // угол в градусах/сек
    }

    void Update()
    {
        if (!isInitialized) return;

        // Движение
        transform.position += direction * speed * Time.deltaTime;

        // Вращение
        transform.Rotate(rotationAxis, rotationSpeed * Time.deltaTime, Space.Self);

        // Проверка выхода за зону
        if (!triggerBounds.Contains(transform.position))
        {
            pool.Return(gameObject);
            isInitialized = false;
        }
    }
}