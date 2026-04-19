using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject[] prefabTargets;
    [SerializeField] private Transform[] spawnPosition;
    [SerializeField] private float gameTime = 0.0f;
    [SerializeField] private GameObject RgunManager;
    [SerializeField] private GameObject BgunManager;
    [SerializeField] private GameObject mNameArea;
    [SerializeField] private TargetPool ObjectPool; 
    [SerializeField] protected AudioClip[] mErrorTextAreaSFX;
    protected AudioSource mTextAreaAudioSource;
    private TextMeshProUGUI timerDisplay;
    private List<Shooter> allShooters = new List<Shooter>();
    private int numberToSpawn = 10;
    private bool hasTargetBeenSpawned = false;
    private bool hasStarted = false;
    private bool doWeWrite = true;
    private string mPlayerName;
    s_HighScore player = new s_HighScore();

    private List<GameObject> spawnedTargets = new List<GameObject>();

    private void Start()
    {
        allShooters.AddRange(BgunManager.GetComponentsInChildren<Shooter>());
        allShooters.AddRange(RgunManager.GetComponentsInChildren<Shooter>());
        GameObject.Find("TimerDisplay").TryGetComponent<TextMeshProUGUI>(out timerDisplay);
        mTextAreaAudioSource = GameObject.Find("NameArea").GetComponent<AudioSource>();
    }

    #region Target Management
    public void SpawnTarget()
    {
        hasTargetBeenSpawned = true;
        spawnedTargets.Clear();

        // Methode de Fisher-Yates pour melanger les positions de spawn de maniere aleatoire
        List<Transform> shuffledList = new List<Transform>(spawnPosition);
        for(int i = 0; i < shuffledList.Count; i++)
        {
            Transform temp = shuffledList[i];
            int randomIndex = Random.Range(i, shuffledList.Count);
            shuffledList[i] = shuffledList[randomIndex];
            shuffledList[randomIndex] = temp;
        }

        int spawnCount = Mathf.Min(numberToSpawn, shuffledList.Count);
        for (int i = 0; i < spawnCount; i++)
        {
            int randomTargetIndex = Random.Range(0, prefabTargets.Length);
            GameObject singleTarget = Instantiate
                (
                    prefabTargets[randomTargetIndex],
                    shuffledList[i]
                );
            spawnedTargets.Add(singleTarget);
        }

        /* Methode vue en E2 il me semble, utilise pour le projet E3 Unity
        /// Cette loop permet de recup les différentes positions de maniere aleatoire sans qu'elle ne se chevauchent
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
        */
    }
    public void DestroyTargets()
    {
        for (int i = 0; i < spawnedTargets.Count; i++)
        {
            var target = spawnedTargets[i];
            if (target != null)
            {
                Destroy(target);
            }
        }
        spawnedTargets.Clear();
    }
    #endregion

    #region Game State Management
    public void StarGameByPushingButton()
    {
        // Pour eviter que le bouton soit appuye plusieurs fois
        if (hasStarted) return;
        if(mPlayerName == null || mPlayerName == "")
        {
            // Faire un son d'erreur qui indique que le nom du joueur est requis
            playErrorSFX();
            return;
        }
        if (hasTargetBeenSpawned)
        {
            Debug.Log("Les cibles ont deja spawn");
            return;
        }
        player.playerName = mPlayerName; // A remplacer par un input field pour que le joueur puisse rentrer son nom
        player.score = 0;
        player.accuracy = 0;
        gameTime = 60.0f;
        hasStarted = true;
        doWeWrite = true;
        mNameArea.SetActive(false);
        SpawnTarget();
    }
    public void resetGame()
    {
        //gameTime = 60f; // On ne relance pas le jeu de suite
        hasStarted = false;
        hasTargetBeenSpawned = false;
        DestroyTargets();
    }
    void endGame()
    {
        timerDisplay.text = "Time's up !";
        Debug.Log("Time's up !");
        //player.playerName = "Hadrien";
        player.score = getScore();
        player.accuracy = getPlayerAccuracy();

        if (doWeWrite)
        {
            HighScore.WriteOnDiskPlayer(player);
            var playerList = HighScore.ReadFromDisk();
            HighScore.writeLeaderBorad(playerList);
            doWeWrite = false;
        }
        DestroyTargets();
        mNameArea.SetActive(true);

    }
    #endregion

    #region Player Stats
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

    /// <summary>
    /// Lorsque le joueur clique sur le bouton valider, 
    /// le nom qu'il a precedemment entre est recupere et stocke dans la variable mPlayerName, 
    /// puis la zone de saisie du nom est cachee
    /// </summary>
    public void OnClickSendName()
    {
        mPlayerName = GameObject.Find("NameText").GetComponent<TextMeshProUGUI>().text;
        Debug.Log("Player name : " + mPlayerName);
    }

    /// <summary>
    /// Si aucun nom n'est entre un son d'erreur est joue lorsque le joueur clique sur le bouton pour commencer la partie
    /// </summary>
    private void playErrorSFX()
    {
        int mIndex = Random.Range(0, mErrorTextAreaSFX.Length);
        mTextAreaAudioSource.PlayOneShot(mErrorTextAreaSFX[mIndex]);
    }
#endregion

    private void Update()
    {
        spawnedTargets.RemoveAll(item => item == null);
        timerDisplay.text = "Time : " + Mathf.Ceil(gameTime).ToString();
        if (gameTime <= 0f)
        {
            endGame();
            return;
        }
        else
        {
            if (spawnedTargets.Count == 0 && hasTargetBeenSpawned)
            {
                Debug.Log("All targets destroyed");
                Debug.Log("Respawn ! ");
                SpawnTarget();
            }
            gameTime -= Time.deltaTime;
        }
    }
}
