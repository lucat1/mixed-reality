using System;
using System.Collections.Generic;
using MixedReality.Toolkit.UX;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

[Serializable]
class JSONSteps {
    public string name;
    public List<JSONStep> steps;
}

[Serializable]
class JSONStep {
    public int stepN;
    public string step_description;
    public string component_code;
}

public class Steps : MonoBehaviour
{
    // A JSON file containing all the steps required to perform the current maintenance.
    public TextAsset stepsFile;
    public DoorManager doorManager;
    public PlaceDoor placeDoor;
    public GameObject manager;
    public MiniatureManager miniatureManager;
    public GameObject challengeEnd; // finish challenge

    JSONSteps steps;

    int currentStepIndex;

    public void StartMaintenance() {
        transform.gameObject.SetActive(true);
        miniatureManager.InitializeDisplayBlocks();
        transform.SetParent(transform);
        var cameraPosition = Camera.main.transform.position;
        transform.position = cameraPosition + new Vector3(0.2f,0,0.323f);

        currentStepIndex = 0;
        DisplayStep();
        manager.SetActive(true);
    }

    bool IsLastStep() {
        return currentStepIndex >= steps.steps.Count - 1;
    }

    JSONStep CurrentStep() {
        Assert.IsTrue(currentStepIndex < steps.steps.Count);
        Assert.IsTrue(currentStepIndex >= 0);
    
        return steps.steps[currentStepIndex];
    }

    private const string textPath = "StepsContent/Text";
    private const string donePath = "StepsContent/Done";
    private const string prevPath = "StepsContent/PreviousButton";
    private const string nextPath = "StepsContent/NextButton";
    private const string changeDoorPositionPath = "StepsContent/ChangeDoorPosition";
    void DisplayStep() {
        var textTransform = transform.Find(textPath);
        var instructionText = textTransform.GetComponent<TMP_Text>();

        var step = CurrentStep();
        string composedText = "<size=90>Step " + (currentStepIndex+1).ToString() + "</size>" + "<br>" +  "<size=70><b>" + step.step_description + "</b></size>";
        instructionText.text = composedText;
        doorManager.HighlightComponents(new () { step.component_code });

        // Enable/Disable finish/next button
        transform.Find(donePath).gameObject.SetActive(IsLastStep());
        transform.Find(nextPath).gameObject.SetActive(!IsLastStep());

        // disable previous button
        var pb = transform.Find(prevPath).gameObject.GetComponent<PressableButton>();
        pb.enabled = currentStepIndex != 0;
    }

    public void NextStep() {
        currentStepIndex++;
        DisplayStep();
    }

    public void PrevStep() {
        currentStepIndex--;
        DisplayStep();
    }

    public void Done() {
        challengeEnd.SetActive(true);
        
    }

    public void ChangeDoorPosition(){
        // deactivate door and miniature
        doorManager.gameObject.SetActive(false);
        miniatureManager.gameObject.SetActive(false);
        // toggle place door menu
        placeDoor.TogglePlaceDoorMenu();
        // reset step
        currentStepIndex = 0;
        // deactivate step menu
        gameObject.SetActive(false);
        // disable manager that switches between miniature and door
        manager.SetActive(false);
    
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    void OnEnable()
    {
        steps = JsonUtility.FromJson<JSONSteps>(stepsFile.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
