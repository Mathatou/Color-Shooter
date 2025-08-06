using System.Data.Common;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] private Transform muzzle;
    [SerializeField] private float maxDistance = 50.0f;
    [SerializeField] private LayerMask TargetLayer;
    [SerializeField] private Color currentColor;
    public void Shoot()
    {
        Debug.Log("Start shooter");
        if (Physics.Raycast(muzzle.position, muzzle.forward, out RaycastHit rHit, maxDistance, TargetLayer))
        {
            var target = rHit.collider.gameObject;
            if (target.tag == "Target")
            {
                Debug.Log("PRRRRRRROUUUT");
            }
            
        }
        Debug.Log("End shooter");
    }
}
