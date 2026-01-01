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

    public void SetStep()
    {
        this.gameObject.GetComponent<MeshRenderer>().material = green;
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
        this.gameObject.GetComponent<MeshRenderer>().material = blue;
    }
}
