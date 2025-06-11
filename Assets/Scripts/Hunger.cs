using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class Hunger : MonoBehaviour
{
    public MeasureDistance _measureDistance;
    public Slider hunger;
    public TextMeshProUGUI text;
    public bool isGameStarted = false;
    [Header("Resource Settings")]
    public float maxValue = 100f;         // Максимальное значение ресурса
    public float currentValue;            // Текущее значение
    public float decreaseRate = 5f;       // Уменьшение в секунду

    private bool isDead = false;          // Флаг, чтобы смерть происходила только один раз

    void Start()
    {
        hunger.minValue = 0f;
        hunger.maxValue = maxValue;
        currentValue = hunger.minValue;
        hunger.value = currentValue;
        isGameStarted = false;
    }

    void Update()
    {
        if (isDead) return;
        if (isGameStarted)
        {
             currentValue += decreaseRate * Time.deltaTime;
        hunger.value = currentValue;
        // Ограничение снизу
        if (currentValue >= maxValue)
        {
            currentValue = maxValue;
            Die();
        }
        }
        // Плавное уменьшение ресурса
       
    }

    /// <summary>
    /// Пополнение ресурса, но не выше максимума
    /// </summary>
    public void Refill(float amount)
    {
        if (isDead) return;
        Debug.Log($"Refilling");
        currentValue -= amount;
        hunger.value = currentValue;
        if (currentValue < hunger.minValue)
            currentValue = hunger.minValue;
    }

    /// <summary>
    /// Поведение при полном истощении ресурса
    /// </summary>
    private void Die()
    {
        isDead = true;
        _measureDistance.FinishRun();
        StartCoroutine(RestartSceneAfterDelay(3f));
    }

    private IEnumerator RestartSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}