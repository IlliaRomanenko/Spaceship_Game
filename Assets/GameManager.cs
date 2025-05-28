using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    // Singleton instance to make GameManager globally accessible
    public static GameManager Instance;

    private void Awake()
    {
        // Ensure only one instance of GameManager exists (singleton pattern)
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject); // Destroy duplicate if one already exists
    }

    // Public method to start a delayed scene restart
    public void RestartSceneAfterDelay(float delay)
    {
        StartCoroutine(Reload(delay));
    }

    // Coroutine that waits for a given time before reloading the scene
    private IEnumerator Reload(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Reload the currently active scene by its build index
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}