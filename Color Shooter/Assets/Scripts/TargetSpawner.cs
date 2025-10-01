using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] prefabTargets;
    [SerializeField] private Transform[] spawnPosition;
    [SerializeField] private float gameTime = 60f;
    
    private int[] randomSpawnIndex;
    private int numberToSpawn = 10;
    private bool hasTargetBeenSpawned = false;
    private bool hasStarted = false;

    private List<GameObject> spawnedTargets = new List<GameObject>();
    public void SpawnTarget()
    {
        hasTargetBeenSpawned = true;
        spawnedTargets.Clear();
        /// Cette loop permet de récup les différentes positions de manière aléatoire sans qu'elle ne se chevauchent
        randomSpawnIndex = new int[numberToSpawn];
        for (int i = 0; i < numberToSpawn; i++) 
        {
            randomSpawnIndex[i] = Random.Range(0, spawnPosition.Length);
            for (int j = 0; j < i; j++) 
            {
                if (randomSpawnIndex[i] == randomSpawnIndex[j])
                {
                    randomSpawnIndex[i] = Random.Range(0, spawnPosition.Length);
                    j = -1; // Restart la loop
                }
            }
            int randomTargetIndex = Random.Range(0, prefabTargets.Length);
            GameObject singleTarget = Instantiate(prefabTargets[randomTargetIndex], spawnPosition[randomSpawnIndex[i]].transform);
            spawnedTargets.Add(singleTarget);
        }

    }
    public void pushButton()
    {
        // Pour éviter que le bouton soit appuyé plusieurs fois
        if (hasStarted) return;
        hasStarted = true;
        SpawnTarget();
    }
    public void resetGame()
    {
        gameTime = 60f;
        hasStarted = false;
        hasTargetBeenSpawned = false;
        foreach (var target in spawnedTargets)
        {
            if (target != null)
            {
                Destroy(target);
            }
        }
        spawnedTargets.Clear();
    }

    private void Update()
    {
        spawnedTargets.RemoveAll(item => item == null);
        if(gameTime <= 0f)
        {
            Debug.Log("Time's up !");
            return;
        }
        else
        {
            gameTime -= Time.deltaTime;
        }
        if (spawnedTargets.Count == 0 && hasTargetBeenSpawned)
        {
            Debug.Log("All targets destroyed");
            Debug.Log("Respawn ! ");
            SpawnTarget();
            //hasSpawned = false;
        }
    }
}
