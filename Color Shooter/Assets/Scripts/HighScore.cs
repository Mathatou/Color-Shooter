using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;

[Serializable]
public class HighScoreList
{
    public List<s_HighScore> mAllScores = new List<s_HighScore>();
}
[Serializable]
public class s_HighScore
{
    public string playerName;
    public int score;
    public float accuracy;
}

public class HighScore : MonoBehaviour
{
#if UNITY_EDITOR
    protected static string JSONDATAPATH = Application.dataPath + "/HighScore/highscore.json";
    protected static string JSONDIRECTORY = Application.dataPath + "/HighScore/";
#elif UNITY_ANDROID
    protected static string JSONDATAPATH = Application.persistentDataPath + "/HighScore/highscore.json";
    protected static string JSONDIRECTORY = Application.persistentDataPath + "/HighScore/";
#endif
    [SerializeField]
    public static TextMeshProUGUI mLeaderboard;
    
    public static void WriteOnDiskPlayer(s_HighScore pPlayer)
    {
        if (!Directory.Exists(JSONDIRECTORY))
        {
            Debug.Log("Directory does not exists Creating...");
            Directory.CreateDirectory(JSONDIRECTORY);
        }
        if (!File.Exists(JSONDATAPATH))
        {
            Debug.Log("File does not exists Creating...");
            File.Create(JSONDATAPATH).Dispose();
        }
        string jsonContent = File.ReadAllText(JSONDATAPATH);
        HighScoreList lHighScores = JsonUtility.FromJson<HighScoreList>(jsonContent);
        if(lHighScores == null)
        {
            lHighScores = new HighScoreList();
        }
        lHighScores.mAllScores.Add(pPlayer);
        // If there are more than 5 scores, remove the lowest one
        if (lHighScores.mAllScores.Count > 5)
        {
            int minScore = lHighScores.mAllScores[0].score;
            int indexToRemove = 0;
            for (int i = 1; i < lHighScores.mAllScores.Count; i++)
            {
                if(minScore > lHighScores.mAllScores[i].score)
                {
                    minScore = lHighScores.mAllScores[i].score;
                    indexToRemove = i;
                }
            }
            //Debug.Log("Removing score: " + lHighScores.mAllScores[indexToRemove].score + " from player: " + lHighScores.mAllScores[indexToRemove].playerName);
            lHighScores.mAllScores.RemoveAt(indexToRemove);
        }

        jsonContent = JsonUtility.ToJson(lHighScores);
        File.WriteAllText(JSONDATAPATH, jsonContent);
    }
    public static HighScoreList ReadFromDisk()
    {
        string json = File.ReadAllText(JSONDATAPATH);
        HighScoreList playerFromDisk = JsonUtility.FromJson<HighScoreList>(json);
        return playerFromDisk;
    }
    public static void writeLeaderBorad(HighScoreList pListHighScore)
    {
        string leaderboardText = "Leaderboard:\n";
        pListHighScore.mAllScores = pListHighScore.mAllScores.OrderByDescending(score => score.score).ToList();
        for (int i = 0; i < pListHighScore.mAllScores.Count; i++)
        {
            leaderboardText += $"{i + 1}. {pListHighScore.mAllScores[i].playerName} - Score: {pListHighScore.mAllScores[i].score}, Accuracy: {pListHighScore.mAllScores[i].accuracy}%\n";
        }
        mLeaderboard.text = leaderboardText;
    }
    private void Start()
    {
        mLeaderboard = GameObject.Find("LeaderBoard").GetComponent<TextMeshProUGUI>();
        //WriteOnDiskPlayer(player);
        //var playerList = ReadFromDisk();
        //writeLeaderBorad(playerList);
    }
}