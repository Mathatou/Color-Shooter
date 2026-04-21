using TMPro;
using UnityEngine;

public class MyGazeInteractor : MonoBehaviour
{

    [SerializeField] private float maxDistance = 100f;
    [SerializeField] private float dwellTime = 10f;
    [SerializeField] private LayerMask TargetLayer;
    [SerializeField] private GameObject UItoHide;
    private string[] textsToCycle = { 
    "Bienvenue dans Color Shooter !", 
    "Tirez sur les cibles de la couleur adéquate !", 
    "Derrière vous se trouve deux pistolets de couleur.",
    "Tirez le levier à côté pour lancer le jeu !"};


    private Camera mainCam;
    private float gazeTimer;
    private Transform currentTarget;
    private int currentTextIndex = 0;
    private TextMeshProUGUI textZone;

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
                    if (targetTransform.name.Equals("Canvas"))
                    {
                        if (textZone == null)
                        {
                            textZone = targetTransform.GetComponentInChildren<TextMeshProUGUI>();
                        }
                        currentTextIndex = (currentTextIndex + 1) % textsToCycle.Length;
                        textZone.text = textsToCycle[currentTextIndex];
                        if (UItoHide.activeInHierarchy)
                        {
                            UItoHide.SetActive(false);
                        }
                    }
                    //rHit.collider.gameObject.SetActive(false);
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
