using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHP = 100;
    private int currentHP;

    void Start()
    {
        currentHP = maxHP;
    }

    public void TakeDamage(int amount)
    {
        Debug.Log("DamageTaken");
        currentHP -= amount;
        Debug.Log($"{gameObject.name} took {amount} damage. Remaining HP: {currentHP}");

        if (currentHP <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} has been destroyed.");
        Destroy(gameObject);
    }
}