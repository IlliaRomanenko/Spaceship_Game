using UnityEngine;

public class ObstacleDamage : MonoBehaviour
{
    public int damageAmount = 10;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        Health targetHealth = other.gameObject.GetComponent<Health>();

        if (targetHealth != null)
        {
            Debug.Log("TakeDamage");
            targetHealth.TakeDamage(damageAmount);
        }
    }
}