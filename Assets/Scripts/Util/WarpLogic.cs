using UnityEngine;

public class WarpLogic : MonoBehaviour
{
    private GameObject player;

    [SerializeField]
    private GameObject destination;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

   
    public void Warp()
    {
        player.GetComponent<Transform>().position = destination.GetComponent<Transform>().position;
    }
}
