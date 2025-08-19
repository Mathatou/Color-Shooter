using UnityEngine;

public class MyGazeInteractor : MonoBehaviour
{

    [SerializeField] private float maxDistance = 100f;
    [SerializeField] private float dwellTime = 10f;
    [SerializeField] private LayerMask TargetLayer;
    
    private Camera mainCam;
    private float gazeTimer;
    private Transform currentTarget;

    private void Awake()
    {
        mainCam = Camera.main;        
    }
    // Update is called once per frame
    void Update()
    {
        var camTransform = mainCam.transform;
        if (Physics.Raycast(camTransform.position, camTransform.forward, out RaycastHit rHit, maxDistance,TargetLayer))
        {

            var targetTransform = rHit.transform;
            if (targetTransform == currentTarget)
            {
                gazeTimer += Time.deltaTime;
                if(gazeTimer >= dwellTime)
                {
                    Debug.Log("Gaze selected : " + targetTransform.name);
                    rHit.collider.gameObject.SetActive(false);
                    gazeTimer = 0f;
                }
            }
            else
            {
                currentTarget = targetTransform;
                gazeTimer = 0f;
            }
        }
        else
        {
            currentTarget = null;
        }

    }
}
