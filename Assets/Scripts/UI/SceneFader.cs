using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneFader : MonoBehaviour
{
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration = 0.5f;

    private void Start()
    {
        StartCoroutine(Fade(0f));
    }

    public void FadeToScene(string sceneName)
    {
        StartCoroutine(FadeAndLoad(sceneName));
    }

    private IEnumerator FadeAndLoad(string sceneName)
    {
        yield return StartCoroutine(Fade(1f));
        SceneManager.LoadScene(sceneName);
    }

    private IEnumerator Fade(float targetAlpha)
    {
        Debug.Log($"SceneFader: Starting fade to alpha {targetAlpha}");

        Color color = fadeImage.color;
        float startAlpha = color.a;
        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.unscaledDeltaTime;
            float t = timer / fadeDuration;
            color.a = Mathf.Lerp(startAlpha, targetAlpha, t);
            fadeImage.color = color;
            yield return null;
        }

        color.a = targetAlpha;
        fadeImage.color = color;
    }
}
