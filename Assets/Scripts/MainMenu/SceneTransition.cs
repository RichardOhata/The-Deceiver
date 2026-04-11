using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    [SerializeField] private ScreenFader screenFader;
    [SerializeField] private GameObject saveIcon;
    public void SwitchScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }


    public void StartGame()
    {
        StartCoroutine(LoadingSequence("Game"));
     
    }

    public IEnumerator LoadingSequence(string sceneName)
    {
       
        screenFader.FadeToBlack();
        yield return new WaitForSeconds(0.5f); 

      
        if (saveIcon != null) saveIcon.SetActive(true);

        
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

       
        operation.allowSceneActivation = false;

      
        while (operation.progress < 0.9f)
        {
     
            yield return null;
        }
        yield return new WaitForSeconds(2.0f);

        if (saveIcon != null) saveIcon.SetActive(false);
        operation.allowSceneActivation = true;
    }
}
