using UnityEngine;
using UnityEngine.VFX;

public class Target : MonoBehaviour
{
    [SerializeField] private string targetColor;
    [SerializeField] protected VisualEffect mVFX_Explosion;
    [SerializeField] protected AudioClip mAudioClip;
    private AudioSource mAudioSource;
    public string TargetColor => targetColor;
    private void Start()
    {
        mAudioSource = GetComponent<AudioSource>();
    }
    public void Die(string gunColor)
    {
        if (gunColor.Equals (targetColor))
        {
            mVFX_Explosion.SendEvent("Explosion");
            mAudioSource.PlayOneShot(mAudioClip);
            Debug.Log($"{gameObject.name} is destroeyd");
            Destroy(gameObject,.5f);
        }
    }



}
