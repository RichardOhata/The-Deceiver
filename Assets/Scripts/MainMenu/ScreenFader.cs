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

    private IEnumerator FadeRoutine(float targetAlpha, float customDuration)
    {
        fadeGroup.blocksRaycasts = true;
        float startAlpha = fadeGroup.alpha;
        float time = 0;
        bool hasUnlocked = false;

     
        while (time < customDuration)
        {
            time += Time.deltaTime;

            
            if (!hasUnlocked && time >= customDuration / 2)
            {
                fadeGroup.blocksRaycasts = false;
                hasUnlocked = true;
            }

            fadeGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, time / customDuration);
            yield return null;
        }

        fadeGroup.alpha = targetAlpha;
        if (targetAlpha == 0f) fadeGroup.blocksRaycasts = false;
    }

 
    public void FadeToBlack()
    {
        StartCoroutine(FadeRoutine(1f, 0.5f));
    }

    public void FadeToClear()
    {
        StartCoroutine(FadeRoutine(0f, fadeDuration)); 
    }
}