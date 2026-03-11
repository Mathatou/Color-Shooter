using UnityEngine;
using UnityEngine.VFX;

public abstract class Shooter : MonoBehaviour
{
    [SerializeField] protected Transform muzzle;
    [SerializeField] protected float maxDistance = 50.0f;
    [SerializeField] protected LayerMask TargetLayer;
    [SerializeField] protected string currentColor;
    [SerializeField] protected VisualEffect mVFX_MuzzleFLash;
    [SerializeField] protected AudioClip[] mShootSFXs;
    protected AudioSource mAudioSource;

    protected int shootNumber = 0;
    protected int hitNumber = 0;
    /// <summary>
    /// Return the number of gunshot fired by the shooter
    /// </summary>
    public int GetShootNumber()
    {
        return shootNumber;
    }
    /// <summary>
    /// Return the number of target hit by the shooter
    /// </summary>
    public int GetHitNumber()
    {
        return hitNumber;
    }
    protected virtual void Start()
    {
        mAudioSource = this.GetComponent<AudioSource>();
        Init();
    }

    protected abstract void Init();

    public virtual void Shoot()
    {
        playVFX();
        playSFX();
        if (Physics.Raycast(muzzle.position, muzzle.forward, out RaycastHit rHit, maxDistance, TargetLayer))
        {
            var target = rHit.collider.GetComponent<Target>();
            if (target == null)
            {
                Debug.LogWarning("C'est pas une cible ca ");
                return;
            }
            var myTargetColor = target.TargetColor;
            if (myTargetColor.Equals(currentColor))
            {
                target.Die(currentColor);
                Debug.Log($"Cible {myTargetColor} touche avec gun {currentColor}");
                hitNumber++;
            }
        }
        shootNumber++;

    }
    private void playVFX()
    {
        mVFX_MuzzleFLash.SendEvent("Fire");
    }
    private void playSFX()
    {
        int mIndex = Random.Range(0, mShootSFXs.Length);
        mAudioSource.PlayOneShot(mShootSFXs[mIndex]);
    }
    public float GunAccuracy()
    {
        if (shootNumber == 0) return 0;
        return (float)hitNumber / shootNumber * 100;
    }
}