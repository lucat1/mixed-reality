using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Step
{
    public int stepN;
    public string step_description;
    public string component_code;
}

[System.Serializable]
public class StepsWrapper
{
    public string name;
    public List<Step> steps;

    public static StepsWrapper CreateFromJSON(string jsonString)
    {
        Debug.Log(jsonString);
        return JsonUtility.FromJson<StepsWrapper>(jsonString);
    }
}