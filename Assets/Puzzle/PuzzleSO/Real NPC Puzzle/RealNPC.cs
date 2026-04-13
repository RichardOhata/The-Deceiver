using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using System.Collections;

public class RealNPC : Puzzle
{
    [SerializeField]
    private GameObject NPCs;

    private GameObject realNPC;

    [SerializeField]
    private UIUpdate uiPrompt;

    [SerializeField]
    private Transform playerCamera;

    private GameObject[] npcArray;

    [SerializeField]
    private LayerMask npcLayer;
    private bool isPromptCurrentlyShowing = false;
    [SerializeField]
    private float interactDistance = 4f;

    [SerializeField]
    private WarpLogic warpLogic;

    [SerializeField]
    private ScreenFader screenFader;

    [SerializeField] private AreaManager areaManager;


    [SerializeField] private EndingSequence endingSequence;
    public override void StartPuzzle()
    {
        base.StartPuzzle();
    }
    public override void SolvePuzzle()
    {
        base.SolvePuzzle();
        areaManager.GetComponent<AreaManager>().OnPuzzleSolved();
        enabled = false;
        endingSequence.TriggerEnding();
    }

    private void OnEnable()
    {
        if (isSolved) return;
        InputManager.Instance.controls.Player.Interact.performed += OnInteractPerformed;
    }

    private void OnDisable()
    {
        if (InputManager.Instance != null)
        {
            InputManager.Instance.controls.Player.Interact.performed -= OnInteractPerformed;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        uiPrompt.updatePanelText();
        uiPrompt.DisablePanel();
        realNPC = null;
        if (NPCs != null && NPCs.transform.childCount > 0)
        {
            npcArray = new GameObject[NPCs.transform.childCount];
            for (int i = 0; i < NPCs.transform.childCount; i++)
            {
                npcArray[i] = NPCs.transform.GetChild(i).gameObject;
            }
        }

        SelectRealNPC();
    
}

    // Update is called once per frame
    void Update()
    {
        if (playerCamera != null && uiPrompt != null && npcArray != null)
        {
           
            bool isLookingAtSomeone = LookAtUtility.IsLookingAtAny(playerCamera, npcArray, 30f, 0f, 4f);

            if (isLookingAtSomeone && !isPromptCurrentlyShowing)
            {
                uiPrompt.EnablePanel();
                isPromptCurrentlyShowing = true;
            }
            else if (!isLookingAtSomeone && isPromptCurrentlyShowing)
            {
                uiPrompt.DisablePanel();
                isPromptCurrentlyShowing = false;
            }
        }
    }

    public override void UpdatePuzzleStatus()
    {
        SaveManager.Instance.currentData.puzzleProgress.npcPuzzle.isSolved = true;
        SaveManager.Instance.SaveGame();
    }

    private void SelectRealNPC()
    {

        if (realNPC != null)
        {
            Renderer[] oldRenderers = realNPC.GetComponentsInChildren<Renderer>();
            foreach (Renderer rend in oldRenderers)
            {
                rend.shadowCastingMode = ShadowCastingMode.On;
            }
            Debug.Log($"Reverted shadows back to ON for the previous NPC: {realNPC.name}");
        }

        if (NPCs == null || NPCs.transform.childCount == 0)
        {
            Debug.LogError("NPCs parent is missing or has no children!");
            return;

        }
       

        int randomIndex = Random.Range(0, NPCs.transform.childCount);

        
        realNPC = NPCs.transform.GetChild(randomIndex).gameObject;


        Renderer[] npcRenderers = realNPC.GetComponentsInChildren<Renderer>();

        if (npcRenderers.Length > 0)
        {
          
            foreach (Renderer rend in npcRenderers)
            {
                rend.shadowCastingMode = ShadowCastingMode.Off;
            }

            Debug.Log($"The real NPC is {realNPC.name}! Shadows disabled for all {npcRenderers.Length} mesh parts.");
        }
        else
        {
            Debug.LogWarning($"The selected NPC ({realNPC.name}) does not have any Renderers!");
        }
    }


    private void OnInteractPerformed(InputAction.CallbackContext context)
    {
        // If the camera isn't assigned, don't try to shoot a laser
        if (playerCamera == null) return;

        // Shoot a ray from the center of the camera forward
        Ray ray = new Ray(playerCamera.position, playerCamera.forward);

        // Check if the ray hits anything on the NPC layer within our interactDistance
        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance, npcLayer))
        {
            // We hit something! Check if it has the NPCWander script attached
            NPCWander npc = hit.collider.GetComponentInParent<NPCWander>();

            if (npc != null)
            {
                // Freeze whoever they clicked on
                npc.FreezeAndInteract();

                // --- THE GUESSING LOGIC ---
                // Did they click on the real one?
                if (npc.gameObject == realNPC)
                {
                    Debug.Log("Correct guess!");
                    SolvePuzzle();
                }
                else
                {
                    StartCoroutine(PenaltyTeleportSequence(npc));
                }
            }
        }
    }

    private System.Collections.IEnumerator PenaltyTeleportSequence(NPCWander wrongNPC)
    {
        if (screenFader != null) screenFader.FadeToBlack();

        yield return new WaitForSeconds(0.8f);

       
        warpLogic.Warp();


        if (playerCamera != null)
        {
           
            playerCamera.root.rotation = Quaternion.identity;

        
        }


        wrongNPC.Unfreeze();
        SelectRealNPC();

   
        yield return new WaitForSeconds(0.8f);


        if (screenFader != null) screenFader.FadeToClear();
    }
}
