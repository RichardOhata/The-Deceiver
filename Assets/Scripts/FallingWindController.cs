using UnityEngine;

public class FallingWindController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private AudioSource windAudioSource;
    private Rigidbody rb;

    [Header("Settings")]
    [Tooltip("The speed at which the wind starts becoming audible.")]
    public float fallThreshold = 8f;

    [Tooltip("The speed at which the wind reaches maximum volume.")]
    public float maxSpeedForVolume = 25f;

    [Range(0, 1)]
    public float maxVolume = 0.8f;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();


        if (windAudioSource != null)
        {
            windAudioSource.loop = true;
            windAudioSource.volume = 0;
            windAudioSource.Play();
        }
    }

    void Update()
    {
        if (rb == null || windAudioSource == null) return;
        float downwardSpeed = -rb.linearVelocity.y;

        if (downwardSpeed > fallThreshold)
        { 
            float targetVolume = Mathf.InverseLerp(fallThreshold, maxSpeedForVolume, downwardSpeed);

           
            windAudioSource.volume = Mathf.Lerp(windAudioSource.volume, targetVolume * maxVolume, Time.deltaTime * 5f);
        }
        else
        {
          
            windAudioSource.volume = Mathf.MoveTowards(windAudioSource.volume, 0, Time.deltaTime * 2f);
        }
    }
}
