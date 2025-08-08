using System.Collections;
using UnityEngine;

public class TargetSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] prefabTargets;
    [SerializeField] private Transform playerPosition;

    public void SpawnTarget()
    {
        int randomIndex = Random.Range(0, prefabTargets.Length);
        Instantiate(prefabTargets[randomIndex],playerPosition);
        
    }
    public void sendbeug()
    {
        Debug.Log("desactivtté");
    }
}
