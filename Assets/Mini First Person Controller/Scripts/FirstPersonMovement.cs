using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FirstPersonMovement : MonoBehaviour
{
    public float speed = 5;

    [Header("Running")]
    public bool canRun = true;
    public bool IsRunning { get; private set; }
    public float runSpeed = 9;
    public KeyCode runningKey = KeyCode.LeftShift;

    private Vector2 moveInput;
    private bool runInput;
    Rigidbody rigidbody;
    /// <summary> Functions to override movement speed. Will use the last added override. </summary>
    public List<System.Func<float>> speedOverrides = new List<System.Func<float>>();



    void Awake()
    {
        // Get the rigidbody on this.
        rigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        InputManager.Instance.controls.Player.Move.performed += OnMovePerformed;
        InputManager.Instance.controls.Player.Move.canceled += OnMoveCanceled;

        InputManager.Instance.controls.Player.Sprint.performed += OnSprintPerformed;
        InputManager.Instance.controls.Player.Sprint.canceled += OnSprintCanceled;
    }

    private void OnDisable()
    {
        if (InputManager.Instance != null)
        {
            InputManager.Instance.controls.Player.Move.performed -= OnMovePerformed;
            InputManager.Instance.controls.Player.Move.canceled -= OnMoveCanceled;

            InputManager.Instance.controls.Player.Sprint.performed -= OnSprintPerformed;
            InputManager.Instance.controls.Player.Sprint.canceled -= OnSprintCanceled;
        }
    }

    private void OnMovePerformed(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
    }

    private void OnMoveCanceled(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
    {
        moveInput = Vector2.zero;
    }

    private void OnSprintPerformed(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
    {
        runInput = true;
    }

    private void OnSprintCanceled(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
    {
        runInput = false;
    }


    void FixedUpdate()
    {
        // Update IsRunning from input.
        IsRunning = canRun && runInput;

        // Get targetMovingSpeed.
        float targetMovingSpeed = IsRunning ? runSpeed : speed;
        if (speedOverrides.Count > 0)
        {
            targetMovingSpeed = speedOverrides[speedOverrides.Count - 1]();
        }

        // Get targetVelocity from input.
        Vector2 targetVelocity = moveInput * targetMovingSpeed;

        // Apply movement.
        rigidbody.linearVelocity = transform.rotation * new Vector3(targetVelocity.x, rigidbody.linearVelocity.y, targetVelocity.y);
    }
}