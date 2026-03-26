using System.Collections;
using UnityEngine;

public class CameraDragTransition : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;

    [Header("Transition Settings")]
    public float duration = 0.5f;
    // In the Inspector, set this curve to "Ease Out" but keep the end steep
    public AnimationCurve transitionCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    public bool _isAtDestination = false;


    [SerializeField] private GameObject mainMenuManager;
    private Coroutine _activeTransition;

    public void ToggleCameraPosition()
    {
        mainMenuManager.GetComponent<MainMenuLogic>().ButtonVisibilityStatus(false);
        mainMenuManager.GetComponent<MainMenuLogic>().SecondaryButtonVisibilityStatus(false);
        if (_activeTransition != null) StopCoroutine(_activeTransition);

        _isAtDestination = !_isAtDestination;
        Transform target = _isAtDestination ? endPoint : startPoint;

        _activeTransition = StartCoroutine(MoveCamera(target));
    }

    IEnumerator MoveCamera(Transform target)
    {
        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;
        float elapsed = 0;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float percent = elapsed / duration;

            // Use the curve to define the "snappiness"
            float curvePercent = transitionCurve.Evaluate(percent);

            transform.position = Vector3.Lerp(startPos, target.position, curvePercent);
            transform.rotation = Quaternion.Slerp(startRot, target.rotation, curvePercent);

            yield return null;
        }

        transform.position = target.position;
        transform.rotation = target.rotation;
        if (_isAtDestination)
        {
            mainMenuManager.GetComponent<MainMenuLogic>().SecondaryButtonVisibilityStatus(true);
        } else
        {
            mainMenuManager.GetComponent<MainMenuLogic>().ButtonVisibilityStatus(true);
        }
    }
}
