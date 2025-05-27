using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Health : MonoBehaviour
{
    // Maximum health points the object starts with
    public int maxHP = 100;

    // Tracks the current health during gameplay
    private int currentHP;

    // Initialize health when the object is created
    void Start()
    {
        currentHP = maxHP;
    }

    // Public method to apply damage to this object
    public void TakeDamage(int amount)
    {
        Debug.Log("DamageTaken");

        // Reduce the current health by the damage amount
        currentHP -= amount;
        Debug.Log($"{gameObject.name} took {amount} damage. Remaining HP: {currentHP}");

        // Check if the object has run out of HP
        if (currentHP <= 0)
        {
            Die();
        }
    }

    // Handles what happens when HP reaches zero
    private void Die()
    {
        Debug.Log($"{gameObject.name} has been destroyed.");

        // Tell the GameManager to restart the scene after a short delay
        GameManager.Instance.RestartSceneAfterDelay(2f);

        // Deactivate the object to remove it from the scene (but keep it alive for coroutine to finish)
        gameObject.SetActive(false);
    }
}