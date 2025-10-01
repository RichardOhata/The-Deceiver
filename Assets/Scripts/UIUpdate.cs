using TMPro;
using UnityEngine;

public class UIUpdate : MonoBehaviour
{
    [SerializeField]
    private GameObject uiPanel;
    [SerializeField]
    private string uiPanelText;

    public void updatePanelText()
    {
        uiPanel.GetComponent<TextMeshProUGUI>().text = uiPanelText;
        uiPanel.SetActive(true);
    }
         
    public void DisablePanel()
    {
        uiPanel.SetActive(false);
    }


}
