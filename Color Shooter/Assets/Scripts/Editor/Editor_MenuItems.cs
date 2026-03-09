using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class Editor_MenuItems : MonoBehaviour
{
    private const string lBlueTarget = "Assets/Prefabs/Target/B_target_prefab.prefab";
    private const string lRedTarget = "Assets/Prefabs/Target/R_target_prefab.prefab";
    [MenuItem("Editor/SpawnTestTarget")]
    public static void spawnTestTarget()
    {
        GameObject lBlueTargetPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(lBlueTarget);
        GameObject lRedTargetPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(lRedTarget);

        if(lBlueTargetPrefab == null || lRedTargetPrefab == null)
        {
            Debug.LogError("One or both target prefabs could not be found. Please check the paths.");
            return;
        }
        string[] spawnPoints = { "SpawnPos",
            "SpawnPos (1)",
            "SpawnPos (2)",
            "SpawnPos (3)",
            "SpawnPos (4)",
            "SpawnPos (5)",
            "SpawnPos (6)",
            "SpawnPos (7)",
            "SpawnPos (8)",
            "SpawnPos (9)",
            "SpawnPos (10)",
        };
        foreach(string spawnPoint in spawnPoints)
        {
            GameObject lSpawnPoint = GameObject.Find(spawnPoint);
            if (lSpawnPoint != null)
            {
                GameObject targetPrefab = Random.value > 0.5f ? lBlueTargetPrefab : lRedTargetPrefab;
                PrefabUtility.InstantiatePrefab(targetPrefab, lSpawnPoint.transform);
            }
            else
            {
                Debug.LogWarning($"Spawn point '{spawnPoint}' not found in the scene.");
            }
        }
    }
    [MenuItem("Editor/DestroyTestTarget")]
    public static void destroyTestTarget()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Target");
        foreach (GameObject target in targets)
        {
            DestroyImmediate(target);
        }
    }

    [MenuItem("Editor/ResetScores")]
    public static void resetScores()
    {
        string path = Application.dataPath + "/HighScore/highscore.json";
        string content = File.ReadAllText(path);
        if(content != "")
        {
            File.WriteAllText(path, "");
            Debug.Log("Scores reset successfully.");
        }
        else
        {
            Debug.Log("No scores to reset.");
        }

    }
}
