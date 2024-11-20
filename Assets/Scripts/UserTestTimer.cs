using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.IO;

public class SceneTimeTracker : MonoBehaviour
{
    private Dictionary<string, float> sceneTimes = new(); 
    private Dictionary<string, int> sceneCount = new();
    private string currentSceneName;
    private float sceneStartTime;

    void Awake()
    {
        DontDestroyOnLoad(this);
    }

    string SceneName(string name)
    {
        return $"{name}_{sceneCount[name]}";
    }

    void Start()
    {
        sceneCount[SceneManager.GetActiveScene().name] = 1;
        currentSceneName = SceneName(SceneManager.GetActiveScene().name);
        sceneStartTime = Time.time;
        SceneManager.sceneLoaded += OnSceneLoaded; // Listen for scene changes
    }

    void Update()
    {
        if (!string.IsNullOrEmpty(currentSceneName))
        {
            sceneTimes[currentSceneName] = Time.time - sceneStartTime;
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (!string.IsNullOrEmpty(currentSceneName))
        {
            sceneTimes[currentSceneName] = Time.time - sceneStartTime;
        }

        if (sceneCount.TryGetValue(scene.name, out int currentCount))
        {
            sceneCount[scene.name] = currentCount + 1;
        }
        else
        {
            sceneCount[scene.name] = 1;
        }
        currentSceneName = SceneName(scene.name);
        sceneStartTime = Time.time;

        if (!sceneTimes.ContainsKey(currentSceneName))
        {
            sceneTimes[currentSceneName] = 0f;
        }
    }

    public void SaveTimesToCSV(string folderPath)
    {
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        // Save the times for the challenge scene
        if (sceneTimes.ContainsKey("challenge"))
        {
            string challengeFilePath = Path.Combine(folderPath, "challenge.csv");
            SaveSceneTimeToCSV(challengeFilePath, "challenge");
        }

        // Save the times for the tutorial scene
        if (sceneTimes.ContainsKey("tutorial"))
        {
            string tutorialFilePath = Path.Combine(folderPath, "tutorial.csv");
            SaveSceneTimeToCSV(tutorialFilePath, "tutorial");
        }

        Debug.Log($"Scene times saved to {folderPath}");
    }

    private void SaveSceneTimeToCSV(string filePath, string sceneName)
    {
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            writer.WriteLine("Scene,Time");
            writer.WriteLine($"{sceneName},{sceneTimes[sceneName]}");
        }
    }

    void OnApplicationQuit()
    {
        string timestamp = System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        string folderPath = Path.Combine(Application.persistentDataPath, timestamp);
        SaveTimesToCSV(folderPath);
    }
}
