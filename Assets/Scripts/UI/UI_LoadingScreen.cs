using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_LoadingScreen : MonoBehaviour
{
    [SerializeField] private Image loadingImage;
    [SerializeField] private TextMeshProUGUI loadingText;

    public float LoadFadeIn(float duration)
    {
        StartCoroutine(FadeInCoroutine(duration));
        return duration;
    }

    private IEnumerator FadeInCoroutine(float duration)
    {
        float startAlpha = 0;
        float timeElapsed = 0;

        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, 1, timeElapsed / duration);
            loadingImage.color = new Color(loadingImage.color.r, loadingImage.color.g, loadingImage.color.b, newAlpha);
            loadingText.color = new Color(loadingText.color.r, loadingText.color.g, loadingText.color.b, newAlpha);
            yield return null;
        }
    }

    public float LoadFadeOut(float duration)
    {
        gameObject.SetActive(true);
        StartCoroutine(FadeOutCoroutine(duration));
        return duration;
    }

    public IEnumerator FadeOutCoroutine(float duration)
    {
        float startAlpha = 1;
        float timeElapsed = 0;

        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, 0, timeElapsed / duration);
            loadingImage.color = new Color(loadingImage.color.r, loadingImage.color.g, loadingImage.color.b, newAlpha);
            loadingText.color = new Color(loadingText.color.r, loadingText.color.g, loadingText.color.b, newAlpha);
            yield return null;
        }
    }
}
