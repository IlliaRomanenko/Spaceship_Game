using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Hunger : MonoBehaviour
{
    [Header("Resource Settings")]
    public float maxValue = 100f;         // Максимальное значение ресурса
    public float currentValue;            // Текущее значение
    public float decreaseRate = 5f;       // Уменьшение в секунду

    private bool isDead = false;          // Флаг, чтобы смерть происходила только один раз

    void Start()
    {
        currentValue = maxValue;
    }

    void Update()
    {
        if (isDead) return;

        // Плавное уменьшение ресурса
        currentValue -= decreaseRate * Time.deltaTime;

        // Ограничение снизу
        if (currentValue <= 0f)
        {
            currentValue = 0f;
            Die();
        }
    }

    /// <summary>
    /// Пополнение ресурса, но не выше максимума
    /// </summary>
    public void Refill(float amount)
    {
        if (isDead) return;

        currentValue += amount;

        if (currentValue > maxValue)
            currentValue = maxValue;
    }

    /// <summary>
    /// Поведение при полном истощении ресурса
    /// </summary>
    private void Die()
    {
        isDead = true;
        Debug.Log($"{gameObject.name} died due to resource depletion.");
        StartCoroutine(RestartSceneAfterDelay(2f));
    }

    private IEnumerator RestartSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}