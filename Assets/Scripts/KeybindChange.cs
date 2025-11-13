using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class KeybindChange : MonoBehaviour
{
    [SerializeField] private InputActionReference actionToRebind;
    [SerializeField] private string actionName;
    [SerializeField] private string originalBindingPath;
    [SerializeField] private string newBinding;

    //public void RebindKey()
    //{
    //    actionToRebind.action.ApplyBindingOverride(0, "<Keyboard>/f");



    //    actionToRebind.action.Enable();

    //    Debug.Log($"Temporarily rebound {actionToRebind.action.name} to F");
    //}

    //public void RebindJumpToF()
    //{
    //    var jumpAction = InputManager.Instance.controls.Player.Jump;
    //    jumpAction.ApplyBindingOverride("<Keyboard>/f");
    //    Debug.Log("Jump rebound to F");
    //}

    //public void RebindJumpToSpace()
    //{
    //    var jumpAction = InputManager.Instance.controls.Player.Jump;
    //    jumpAction.ApplyBindingOverride("<Keyboard>/space");
    //    Debug.Log("Jump rebound to Space");
    //}

    public void RebindAction()
    {
        var controls = InputManager.Instance.controls;
        var playerMap = controls.Player.Get();

        var action = playerMap.FindAction(actionName);
        if (action == null)
        {
            Debug.LogWarning($"[KeybindChange] Could not find action '{actionName}' in Player map!");
            return;
        }

        action.ApplyBindingOverride(newBinding);
        Debug.Log($"[KeybindChange] '{actionName}' rebound to {newBinding}");
    }

    /// <summary>
    /// Reset all binding overrides for the given action.
    /// </summary>
    public void ResetBinding()
    {
        var controls = InputManager.Instance.controls;
        var playerMap = controls.Player.Get();

        var action = playerMap.FindAction(actionName);
        if (action == null)
        {
            Debug.LogWarning($"[KeybindChange] Could not find action '{actionName}' in Player map!");
            return;
        }

        action.ApplyBindingOverride(originalBindingPath);
        Debug.Log($"[KeybindChange] '{actionName}' rebound to {newBinding}");

    }
}
