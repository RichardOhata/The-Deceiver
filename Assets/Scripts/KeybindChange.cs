using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class KeybindChange : MonoBehaviour
{
    [System.Serializable]
    public struct BindingOverride
    {
        public string actionName;    // e.g. "Move"
        public int bindingIndex;     // e.g. 1 (Up), 2 (Down), 3 (Left), 4 (Right)...
        public string newBinding;    // e.g. "<Keyboard>/f"
    }

    [Header("Configuration")]
    // This creates a collapsible list in the Inspector
    [SerializeField] private List<BindingOverride> bindingsToChange;

    public void RebindAll()
    {
        var controls = InputManager.Instance.controls;
        var playerMap = controls.Player.Get();

        foreach (var bind in bindingsToChange)
        {
            InputAction action = playerMap.FindAction(bind.actionName);

            if (action == null)
            {
                Debug.LogWarning($"[KeybindChange] Action '{bind.actionName}' not found!");
                continue;
            }

            // Apply the override
            action.ApplyBindingOverride(bind.bindingIndex, bind.newBinding);
            Debug.Log($"Rebound {bind.actionName} [{bind.bindingIndex}] to {bind.newBinding}");
        }
    }

    public void ResetAll()
    {
        var controls = InputManager.Instance.controls;
        var playerMap = controls.Player.Get();

        foreach (var bind in bindingsToChange)
        {
            InputAction action = playerMap.FindAction(bind.actionName);
            if (action != null)
            {
                action.RemoveBindingOverride(bind.bindingIndex);
            }
        }
        Debug.Log("[KeybindChange] All bindings reset to default.");
    }

    // Safety check: Reset when this object is turned off/destroyed
    private void OnDisable()
    {
        ResetAll();
    }
}
