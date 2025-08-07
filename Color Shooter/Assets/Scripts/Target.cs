using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] private string targetColor;
    public string TargetColor => targetColor;
}
