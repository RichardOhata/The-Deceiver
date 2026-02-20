using TMPro;
using UnityEngine;

public class UIUpdate : MonoBehaviour
{
    [SerializeField]
    private GameObject uiPanel;
    [SerializeField]
    private string uiPanelText;

    // Sets the ui panel text with in class value
    public void updatePanelText()
    {
        uiPanel.GetComponent<TextMeshProUGUI>().text = uiPanelText;
        uiPanel.SetActive(true);
    }

    // Sets the ui panel text with specified value
    public void updatePanelText(string panelText)
    {
        uiPanel.GetComponent<TextMeshProUGUI>().text = panelText;
        uiPanel.SetActive(true);
    }

    public void EnablePanel()
    {
        uiPanel.SetActive(true);
    }

    public void DisablePanel()
    {
        uiPanel.SetActive(false);
    }

    // Sets the text value
    public void SetText(string newText)
    {
        uiPanelText = newText;
    }


}
