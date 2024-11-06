using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
using UnityEngine.SceneManagement; // Add this line


public class instructionOverviewMenu : MonoBehaviour
{
    public GameObject stepOverviewMenu;
    private int stepCount = 0;
    public string OperationsFile;
    public cahngeObjectsVisibility visibilityScript;
    private List<string> components;
    private List<string> step_description;
    private StepsWrapper stepsData;


    private string textPath = "UIContainer_steps/StepsContent/Text";

    public void startManteinance()
    {
        Vector3 cameraPosition = Camera.main.transform.position;

        stepOverviewMenu.transform.position = cameraPosition + new Vector3(0.2f,0,0.323f);
        stepOverviewMenu.SetActive(true);
        // Transform textTransform = stepOverviewMenu.transform.Find("ManipulationContainer/ManipulationBar/content/Text");
        Transform textTransform = stepOverviewMenu.transform.Find(textPath);
        TMP_Text instructionText = textTransform.GetComponent<TMP_Text>();

        string composedText = "<size=90>Step " + (stepCount+1).ToString() + "</size>" + "<br>" +  "<size=70><b>" + step_description[0] + "</b></size>";
        instructionText.text = composedText;
        visibilityScript.highlightObjects(new HashSet<string> { components[0] });
    }

    public void nextStep()
    {

        stepCount++;
        activateDone();
        if(stepCount <= step_description.Count -1)
        {

            Transform textTransform = stepOverviewMenu.transform.Find(textPath);
            TMP_Text instructionText = textTransform.GetComponent<TMP_Text>();

            string composedText = "<size=90>Step " + (stepCount+1).ToString() + "</size>" + "<br>" +  "<size=70><b>" + step_description[stepCount] + "</b></size>";
            instructionText.text = composedText;
            visibilityScript.highlightObjects(new HashSet<string> { components[stepCount] });
        }
        else
        {
            stepCount--;
        }
    }
    public void previousStep()
    {

        stepCount--;
        activateDone();
        if(stepCount >= 0)
        {
            Transform textTransform = stepOverviewMenu.transform.Find(textPath);
            TMP_Text instructionText = textTransform.GetComponent<TMP_Text>();

            string composedText = "<size=90>Step " + (stepCount+1).ToString() + "</size>" + "<br>" +  "<size=70><b>" + step_description[stepCount] + "</b></size>";
            instructionText.text = composedText;
            visibilityScript.highlightObjects(new HashSet<string> { components[stepCount] });
        }
        else
        {
            stepCount++;
        }
    }

    private void activateDone()
    {
        if(stepCount == step_description.Count -1)
        {
            stepOverviewMenu.transform.Find("UIContainer_steps/StepsContent/Done").gameObject.SetActive(true);
            stepOverviewMenu.transform.Find("UIContainer_steps/StepsContent/NextButton").gameObject.SetActive(false);
        }else
        {
            stepOverviewMenu.transform.Find("UIContainer_steps/StepsContent/Done").gameObject.SetActive(false);
            stepOverviewMenu.transform.Find("UIContainer_steps/StepsContent/NextButton").gameObject.SetActive(true);
        }
    }

    public void Done()
    {
        SceneManager.LoadScene("Menu");

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
