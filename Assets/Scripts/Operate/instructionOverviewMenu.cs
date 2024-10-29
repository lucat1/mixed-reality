using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;

public class instructionOverviewMenu : MonoBehaviour
{
    public GameObject stepOverviewMenu;
    private int stepCount = 0;
    public string OperationsFile;
    public cahngeObjectsVisibility visibilityScript;
    private List<string> components;
    private List<string> step_description;
    private StepsWrapper stepsData;




    public void startManteinance()
    {
        Vector3 cameraPosition = Camera.main.transform.position;

        stepOverviewMenu.transform.position = cameraPosition + new Vector3(0.2f,0,0.323f);
        stepOverviewMenu.SetActive(true);
        Transform textTransform = stepOverviewMenu.transform.Find("ManipulationContainer/ManipulationBar/content/Text");
        TMP_Text instructionText = textTransform.GetComponent<TMP_Text>();

        string composedText = "<size=60><b>Step " + (stepCount+1).ToString() + "</size></b>" + "<br>" +  step_description[0];
        instructionText.text = composedText;
        visibilityScript.highlightObjects(new List<string> { components[0] });
    }

    public void nextStep()
    {

        stepCount++;
        if(stepCount <= step_description.Count -1)
        {

            Transform textTransform = stepOverviewMenu.transform.Find("ManipulationContainer/ManipulationBar/content/Text");
            TMP_Text instructionText = textTransform.GetComponent<TMP_Text>();

            string composedText = "<size=60><b>Step " + (stepCount+1).ToString() + "</size></b>" + "<br>" +  step_description[stepCount];
            instructionText.text = composedText;
            visibilityScript.highlightObjects(new List<string> { components[stepCount] });
        }
        else
        {
        stepCount--;
        }
    }
    public void previousStep()
    {

        stepCount--;
        if(stepCount >= 0)
        {
            Transform textTransform = stepOverviewMenu.transform.Find("ManipulationContainer/ManipulationBar/content/Text");
            TMP_Text instructionText = textTransform.GetComponent<TMP_Text>();

            string composedText = "<size=60><b>Step " + (stepCount+1).ToString() + "</size></b>" + "<br>" +  step_description[stepCount];
            instructionText.text = composedText;
            visibilityScript.highlightObjects(new List<string> { components[stepCount] });
        }
        else
        {
            stepCount++;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        components = new List<string>();
        step_description = new List<string>();

        // Load JSON from file
        string filePath = Path.Combine(Application.streamingAssetsPath, OperationsFile);

        if (File.Exists(filePath))
        {
            // Read the JSON file
            string jsonString = File.ReadAllText(filePath);
            stepsData = StepsWrapper.CreateFromJSON(jsonString);
            Debug.Log(stepsData.steps.Count);

            // add object to show
            foreach(Step step in stepsData.steps)
            {
                Debug.Log(step.component_code);
                components.Add(step.component_code);
                step_description.Add(step.step_description);
            }
        }
        else
        {
            Debug.LogError("JSON file not found at " + filePath);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
