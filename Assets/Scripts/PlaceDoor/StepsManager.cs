using System;
using System.Collections.Generic;
using MixedReality.Toolkit.UX;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using MixedReality.Toolkit.SpatialManipulation;

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
    public DoorManager doorManager;
    public PlacementManager placeDoor;
    public MiniatureManager miniatureManager;
    public GameObject challengeEnd; // finish challenge
    JSONSteps steps;

    int currentStepIndex;
    public void Reset() {
        // reset step
        currentStepIndex = 0;
    }

    public void Show() {
        // Show the steps menu
        gameObject.SetActive(true);

        Reset();
        miniatureManager.Reset();
        DisplayStep();
    }

    public void Hide() {
        gameObject.SetActive(false);
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
        doorManager.HighlightComponents(new () { step.component_code }, step.ring);

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
        // if the challenge is not acative we go to main menu
        // otherwise the user can come back to main menu from challenge completed popoup
        if(TimeTracker.Instance){
            if(!TimeTracker.Instance.challengeOn)
                SceneManager.LoadScene("Menu");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        steps = JsonUtility.FromJson<JSONSteps>(stepsFile.ToString());
        Hide();
    }
}
