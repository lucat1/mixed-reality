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
    public bool ring;
}

public class StepsManager : MonoBehaviour
{
    // A JSON file containing all the steps required to perform the current maintenance.
    public TextAsset stepsFile;
    public PlacementManager placeDoor;
    private JSONSteps steps;
    private DoorManager dm;

    int currentStepIndex;
    public void Reset() {
        // reset step
        Debug.Log("[StepsManager] Resetting to first step");
        currentStepIndex = 0;
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

    void DisplayStep() {
        var textTransform = transform.Find(textPath);
        var instructionText = textTransform.GetComponent<TMP_Text>();

        var step = CurrentStep();
        string composedText = "<size=90>Step " + (currentStepIndex+1).ToString() + "</size>" + "<br>" +  "<size=70><b>" + step.step_description + "</b></size>";
        instructionText.text = composedText;
        dm.HighlightComponents(new () { step.component_code }, step.ring);

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
        NewSceneManager.Instance.GoTo(new List<string> { "MenuSceneCanvas", "MenuPanel" });

        // if the challenge is not acative we go to main menu
        // otherwise the user can come back to main menu from challenge completed popoup
        // TODO: log!!
        if(TimeTracker.Instance){
            if(!TimeTracker.Instance.challengeOn)
                Debug.LogError("DO LOGGING!!");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Check if tutorial
        if (NewSceneManager.Instance.TutorialActive)
        {
            BuildStep3PopUp();
        }
        else
        {
            BuildSteps();
        }
        
    }

    void OnEnable() {
        Reset();
        // Prevent DisplayStep from being called when the elemet has not been initialized by Start() yet
        if (steps != null)
            DisplayStep();
    }

    private void BuildStep3PopUp(){
        NewSceneManager.Instance.HideObject("BigDoor");
        NewSceneManager.Instance.HideObject("StepsContent");
        NewSceneManager.Instance.HideObject("StepsManipulationContainer");
        NewPopUpManager.Instance.ShowBigPopUp(
            "Step 3: Navigate Steps",
            "In this step, you will need to align the holographic door with the real train door. \n First move the blue ball to match the bottom-right corner of the door. \nThen move the Red Ball to match the top left-corner of the door. \n When the door is correctly placed click \"Confirm\".",
             "Continue", 
            () =>
            {
            NewSceneManager.Instance.ShowObject("BigDoor");
            NewSceneManager.Instance.ShowObject("StepsContent");
            NewSceneManager.Instance.ShowObject("StepsManipulationContainer");
            BuildSteps();

            }
            );
    }

    private void BuildSteps(){
        steps = JsonUtility.FromJson<JSONSteps>(stepsFile.ToString());
        dm = transform.parent.GetComponentInChildren<DoorManager>();
        Assert.IsNotNull(dm);
        Reset();
        DisplayStep();
    }
}
