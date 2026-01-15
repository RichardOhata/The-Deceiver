using UnityEngine;

public class FillInTheHolePuzzle : Puzzle
{
    [SerializeField]
    private Camera playerCamera;

    [SerializeField]
    private GameObject target;

    private void Start()
    {
        playerCamera = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Camera>();
    }
 
    public override void StartPuzzle()
    {
        base.StartPuzzle();
     

    }

    public override void SolvePuzzle()
    {
        base.SolvePuzzle();
      
    }


    private void Update()
    {
        CheckIfLooking();
      
    }

    // Set Setting FOV to 60 for this puzzle
    private void CheckIfLooking()
    {

        // Debugging ray
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        Debug.DrawRay(ray.origin, ray.direction * 20.5f, Color.red);
        float distance = Vector3.Distance(playerCamera.transform.position, target.transform.position);
        Debug.Log("Distance to Target: " + distance.ToString("F2") + "m");

        if (Physics.Raycast(ray, out RaycastHit hit, 20.5f)) 
        {
          
            if (hit.collider.gameObject == target && hit.distance >= 16f)
            {
            
                Vector3 dirToHoleCenter = (target.transform.position - playerCamera.transform.position).normalized;
                float alignmentScore = Vector3.Angle(playerCamera.transform.forward, dirToHoleCenter);

             
                if (alignmentScore < 0.3f) // Adjust angle for tweaking precison
                {
                    Debug.Log("Yes - Distance and Alignment are Perfect!");

                    // Timer Logic
                    SolvePuzzle();
                    //Set this component to be inactive
                }
            }
        }

    }

}
