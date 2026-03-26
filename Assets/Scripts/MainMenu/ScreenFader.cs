using System.Collections;
using UnityEngine;

public class ScreenFader : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CanvasGroup fadeGroup;

    [Header("Settings")]
    [SerializeField] private float fadeDuration = 2f;

    private void Start()
    {
        fadeGroup.alpha = 1f;
        FadeToClear();
    }

    public void FadeToBlack()
    {
        StartCoroutine(FadeRoutine(1f));
    }

    public void FadeToClear()
    {
        StartCoroutine(FadeRoutine(0f));
    }

    private IEnumerator FadeRoutine(float targetAlpha)
    {

        fadeGroup.blocksRaycasts = true;

        float startAlpha = fadeGroup.alpha;
        float time = 0;
        bool hasUnlocked = false;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;


            if (!hasUnlocked && time >= 1.0f)
            {
                fadeGroup.blocksRaycasts = false;
                hasUnlocked = true;
                Debug.Log("Buttons are now clickable!");
            }

            fadeGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, time / fadeDuration);
            yield return null;
        }

        fadeGroup.alpha = targetAlpha;
        if (targetAlpha == 0f)
        {
            fadeGroup.blocksRaycasts = false;
        }
    }
}