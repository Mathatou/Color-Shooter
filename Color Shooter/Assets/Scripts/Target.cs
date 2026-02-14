using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] private string targetColor;
    public string TargetColor => targetColor;

    public void Die(string gunColor)
    {
        if (gunColor.Equals (targetColor))
        {
            Debug.Log($"{gameObject.name} is destroeyd");
            Destroy(gameObject);
        }
    }



}
