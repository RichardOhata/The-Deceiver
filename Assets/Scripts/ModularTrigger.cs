using UnityEngine;
using UnityEngine.Events;

public class ModularTrigger : MonoBehaviour
{
    [Header("Trigger Settings")]
    public string targetTag = "Player"; // Only trigger on objects with this tag

    [Header("Events")]
    public UnityEvent onEnter; // Called when target enters
    public UnityEvent onExit;  // Called when target exits

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            onEnter?.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            onExit?.Invoke();
        }
    }
}
