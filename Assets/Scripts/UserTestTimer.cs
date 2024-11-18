using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.IO;

public class SceneTimeTracker : MonoBehaviour
{
    private Dictionary<string, float> sceneTimes = new (); // Store time spent in each scene
    private Dictionary<string, int> sceneCount = new (); // Store time spent in each scene
    private string currentSceneName;
    private float sceneStartTime;

    void Awake()
    {
        DontDestroyOnLoad(this); // Ensure this object persists across scenes
    }

    string SceneName(string name){
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
            // Update the current scene's time
            sceneTimes[currentSceneName] = Time.time - sceneStartTime;
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Save the time for the previous scene
        if (!string.IsNullOrEmpty(currentSceneName))
        {
            sceneTimes[currentSceneName] = Time.time - sceneStartTime;
        }

        // Update to the new scene
        
        // init count
        if (sceneCount.TryGetValue(scene.name, out int currentCount))
        {
            sceneCount[scene.name] = currentCount + 1;
        }
        else
        {
            sceneCount[scene.name] = 1;  // Initialize to 1 if the key does not exist
        }
        currentSceneName = SceneName(scene.name);
        sceneStartTime = Time.time;

        // Ensure the new scene is in the dictionary
        if (!sceneTimes.ContainsKey(currentSceneName))
        {
            sceneTimes[currentSceneName] = 0f;
        }
    }

    public void SaveTimesToCSV(string filePath)
    {
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            // Write header
            writer.WriteLine(string.Join(",", sceneTimes.Keys));

            // Write row of times
            writer.WriteLine(string.Join(",", sceneTimes.Values));
        }

        Debug.Log($"Scene times saved to {filePath}");
    }

    void OnApplicationQuit()
    {
        // Save data to CSV on application exit
        SaveTimesToCSV(Path.Combine(Application.dataPath, $"run_{Time.time}.csv"));
    }
}