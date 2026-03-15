using UnityEngine;

public class RubberBandCamera : MonoBehaviour
{
   [SerializeField] Transform character;
    public float sensitivity = 0.1f;
    public float maxLookAngle = 35f; // How far they can stretch their neck
    public float snapBackSpeed = 5f; // How fast it pulls back to center

    [Header("Transition Settings")]
    public float transitionSpeed = 3f;
    public float spamCooldown = 1.5f;

    [HideInInspector] public Vector3 lockedBodyRotation;

    private Vector2 currentOffset;
    private float lastTransitionTime = -100f;

    // Transition variables
    private bool isTransitioning;
    private float transitionProgress;
    private Quaternion startCharacterRot;
    private Quaternion startCameraRot;
    private Quaternion targetCharacterRot;
    private Quaternion targetCameraRot;

    void Awake()
    {
        if (character == null)
        {
            character = GetComponentInParent<FirstPersonMovement>().transform;
        }
    }

    void OnEnable()
    {
        if (Time.time - lastTransitionTime < spamCooldown)
        {
            // Fast spam detected! Skip the smooth sweep and instantly snap to the target view.
            isTransitioning = false;
            currentOffset = Vector2.zero;

            // Immediately apply target rotations so they aren't stuck looking away
            targetCharacterRot = Quaternion.Euler(lockedBodyRotation);
            character.localRotation = targetCharacterRot;
            transform.localRotation = Quaternion.identity;

            return;
        }

        lastTransitionTime = Time.time;
        isTransitioning = true;
        transitionProgress = 0f;
        currentOffset = Vector2.zero;

        startCharacterRot = character.localRotation;
        startCameraRot = transform.localRotation;

        targetCharacterRot = Quaternion.Euler(lockedBodyRotation);
        targetCameraRot = Quaternion.identity;
    }

    void Update()
    {
        if (InputManager.Instance == null) return;

        if (Time.timeScale == 0f) return;

        // Initial Transition Phase
        if (isTransitioning)
        {
            transitionProgress += Time.deltaTime * transitionSpeed;

            character.localRotation = Quaternion.Slerp(startCharacterRot, targetCharacterRot, transitionProgress);
            transform.localRotation = Quaternion.Slerp(startCameraRot, targetCameraRot, transitionProgress);

            if (transitionProgress >= 1f)
            {
                isTransitioning = false;
            }
            return;
        }

        // Rubber Band Phase
        character.localRotation = targetCharacterRot;

        Vector2 mouseDelta = InputManager.Instance.controls.Player.Look.ReadValue<Vector2>();
        currentOffset += mouseDelta * sensitivity;

        currentOffset.x = Mathf.Clamp(currentOffset.x, -maxLookAngle, maxLookAngle);
        currentOffset.y = Mathf.Clamp(currentOffset.y, -maxLookAngle, maxLookAngle);

        currentOffset = Vector2.Lerp(currentOffset, Vector2.zero, Time.deltaTime * snapBackSpeed);

        transform.localRotation = Quaternion.Euler(-currentOffset.y, currentOffset.x, 0f);
    }
}
