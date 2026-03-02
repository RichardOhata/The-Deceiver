using UnityEngine;

public class FillInTheHolePuzzle : Puzzle
{
    [SerializeField]
    private Camera playerCamera;

    [SerializeField]
    private GameObject target;

    [SerializeField]
    private float minDistance = 16f;

    [SerializeField]
    private float maxDistance = 20.5f;

    [SerializeField]
    private float counter = 0.0f;

    [SerializeField]
    private float counterEnd = 0.7f;


    [SerializeField]
    private GameObject reticleCube;

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
        reticleCube.SetActive(true);

        enabled = false;
    }


    private void Update()
    {
        CheckIfLooking();
      
    }

    // Set Setting FOV to 60 for this puzzle
    private void CheckIfLooking()
    {

      
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        // For debugging
        //Debug.DrawRay(ray.origin, ray.direction * 20.5f, Color.red);
        //float distance = Vector3.Distance(playerCamera.transform.position, target.transform.position);
        //Debug.Log("Distance to Target: " + distance.ToString("F2") + "m");

        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance))
        {

            Vector3 dirToHoleCenter = (target.transform.position - playerCamera.transform.position).normalized;
            float alignmentScore = Vector3.Angle(playerCamera.transform.forward, dirToHoleCenter);

            if (hit.collider.gameObject == target && hit.distance >= minDistance && alignmentScore < 0.3f && SettingsManager.Instance.reticle == 1)
            {
                counter += Time.deltaTime;

                if (counter >= counterEnd)
                {
                    SolvePuzzle();
                }
            }
            else
            {
                counter = 0.0f;
            }
        }
        else
        {
            counter = 0.0f;
        }
    }

    public void ShowReticleCube(bool setActive)
    {
        if (isSolved)
        {
            reticleCube.SetActive(setActive);
        }
    }

}
