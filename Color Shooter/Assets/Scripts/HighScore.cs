using UnityEngine;
using System.IO;
public class HighScore
{
    public string playerName;
    public int score;
    public float accuracy;
}
/*
public class mainClass : MonoBehaviour
{
    var player = new HighScore
    {
        playerName = "Bob",
        score = 100,
        accuracy = 95.5f
    };

    var json = JsonUtility.ToJson(player);
    File.WriteAllText(Path.Combine(Application.persistentDataPath, "highscore.json"), json);
}*/