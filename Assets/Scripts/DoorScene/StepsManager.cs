/*
The StepsManager script is responsible for managing the steps menu.

Key Features:
- Dynamically builds the steps based on tasks from a JSON file.
- Allows the user to navigate through maintenance steps.
*/
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
    public bool Step4PopupShown { get; private set; } = false; // flag to check if popup 4 has been shown
    int currentStepIndex;

    // reset to first step
    public void Reset() {
        currentStepIndex = 0;
    }

    // check if last step
    bool IsLastStep() {
        return currentStepIndex >= steps.steps.Count - 1;
    }

    // check if current step
    JSONStep CurrentStep() {
        Assert.IsTrue(currentStepIndex < steps.steps.Count);
        Assert.IsTrue(currentStepIndex >= 0);
    
        return steps.steps[currentStepIndex];
    }

    private const string textPath = "StepsContent/Text";
    private const string donePath = "StepsContent/Done";
    private const string prevPath = "StepsContent/PreviousButton";
    private const string nextPath = "StepsContent/NextButton";

    // show step
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

    // go to next step
    public void NextStep() {
        currentStepIndex++;
        if (NewSceneManager.Instance.TutorialActive && currentStepIndex==1 && Step4PopupShown==false)
        {
            BuildStep4TutorialPopUp();
        }
        else
        {
            DisplayStep();
        }
    }

    // go to previous step
    public void PrevStep() {
        currentStepIndex--;
        DisplayStep();
    }

    // Go back and change the door position
    public void RePlaceDoor() {
        NewSceneManager.Instance.GoTo("Placement", new List<string> { "PlacementPanel", "PlaceDoor" });
    }

    // if finished all steps -> button done
    public void Done() {
        if (NewSceneManager.Instance.TutorialActive){
             NewPopUpManager.Instance.ShowBigPopUp(
            "CannotFinishTutorialWithoutMiniature",
            "Watch Out!",
            "You tried to click \"Done\" without having followed the last step of the tutorial. \nOpen the palm of your hand and visualize the miniature to complete the tutorial!  \n Click 'Try Again' and proceed with the correct order of the tutorial.",
             "Try Again", 
            () =>
            {
             Debug.Log("User tried to finish steps before finisnih the tutorial!");

            }
            );
        }
        else if (NewSceneManager.Instance.ChallengeActive) // if challenge && done -> challege completed!
        {
            NewPopUpManager.Instance.ShowSinglePopup(
            "FinishChallenge",
            "Congratulations!",
            "You finished the challenge! Good job! Now you're ready to use SBB HoloGuide on your own.",
            "Finish",
            () =>
            {
                NewSceneManager.Instance.EndChallenge();
                NewSceneManager.Instance.GoTo("Menu", new List<string> { "MenuPanel", "PalmMiniature" });
            }
        );

        }
        else // normal execution
        {
            NewSceneManager.Instance.GoTo("Menu", new List<string> { "MenuPanel", "PalmMiniature" });
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Check if tutorial
        if (NewSceneManager.Instance.TutorialActive)
        {
            BuildStep3TutorialPopUp();
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

    // step 3 tutorial popup
    private void BuildStep3TutorialPopUp(){
        NewSceneManager.Instance.HideObject("BigDoor");
        NewSceneManager.Instance.HideObject("StepsContent");
        NewSceneManager.Instance.HideObject("StepsManipulationContainer");
        NewPopUpManager.Instance.ShowBigPopUp(
            "NavigateSteps",
            "Step 3: Navigate Maintenance Steps",
            "The next step is about navigating through the steps of the maintenance process. \n Fo each step, you will see: \n- the instruction displayed in the menu \n - the involved component is highlighted on the door hologram  \n Use the buttons to explore the process and navigate to step 2 for more details about the tutorial. \n Click \"Continue\" to proceed.",
             "Continue", 
            () =>
            {
            NewSceneManager.Instance.ShowObjects(new (){"BigDoor", "StepsContent", "StepsManipulationContainer"});
            BuildSteps();

            }
            );
            
    }

    // step 4 tutorial popup
    private void BuildStep4TutorialPopUp(){
        NewPopUpManager.Instance.ShowSinglePopup(
            "UseMiniature",
            "Step 4: Use the Miniature View",
            "This component is smaller and harder to see.This is where the miniature view becomes handy. \nSimply open your palm to view the component up close! ",
             "Continue", 
            () =>
            {
            DisplayStep();

            }
            );
        Step4PopupShown=true;
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
