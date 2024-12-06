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
    private MiniatureManager mm;


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

        // change text on the Steps panel
        var textTransform = transform.Find(textPath);
        var instructionText = textTransform.GetComponent<TMP_Text>();

        var step = CurrentStep();
        string composedText = "<size=90>Step " + (currentStepIndex+1).ToString() + "</size>" + "<br>" +  "<size=70><b>" + step.step_description + "</b></size>";
        instructionText.text = composedText;

        // higlite element in hte Big door and select display block in the miniature
        dm.HighlightComponents(new () { step.component_code }, step.ring);
        mm.ActivateDisplayBlock(currentStepIndex);

        // Enable/Disable finish/next button
        transform.Find(donePath).gameObject.SetActive(IsLastStep());
        transform.Find(nextPath).gameObject.SetActive(!IsLastStep());

        // disable previous button
        var pb = transform.Find(prevPath).gameObject.GetComponent<PressableButton>();
        pb.enabled = currentStepIndex != 0;
    }

    public void NextStep() {
        currentStepIndex++;
        if (NewSceneManager.Instance.TutorialActive && currentStepIndex==1)
        {
            BuildStep4PopUp();
        }
        else
        {
            DisplayStep();
        }

    }

    public void PrevStep() {
        currentStepIndex--;
        DisplayStep();
    }

    public void Done() {
        NewSceneManager.Instance.GoTo(new List<string> { "MenuSceneCanvas", "MenuPanel", "PalmMiniature" });

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
            "Step 3: Navigate Maintenance Steps",
            "The next step is about navigating through the steps of the maintenance process. \n Fo each step, you will see: \n- the instruction displayed in the menu \n - the involved component is highlighted on the door hologram  \n Use the buttons to explore the process and navigate to step 2 for more details about the tutorial. \n Click \"Continue\" to proceed.",
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

    private void BuildStep4PopUp(){
        NewPopUpManager.Instance.ShowSinglePopup(
            "Step 4: Use the Miniature View",
            "This component is smaller and harder to see.This is where the miniature view becomes handy. \nSimply open your palm to view the component up close! ",
             "Continue", 
            () =>
            {
            DisplayStep();

            }
            );
    }

    private void BuildSteps(){
        steps = JsonUtility.FromJson<JSONSteps>(stepsFile.ToString());
        


        dm = transform.parent.GetComponentInChildren<DoorManager>();
        Assert.IsNotNull(dm);
        mm = transform.parent.GetComponentInChildren<MiniatureManager>();
        Assert.IsNotNull(mm);
        Reset();
        mm.InitializeDisplayBlocks(getComponentsList());
        DisplayStep();
    }

    private List<string> getComponentsList(){
        List<string> components = new ();

        if (steps != null && steps.steps != null)
            foreach (JSONStep step in steps.steps)
                if (!string.IsNullOrEmpty(step.component_code))
                    components.Add(step.component_code);

        return components;
    }
}
