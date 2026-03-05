using UnityEngine;

public class WarpLogic : MonoBehaviour
{
    private GameObject player;

    [SerializeField]
    private GameObject destination;
  
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

   
    public void Warp()
    {
        Rigidbody rb = player.GetComponent<Rigidbody>();
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        player.GetComponent<Transform>().position = destination.GetComponent<Transform>().position;
    }
}
