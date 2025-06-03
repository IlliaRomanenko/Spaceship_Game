using UnityEngine;

public class ResourcePickup : MonoBehaviour
{
    public float refillAmount = 25f; // Сколько восстанавливает
    private void OnTriggerEnter(Collider other)
    {
        Hunger meter = other.GetComponent<Hunger>();

        if (meter != null)
        {
            meter.Refill(refillAmount);
            Destroy(gameObject); // Убираем предмет после использования
        }
    }
}