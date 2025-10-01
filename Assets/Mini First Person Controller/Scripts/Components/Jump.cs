// 2025-09-10 AI-Tag
// This was created with the help of Assistant, a Unity Artificial Intelligence product.

using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Jump : MonoBehaviour
{
    private Rigidbody rigidbody;
    public float jumpStrength = 2;
    public event System.Action Jumped;

    [SerializeField, Tooltip("Prevents jumping when the transform is in mid-air.")]
    private GroundCheck groundCheck;

    //[Header("Input Action")]
    //public InputAction jumpAction; // Assign this in the Inspector or via code.

    void Awake()
    {   
        rigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        InputManager.Instance.controls.Player.Jump.performed += OnJumpPerformed;
    }

    private void OnDisable()
    {
        if (InputManager.Instance != null)
        {
          
            InputManager.Instance.controls.Player.Jump.performed -= OnJumpPerformed;
        }
    }

    private void OnJumpPerformed(InputAction.CallbackContext context)
    {
        if (!groundCheck || groundCheck.isGrounded)
        {
            rigidbody.AddForce(Vector3.up * 100 * jumpStrength);
            Jumped?.Invoke();
        }
    }


}