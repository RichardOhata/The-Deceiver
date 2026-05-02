using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;
using EasyTextEffects;

public class DisclaimerManager : MonoBehaviour
{
    [SerializeField] private GameObject disclaimerHeaderText;
    [SerializeField] private GameObject disclaimerText;
    [SerializeField] private GameObject pressAnyKeyText;

    [Header("Timing Settings")]
    [SerializeField] private float initialDelay = 1.0f;
    [SerializeField] private float timeBetweenLines = 2.0f;


    [Header("Scene Transition")]
    [SerializeField] private string titleSceneName = "Title Screen";

    private Coroutine sequenceCoroutine;
    private bool isSequenceComplete = false;

    void Start()
    {
        disclaimerHeaderText.GetComponent<CanvasGroup>().alpha = 0;
        disclaimerText.GetComponent<CanvasGroup>().alpha = 0;
        pressAnyKeyText.GetComponent<CanvasGroup>().alpha = 0;

        disclaimerHeaderText.SetActive(false);
        disclaimerText.SetActive(false);
        pressAnyKeyText.SetActive(false);
        sequenceCoroutine = StartCoroutine(ShowDisclaimerSequence());
    }

    void Update()
    {
      
        if (!isSequenceComplete && Input.GetMouseButtonDown(0))
        {
            SkipSequence();
            pressAnyKeyText.GetComponent<TextEffect>().StartManualTagEffects();
        }
    
        else if (isSequenceComplete && Input.anyKeyDown)
        {
            SceneManager.LoadScene(titleSceneName);
        }
    }


    private IEnumerator ShowDisclaimerSequence()
    {
        yield return new WaitForSeconds(initialDelay);

        disclaimerHeaderText.SetActive(true);
        yield return null; 
        disclaimerHeaderText.GetComponent<CanvasGroup>().alpha = 1;

        yield return new WaitForSeconds(timeBetweenLines);

        disclaimerText.SetActive(true);
        yield return null;
        disclaimerText.GetComponent<CanvasGroup>().alpha = 1;

        yield return new WaitForSeconds(timeBetweenLines * 1.5f);

        pressAnyKeyText.SetActive(true);
        yield return null;
        pressAnyKeyText.GetComponent<CanvasGroup>().alpha = 1;

        yield return null;

        isSequenceComplete = true;
        pressAnyKeyText.GetComponent<TextEffect>().StartManualEffects();
    }

    private void SkipSequence()
    {
        if (sequenceCoroutine != null)
        {
            StopCoroutine(sequenceCoroutine);
        }
        ForceTextVisible(disclaimerHeaderText);
        ForceTextVisible(disclaimerText);
        ForceTextVisible(pressAnyKeyText);
    
        disclaimerHeaderText.GetComponent<TextEffect>().StopAllEffects();
        disclaimerText.GetComponent<TextEffect>().StopAllEffects();
        TextEffect pressAnyKeyFx = pressAnyKeyText.GetComponent<TextEffect>();
        pressAnyKeyFx.StopAllEffects();
        //pressAnyKeyFx.Refresh();
        isSequenceComplete = true;

        pressAnyKeyText.GetComponent<TextEffect>().StartManualEffects();
    }

    private void ForceTextVisible(GameObject textObject)
    {
        textObject.SetActive(true);

        CanvasGroup cg = textObject.GetComponent<CanvasGroup>();
        if (cg != null) cg.alpha = 1;

      
        TextMeshProUGUI tmp = textObject.GetComponent<TextMeshProUGUI>();
        if (tmp != null)
        {
            tmp.color = new Color(tmp.color.r, tmp.color.g, tmp.color.b, 1f);
        }
    }

}
