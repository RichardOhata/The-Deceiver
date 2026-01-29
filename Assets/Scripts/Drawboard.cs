using UnityEngine;

public class Drawboard : MonoBehaviour
{

    [SerializeField]
    private GameObject drawingCam;

    [SerializeField]
    private Camera playerCam;

    [SerializeField]
    private GameObject paintBoard;

    [SerializeField]
    private UIUpdate UIprompt;

    [SerializeField]
    private Draw drawManager;

    [SerializeField]
    private bool showUIprompt = true;

    public bool mazeUIOpen = false;

    private static Drawboard currentlyActiveBoard = null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerCam = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Camera>();
        UIprompt.updatePanelText();
        UIprompt.DisablePanel();
    }

    // Update is called once per frame
    void Update()
    {
        if (LookAtUtility.IsLookingAt(playerCam.transform, paintBoard.transform, 20.0f, 0f, 2f) && showUIprompt)
        {
            currentlyActiveBoard = this;
            UIprompt.EnablePanel();
            if (Input.GetKeyDown(KeyCode.E) && !mazeUIOpen)
            {
                OpenMazeUI();
              
            }
        } else
        {
            if (currentlyActiveBoard == this)
            {
                UIprompt.DisablePanel();
                currentlyActiveBoard = null;

            }
           
        }
    }

    private void OpenMazeUI()
    {
        mazeUIOpen = true;
        drawingCam.gameObject.SetActive(true);
        playerCam.enabled = false;
        drawManager.enabled = true;
        PauseMenuLogic.Instance.HandlePause(false, false);
        UIprompt.DisablePanel();
        showUIprompt = false;
    }

    public void CloseMazeUI()
    {
        mazeUIOpen = false;
        drawingCam.gameObject.SetActive(false);
        playerCam.enabled = true;
        drawManager.enabled = false;

        UIprompt.DisablePanel();
        showUIprompt = true;
      
    }
}
