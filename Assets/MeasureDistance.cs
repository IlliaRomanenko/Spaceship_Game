using UnityEngine;
using TMPro;

public class MeasureDistance : MonoBehaviour
{
    public TextMeshProUGUI measureDistanceText;
    public TextMeshProUGUI resultText; // Текст для отображения результата после забега
    public float environmentSpeed = 5f;

    public bool isRunStarted = false;
    public bool isRunFinished = false; // Убедимся, что завершение происходит один раз

    private float totalDistance = 0f;
    private float bestDistance = 0f;

    private void Start()
    {
        if (measureDistanceText == null)
            measureDistanceText = GetComponent<TextMeshProUGUI>();

        // Загрузить рекорд из PlayerPrefs
        bestDistance = PlayerPrefs.GetFloat("BestDistance", 0f);

        if (resultText != null)
            resultText.gameObject.SetActive(false); // Скрыть сообщение сначала
    }

    private void Update()
    {
        if (isRunStarted && !isRunFinished)
        {
            totalDistance += environmentSpeed * Time.deltaTime;
            measureDistanceText.text = $"Distance: {totalDistance:F2} Km";
        }

        // Пример: проверка окончания забега (можно вызвать из другого скрипта)
        if (!isRunStarted && !isRunFinished && totalDistance > 0f)
        {
            FinishRun();
        }
    }

    public void FinishRun()
    {
        isRunFinished = true;

        // Проверка на рекорд
        if (totalDistance > bestDistance)
        {
            bestDistance = totalDistance;
            PlayerPrefs.SetFloat("BestDistance", bestDistance);
            PlayerPrefs.Save();
        }

        // Показать результат
        if (resultText != null)
        {
            resultText.gameObject.SetActive(true);
            resultText.text = $"Run Complete!\nDistance: {totalDistance:F2} Km\nRecord: {bestDistance:F2} Km";
        }
    }

    // Можно вызвать при старте новой попытки
    public void ResetRun()
    {
        totalDistance = 0f;
        isRunStarted = true;
        isRunFinished = false;

        if (resultText != null)
            resultText.gameObject.SetActive(false);
    }
}
