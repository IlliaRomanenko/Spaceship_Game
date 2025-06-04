using System;
using System.Collections;
using Meta.XR.ImmersiveDebugger.UserInterface.Generic;
using TMPro;
using UnityEngine;


public class Menu: MonoBehaviour
{
    public GameObject UI;
    public GameObject Gameplay;
    public GameObject UIMenu;
    public GameObject SpawnZone;
    public MeasureDistance distance;
    
    public TextMeshProUGUI RunText;
    public GameObject Canvas;
    
    private float seconds = 3.0f;
    
    private bool isStarted = false;
    public void StartPressed()
    {
        Gameplay.SetActive(true);
        UI.SetActive(true);
        UIMenu.SetActive(false);
        Canvas.SetActive(true);
    }

    public void StartRun()
    {
        if (!SpawnZone.activeSelf)
        {
            StartCoroutine(RunCountdown());
        }

        
    }

    public void Quite()
    {
        Debug.Log("Quite");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Остановить Play Mode
#else
    Application.Quit(); // Закрыть игру в билде
#endif
    }

    private IEnumerator RunCountdown()
    {
        float countdown = seconds;

        while (countdown > 0)
        {
            RunText.text = Mathf.CeilToInt(countdown).ToString();
            yield return new WaitForSeconds(1f);
            countdown -= 1f;
        }

        RunText.text = "GO!";
        yield return new WaitForSeconds(1f);
        RunText.text = "";
        distance.isRunStarted = true;
        SpawnZone.SetActive(true);
    }
}
