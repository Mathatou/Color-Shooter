using UnityEngine;
using UnityEngine.VFX;

public class Target : MonoBehaviour
{
    [SerializeField] private string targetColor;
    [SerializeField] protected VisualEffect mVFX_Explosion;
    public string TargetColor => targetColor;
    
    public void Die(string gunColor)
    {
        if (gunColor.Equals (targetColor))
        {
            mVFX_Explosion.SendEvent("Explosion");
            Debug.Log($"{gameObject.name} is destroeyd");
            Destroy(gameObject,.5f);
        }
    }



}
