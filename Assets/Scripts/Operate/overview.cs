using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;

public class overview : MonoBehaviour
{
    public string OperationsFile;
    public cahngeObjectsVisibility visibilityScript;
    public RenderDoor renderDoor;
    public GameObject placeDoorMenu;
    public GameObject Door;
    public GameObject overviewMenu;
    private StepsWrapper stepsData;
    private List<string> components;
    private List<string> step_description;



    public void startOverviewMode()
    {
        // disable place deoor menu 
        placeDoorMenu.SetActive(false);
        visibilityScript.highlightObjects(components);
        renderDoor.cancelAnchorPoints();
        createOverviewMenu();
        
    }

    void createOverviewMenu()
    {
        Vector3 cameraPosition = Camera.main.transform.position;

        overviewMenu = Instantiate(overviewMenu, cameraPosition + new Vector3(0,0,0.3f) , Quaternion.identity);
        Transform textTransform = overviewMenu.transform.Find("ManipulationContainer/ManipulationBar/content/Text");
        TMP_Text instructionText = textTransform.GetComponent<TMP_Text>();
        string composedText = "";
        int i = 0;
        foreach(string step in step_description)
        {
            composedText += "step: " + i + ")  " + step + "\n" + "<br>";
            i ++;
        }
        instructionText.text = composedText;
    }
    void Start()
    {
        components = new List<string>();
        step_description = new List<string>();

        // Load JSON from file
        string filePath = Path.Combine(Application.dataPath, OperationsFile);

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
