using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class TimeTracker : MonoBehaviour
{
    // variables for monitoring scenes change
    private Dictionary<string, float> sceneTimes = new(); 
    private Dictionary<string, int> sceneCount = new();
    private string currentSceneName;
    private float sceneStartTime;
    public static TimeTracker Instance { get; private set; }
    void Awake()
    {
        // Ensure only one instance exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
            Destroy(gameObject); 
    }

    // assemble the scene name based on the count
    string SceneName(string name)
    {
        return $"{name}_{sceneCount[name]}";
    }

    void Start()
    {
        Instance = this;
        // initialize counter for the current scene
        sceneCount[SceneManager.GetActiveScene().name] = 1;
        currentSceneName = SceneName(SceneManager.GetActiveScene().name);
        sceneStartTime = Time.time;

        // Listen for scene changes
        SceneManager.sceneLoaded += OnSceneLoaded; 

        SaveTimesToCSV(actionDurations, Application.persistentDataPath, $"diocane");
    }

    void Update()
    {
        if (!string.IsNullOrEmpty(currentSceneName))
            sceneTimes[currentSceneName] = Time.time - sceneStartTime;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"[UseTestTimer] OnSceneLoaded({scene})");
        if (!string.IsNullOrEmpty(currentSceneName))
            sceneTimes[currentSceneName] = Time.time - sceneStartTime;

        if (sceneCount.TryGetValue(scene.name, out int currentCount))
            sceneCount[scene.name] = currentCount + 1;
        else
            sceneCount[scene.name] = 1;
        currentSceneName = SceneName(scene.name);
        sceneStartTime = Time.time;

        if (!sceneTimes.ContainsKey(currentSceneName))
            sceneTimes[currentSceneName] = 0f;
    }

    // save the dictionary of the times into a CSV
    public void SaveTimesToCSV(Dictionary<string, float> data, string folderPathRoot, string FileName)
    {
        if (!Directory.Exists(folderPathRoot))
            Directory.CreateDirectory(folderPathRoot);

        string filePath = Path.Combine(folderPathRoot, $"{FileName}.csv");
        SaveIntervalsTimeToCSV(data, filePath);

        Debug.Log($"[UserTestTimer] Scene times saved to {filePath}");
    }

    private void SaveIntervalsTimeToCSV(Dictionary<string, float> data, string filePath)
    {
        Debug.Log("[UserTestTimer] saving intervals to CSV: " + filePath);
        var result = "";
        if (filePath.Contains("action"))
        {
            result += "Scene, Action ,Time\n";
            foreach (KeyValuePair<string, float> kvp in data)
            {
                string[] key = kvp.Key.Split('|');
                result += $"{key[0]},{key[1]},{kvp.Value}\n";
            }
        }
        else
        {
            result += "Sene,Time";
            foreach (KeyValuePair<string, float> kvp in data)
                result += $"{kvp.Key},{kvp.Value}\n";
        }
        UnityEngine.Windows.File.WriteAllBytes(filePath, Encoding.ASCII.GetBytes(result));
    }

    // variables for monitoring individual actions time
    private Dictionary<string, float> actionDurations = new Dictionary<string, float>();
    private string currentAction;
    private float actionStartTime;

    public bool challengeOn = true;

    public void StartAction(string actionName)
    {
        Debug.Log($"[UserTestTimer] Starting time for scene: {actionName}");
        if (!string.IsNullOrEmpty(currentAction))
            EndAction(); // End previous action if any

        currentAction = actionName;
        actionStartTime = Time.time;
    }

    // end the action and save it to the dictionary
    public void EndAction()
    {
        Debug.Log($"[UserTestTimer] Finished action: {currentAction}");
        if (!string.IsNullOrEmpty(currentAction))
        {
            float duration = Time.time - actionStartTime;
            actionDurations[currentAction] = duration;

            currentAction = null;
        }
        Debug.Log("[UserTestTimer] Current Dictionary:");
        foreach(string k in actionDurations.Keys)
            Debug.Log($"[UserTestTimer] {k} : {actionDurations[k]}");
    }

    // save everyting when we quit the app
    public void SaveTimings()
    {
        string timestamp = System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        SaveTimesToCSV( sceneTimes, Application.persistentDataPath, $"scene_times_{timestamp}");
        SaveTimesToCSV( actionDurations, Application.persistentDataPath, $"action_times_{timestamp}");
    }
}
