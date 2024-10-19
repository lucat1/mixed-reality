using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Step
{
    public int stepN;
    public string step_description; // Matches the JSON key
    public string component_code; // Matches the JSON key
}

[System.Serializable]
public class StepsWrapper
{
    public string name;
    public List<Step> steps; // List of collections

    public static StepsWrapper CreateFromJSON(string jsonString)
    {
        Debug.Log(jsonString);
        return JsonUtility.FromJson<StepsWrapper>(jsonString);
    }
}