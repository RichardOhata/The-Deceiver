using TMPro;
using UnityEngine;

public class TimeButtonButtonLogic : MonoBehaviour
{
    [SerializeField] private int index;
    [SerializeField] private int numberToMatch;
    [SerializeField] private TextMeshPro buttonText;
    private Camera playerCamera;

    private TimeButtonButtonLogic currentInstance;

    private void Start()
    {
        buttonText.text = index + " | " + numberToMatch;
        playerCamera = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Camera>();
        
    }

    private void Update()
    {
        if (LookAtUtility.IsPointedAt(playerCamera, this.gameObject, 0f, 5f)) {
            currentInstance = this;
            GetComponent<UIUpdate>().EnablePanel();
            GetComponent<UIUpdate>().updatePanelText();
            if (Input.GetKeyDown(KeyCode.E))
            {
                GameObject.FindGameObjectWithTag("Time Button Puzzle").GetComponent<TimeButtonPuzzle>().CheckButtonPress(this.gameObject, index, numberToMatch);
            }
       
        } else if (currentInstance == this)
        {
            GetComponent<UIUpdate>().DisablePanel();
            currentInstance = null;
        }
     
    }
    public int GetIndex()
    {
        return index;
    }

    public int GetNumberToMatch()
    {
        return numberToMatch;
    }

}
