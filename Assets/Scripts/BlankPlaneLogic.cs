using UnityEngine;

public class BlankPlaneLogic : MonoBehaviour
{
    [SerializeField]
    private Material blue;

    [SerializeField]
    private Material green;

    [SerializeField]
    private FirstPersonAudio playerAudio; 

    private void Awake()
    {
        playerAudio = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<FirstPersonAudio>();
    }

    public enum CorrectPathFlag
    {
        correct,
        incorrect
        
    }

    [SerializeField]
    private CorrectPathFlag flag;

    [SerializeField]
    public bool steppedOn = false;

    public void SetStep()
    {
        gameObject.GetComponent<MeshRenderer>().material = green;
        steppedOn = true;
        if (flag == CorrectPathFlag.correct)
        {
            playerAudio.stepAudio.mute = false;
        } else
        {
            playerAudio.stepAudio.mute = true;
        }
    }

    public void ResetStatus()
    {
        gameObject.GetComponent<MeshRenderer>().material = blue;
        steppedOn = false;
    }
}
