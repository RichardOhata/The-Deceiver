using UnityEngine;

public class BlankPlaneLogic : MonoBehaviour
{
    [SerializeField]
    private Material blue;

    [SerializeField]
    private Material green;

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
            GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<FirstPersonAudio>().stepAudio.mute = false;
        } else
        {
            GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<FirstPersonAudio>().stepAudio.mute = true;
        }
    }

    public void ResetStatus()
    {
        gameObject.GetComponent<MeshRenderer>().material = blue;
        steppedOn = false;
    }
}
