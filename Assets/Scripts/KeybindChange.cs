using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class KeybindChange : MonoBehaviour
{
    [SerializeField] private InputActionReference actionToRebind;
    private string originalBindingPath;

    public void RebindKey()
    {
        actionToRebind.action.ApplyBindingOverride(0, "<Keyboard>/f");
   

      
        actionToRebind.action.Enable();

        Debug.Log($"Temporarily rebound {actionToRebind.action.name} to F");
    }

    public void RebindJumpToF()
    {
        var jumpAction = InputManager.Instance.controls.Player.Jump;
        jumpAction.ApplyBindingOverride("<Keyboard>/f");
        Debug.Log("Jump rebound to F");
    }

    public void RebindJumpToSpace()
    {
        var jumpAction = InputManager.Instance.controls.Player.Jump;
        jumpAction.ApplyBindingOverride("<Keyboard>/space");
        Debug.Log("Jump rebound to Space");
    }


}
