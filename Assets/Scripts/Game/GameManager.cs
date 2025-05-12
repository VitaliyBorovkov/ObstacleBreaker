using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject loseScreen;
    [SerializeField] private GameObject winScreen;
    [SerializeField] private SceneFader sceneFader;

    public void LoseGame()
    {
        Debug.Log("GameManager: Player lose. Size too small.");

        if (loseScreen != null)
        {
            loseScreen.SetActive(true);
        }

        Time.timeScale = 0f;
    }

    public void ReloadScene()
    {
        Time.timeScale = 1f;

        if (sceneFader != null)
        {
            sceneFader.FadeToScene(SceneManager.GetActiveScene().name);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void WinGame()
    {
        Debug.Log("GameManager: Player wins.");

        if (winScreen != null)
        {
            Time.timeScale = 0f;
            winScreen.SetActive(true);
        }
    }

    public void LoadNextScene()
    {
        Time.timeScale = 1f;

        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        int nextIndex = currentIndex + 1;

        if (nextIndex < SceneManager.sceneCountInBuildSettings)
        {
            if (sceneFader != null)
                sceneFader.FadeToScene(SceneUtility.GetScenePathByBuildIndex(nextIndex));
            else
                SceneManager.LoadScene(nextIndex);
        }
        else
        {
            Debug.LogWarning("GameManager: No next scene in build settings.");
        }
    }
}