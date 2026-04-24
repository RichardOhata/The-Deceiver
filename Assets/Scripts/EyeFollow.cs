using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EyeFollow : MonoBehaviour
{
    [Header("References")]
    public Transform pupilTransform;

    [Header("Settings")]
    public float movementRange = 2f;
    public float lookThreshold = 0.85f;
    public float trackSpeed = 5f;
    public float snapBackSpeed = 20f;

    [Header("Cooldown")]
    [Tooltip("How many seconds the eye stays frozen after you look away")]
    public float cooldownDuration = 3f;


    private Vector3 initialPupilPos;
    private Transform player;
    private Transform playerCam;

    private float currentCooldown = 0f;


    void Start()
    {
        if (pupilTransform != null)
            initialPupilPos = pupilTransform.localPosition;

        GameObject pObj = GameObject.FindGameObjectWithTag("Player");
        if (pObj != null)
            player = pObj.transform;

        if (Camera.main != null)
            playerCam = Camera.main.transform;
    }

    void Update()
    {
        if (pupilTransform == null || player == null || playerCam == null) return;
        Vector3 dirToEye = (transform.position - playerCam.position).normalized;
        bool isLookingAtEye = Vector3.Dot(playerCam.forward, dirToEye) > lookThreshold;
        if (isLookingAtEye)
        { 
            pupilTransform.localPosition = Vector3.Lerp(pupilTransform.localPosition, initialPupilPos, Time.deltaTime * snapBackSpeed);
            currentCooldown = cooldownDuration;
        }
        else
        {
            if (currentCooldown > 0f)
            {
                currentCooldown -= Time.deltaTime;
                pupilTransform.localPosition = Vector3.Lerp(pupilTransform.localPosition, initialPupilPos, Time.deltaTime * snapBackSpeed);
            }
            else
            {
                Vector3 localPlayerPos = transform.InverseTransformPoint(player.position);
                Vector2 flatDir = new Vector2(localPlayerPos.x, localPlayerPos.y).normalized;
                Vector3 targetPos = initialPupilPos + new Vector3(flatDir.x, flatDir.y, 0f) * movementRange;
                pupilTransform.localPosition = Vector3.Lerp(pupilTransform.localPosition, targetPos, Time.deltaTime * trackSpeed);
            }
        }
    }
}
