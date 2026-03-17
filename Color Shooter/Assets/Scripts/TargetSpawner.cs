using System.Collections.Generic;
using UnityEngine;

//A rename en GameManager

public class TargetSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] prefabTargets;
    [SerializeField] private Transform[] spawnPosition;
    [SerializeField] private float gameTime = 60.0f;
    [SerializeField] private GameObject RgunManager;
    [SerializeField] private GameObject BgunManager;
    private List<Shooter> allShooters = new List<Shooter>();
    private int[] randomSpawnIndex;
    private int numberToSpawn = 10;
    private bool hasTargetBeenSpawned = false;
    private bool hasStarted = false;
    s_HighScore player = new s_HighScore();

    private List<GameObject> spawnedTargets = new List<GameObject>();

    private void Start()
    {
        allShooters.AddRange(BgunManager.GetComponentsInChildren<Shooter>());
        allShooters.AddRange(RgunManager.GetComponentsInChildren<Shooter>());
    }


    public void SpawnTarget()
    {
        hasTargetBeenSpawned = true;
        spawnedTargets.Clear();
        /// Cette loop permet de r�cup les diff�rentes positions de mani�re al�atoire sans qu'elle ne se chevauchent
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
        // Pour �viter que le bouton soit appuy� plusieurs fois
        if (hasStarted) return;
        if(hasTargetBeenSpawned ) 
        {
            Debug.Log("Les cibles ont deja spawn");
            return;
        }
        player.playerName = "Hadrien";
        player.score = 0;
        player.accuracy = 0;
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
    public int getScore()
    {
        int score = 0;
        for (int i =0; i < allShooters.Count; i++)
        {
            score += allShooters[i].GetHitNumber();
        }
        return score;
    }
    public float getPlayerAccuracy()
    {
        float accur = 0.0f;
        int totalHit = 0;
        int totalShots = 0;
        for (int i =0; i < allShooters.Count; i++)
        {
            var currentShooter = allShooters[i];
            totalHit += currentShooter.GetHitNumber();
            totalShots += currentShooter.GetShootNumber();
        }
        // Avoid divide by zero
        if (totalShots==0)
        {
            return 0;
        }
        accur = (float)totalHit / totalShots;
        return accur*100;
    }
    private void Update()
    {
        spawnedTargets.RemoveAll(item => item == null);
        if (gameTime <= 0f)
        {
            Debug.Log("Time's up !");
            player.playerName = "Hadrien";
            //player.score = getScore();
            player.accuracy = getPlayerAccuracy();
            HighScore.WriteOnDiskPlayer(player);
            var playerList = HighScore.ReadFromDisk();
            HighScore.writeLeaderBorad(playerList);
            return;
        }
        else
        {
            //Debug.Log("Time left : " + Mathf.Ceil(gameTime) + " seconds");
            //player.score += 1;
            gameTime -= Time.deltaTime;
        }
        if (spawnedTargets.Count == 0 && hasTargetBeenSpawned)
        {
            Debug.Log("All targets destroyed");
            Debug.Log("Respawn ! ");
            SpawnTarget();
        }
    }
}
