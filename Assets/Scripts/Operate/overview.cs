using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class overview : MonoBehaviour
{
    public string OperationsFile;
    public cahngeObjectsVisibility visibilityScript;
    public RenderDoor renderDoor;
    public GameObject placeDoorMenu;
    public GameObject Door;
    private StepsWrapper stepsData;
    private List<string> components;



    public void startOverviewMode()
    {
        // disable place deoor menu 
        placeDoorMenu.SetActive(false);
        visibilityScript.highlightObjects(components);
        renderDoor.cancelAnchorPoints();
        
    }

    // private void pointSmall()
    // {
    //     Renderer[] rendererComponents = Door.GetComponentsInChildren<Renderer>();
    //     float doorWidth = Door.GetComponent<SpriteRenderer>().bounds.size.x;
    //     Debug.Log(doorWidth);

    //     foreach(Renderer component in rendererComponents)
    //     {
    //         // get components to modify
    //         if(components.Contains(component.name))
    //         {
    //             // check size w.r.t. the door
    //             float DoorComponent = 

    //         }
    //     }
    // }
    // Start is called before the first frame update
    void Start()
    {
        components = new List<string>();
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
