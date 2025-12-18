using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-100)]
public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    public InputSystem_Actions controls { get; private set; }
 
    private void Awake()
    {
       
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        controls = new InputSystem_Actions();
        controls.Enable();
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }
    public void SetActionState(string actionName, bool newState)
    {
        // Search globally in the asset for the string (e.g. "Jump")
        InputAction action = controls.asset.FindAction(actionName);

        if (action != null)
        {
            if (newState) action.Enable();
            else action.Disable();

            Debug.Log($"[InputManager] Action '{actionName}' set to {newState}");
        }
        else
        {
            Debug.LogWarning($"[InputManager] Could not find action '{actionName}'");
        }
    }

}
