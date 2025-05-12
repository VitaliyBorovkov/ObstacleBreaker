using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private string sceneToLoad = "GameScene";

    public SceneFader fader;

    public void StartGame()
    {
        fader.FadeToScene(sceneToLoad);
        SceneManager.LoadScene(sceneToLoad);
    }

    public void ExitGame()
    {
        Debug.Log("Exiting game...");
        Application.Quit();
    }
}
