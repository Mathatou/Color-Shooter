using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] private Transform muzzle;
    [SerializeField] private float maxDistance = 50.0f;
    [SerializeField] private LayerMask TargetLayer;
    [SerializeField] private Color currentColor;
    public void Shoot()
    {
        if (Physics.Raycast(muzzle.position, muzzle.forward, out RaycastHit rHit, maxDistance, TargetLayer))
        {
            Debug.Log("PRRRRRRROUUUT");
        }
    }
}
