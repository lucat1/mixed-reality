using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System;

class Line {
    public string Action;
    public float Time;

    public Line(string action, float time) {
        Action = action;
        Time = time;
    }
}

public class Timer
{
    private static Timer instance = null;
    public static Timer Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new Timer();
            }
            return instance;
        }
    }
    private List<Line> lines = new(); 
    private readonly static string folder = Application.persistentDataPath;
    private readonly string fileName;

    private Timer() {
        lastTime = Time.time;
        fileName = DateTime.Now.ToString("MM-dd-yyyy+hh\\-mm");
    }

    private void SaveCSV()
    {
        if (!Directory.Exists(folder))
            Directory.CreateDirectory(folder);

        string filePath = Path.Combine(folder, $"{fileName}.csv");
        Debug.Log($"[Timer] saving {lines.Count} lines to {fileName} ({filePath})");

        var result = "Action,Time\n";
        foreach (var line in lines)
            result += $"{line.Action},{line.Time}\n";

        UnityEngine.Windows.File.WriteAllBytes(filePath, Encoding.ASCII.GetBytes(result));
    }

    public bool challengeOn = true;

    float lastTime;
    string lastAction;

    public void Action(string action) {
        Flush();
        lastTime = Time.time;
        lastAction = action;
    }

    public void Flush() {
        if(lastAction == null)
            return;

        var elapsedTime = Time.time - lastTime;
        if(elapsedTime == 0)
            return;

        lines.Add(new Line(lastAction, elapsedTime));
        SaveCSV();
        lastAction = null;
    }
}