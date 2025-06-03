using UnityEngine;
using TMPro;

public class MeasureDistance : MonoBehaviour
{
    public TextMeshProUGUI measureDistanceText;
    public float environmentSpeed = 5f;
    
    public bool isRunStarted = false;

    private float totalDistance = 0f;

    private void Start()
    {
        if (measureDistanceText == null)
        {
            measureDistanceText = GetComponent<TextMeshProUGUI>();
        }
    }

    private void Update()
    {
        if (isRunStarted)
        {
            totalDistance += environmentSpeed * Time.deltaTime;
            measureDistanceText.text = $"Distance: {totalDistance:F2} Km";
        }
    }
}