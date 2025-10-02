using UnityEngine;

public abstract class Shooter : MonoBehaviour
{
    [SerializeField] protected Transform muzzle;
    [SerializeField] protected float maxDistance = 50.0f;
    [SerializeField] protected LayerMask TargetLayer;
    [SerializeField] protected string currentColor;

    protected int shootNumber = 0;
    protected int hitNumber = 0;
    public int GetShootNumber()
    {
        return shootNumber;
    }
    public int GetHitNumber()
    {
        return hitNumber;
    }
    protected virtual void Start()
    {
        Init();
    }

    protected abstract void Init();

    public virtual void Shoot()
    {
        if (Physics.Raycast(muzzle.position, muzzle.forward, out RaycastHit rHit, maxDistance, TargetLayer))
        {
            var target = rHit.collider.GetComponent<Target>();
            if (target == null)
            {
                Debug.LogWarning("C'est pas une cible �a ");
                return;
            }
            var myTargetColor = target.TargetColor;
            if (target != null)
            {
                if (myTargetColor.Equals(currentColor))
                {
                    target.Die(currentColor);
                    Debug.Log($"Cible {myTargetColor} touch� avec gun {currentColor}");
                    hitNumber++;
                }

            }
        }
        shootNumber++;

    }
    public float GetAccuracy()
    {
        if (shootNumber == 0) return 0;
        return (float)hitNumber / shootNumber * 100;
    }
}