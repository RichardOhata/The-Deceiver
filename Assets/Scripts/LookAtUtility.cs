using UnityEngine;

public static class LookAtUtility
{
    /// <summary>
    /// Returns true if the player's camera is looking at the given target 
    /// within the specified angle and distance range.
    /// </summary>
    public static bool IsLookingAt(Transform camera, Transform target, float maxAngle, float minDistance = 0f, float maxDistance = Mathf.Infinity)
    {
        if (camera == null || target == null)
            return false;

        Vector3 toTarget = target.position - camera.position;
        float distance = toTarget.magnitude;

        // Check distance bounds
        if (distance < minDistance || distance > maxDistance)
            return false;

        // Check angle
        float angle = Vector3.Angle(camera.forward, toTarget.normalized);
        return angle <= maxAngle;
    }

    /// <summary>
    /// Returns true if the player is looking at ANY of the targets in the given list
    /// within the specified angle and distance range.
    /// </summary>
    public static bool IsLookingAtAny(Transform camera, GameObject[] targets, float maxAngle, float minDistance = 0f, float maxDistance = Mathf.Infinity)
    {
        if (camera == null || targets == null)
            return false;

        foreach (var target in targets)
        {
            if (target == null) continue;

            if (IsLookingAt(camera, target.transform, maxAngle, minDistance, maxDistance))
                return true;
        }

        return false;
    }

    /// <summary>
    /// Returns true if the player is looking at ALL targets in the given list
    /// within the specified angle and distance range.
    /// </summary>
    public static bool IsLookingAtAll(Transform camera, GameObject[] targets, float maxAngle, float minDistance = 0f, float maxDistance = Mathf.Infinity)
    {
        if (camera == null || targets == null)
            return false;

        foreach (var target in targets)
        {
            if (target == null) continue;

            if (!IsLookingAt(camera, target.transform, maxAngle, minDistance, maxDistance))
                return false;
        }

        return true;
    }


}
