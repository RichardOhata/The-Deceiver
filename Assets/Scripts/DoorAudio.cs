using UnityEngine;

public class DoorAudio : MonoBehaviour
{
    [SerializeField] private AudioSource doorSound;

    public bool playSound = true;

    public void PlayDoorSound()
    {
        if (playSound)
        {
            doorSound.Play();
        }
    }
}
