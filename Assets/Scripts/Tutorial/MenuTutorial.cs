using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

// this script manages the Menu tutorial scene
public class MenuTutorial : MonoBehaviour
{
    public GameObject start; // welcome user to menu tutorial
    public GameObject step1; // teache user how to use manipulator handler
    public GameObject menuT; // tasks menu popup
    public GameObject manipulatorCheck; // handler object that has to be moved by user
    public GameObject step1Completed; // tell user step 1 was successfully completed
    public GameObject step2; // teach user how the tasks menu is structured
    public GameObject step2Row; // teach user what each row represents
    public GameObject step2Priority; // teach user what the colored square represents
    public GameObject step2Pick; // teach user to read the table and select one task (highest priority one)
    public GameObject correctPick; // if user clicks correct row 
    public GameObject wrongPick; // if user clicks wrong row
    private bool hasMoved = false; // check if user has moved the handler
    public PopUpManager PUManager; // manager for popup
    

    // start configuration - manage what should be displayed when scene is opened
    public void Start()
    {
        Assert.IsNotNull(step1);
        Assert.IsNotNull(menuT);
        Assert.IsNotNull(manipulatorCheck);
        Assert.IsNotNull(step1Completed);
        Assert.IsNotNull(step2);
        Assert.IsNotNull(step2Row);
        Assert.IsNotNull(step2Priority);
        Assert.IsNotNull(step2Pick);
        Assert.IsNotNull(correctPick);
        Assert.IsNotNull(wrongPick);

        menuT.SetActive(true);
        step2Row.SetActive(false);
        step2Pick.SetActive(false);
        step2Priority.SetActive(false);
    }

    // if start && user clicks 'continue' -> go to step 1
    public void LoadStep1()
    {
        start.SetActive(false);
        PUManager.ShowPopup(step1);
    }

    // 2 - if step1 (or step1 completed) && 'continue' -> go to step 2
    public void LoadStep2()
    {
        step1.SetActive(false);
        step1Completed.SetActive(false);
        manipulatorCheck.SetActive(false);
        PUManager.ShowPopup(menuT);
    }

    // 3 - if step1 && tried manipulator && worked -> go to step 1 completed
    public void OnManipulationStarted()
    {
        hasMoved = true;
    }

    public void OnManipulationEnded()
    {
        if (hasMoved)
        {
            Step1Completed();
        }
    }

    // 4 - tell the user step1 has been successfully completed
    public void Step1Completed()
    {
        step1.SetActive(false);
        PUManager.ShowPopup(step1Completed);
    }

    // 5 - if step2 && user clicks 'continue' -> go to step2Row
    public void GoToRow()
    {
        step2.SetActive(false);
        step2Row.SetActive(true);
    }

    // 6 - if step2Row && user clicks 'continue' -> go to step2Priority
    public void GoToPriority()
    {
        step2Row.SetActive(false);
        step2Priority.SetActive(true);

    }

    // 7 - if step2Priority && user clicks 'continue' -> go to step2Pick
    public void GoToPick()
    {
        step2Priority.SetActive(false);
        PUManager.ShowPopup(step2Pick);

    }

    // 8 - if step2Pick && user clicks 'continue' -> hide step2Pick
    public void HidePick()
    {
        step2Pick.SetActive(false);

    }

    // 9 - if correct pick -> correct pick pop up
    public void CorrectPick()
    {
        PUManager.ShowPopup(correctPick);

    }

    // 10 - if worng pick -> wrong pick pop up
    public void WrongPick()
    {
        PUManager.ShowPopup(wrongPick);

    }

    // 11 - if worng pick && Try again -> hide wrong pick pop up
    public void HideWrongPick()
    {
        wrongPick.SetActive(false);

    }

    // 12 - if correct pick && continue -> start plae door scene tutorial
    public void GoToPlaceDoorTutorial()
    {
        SceneManager.LoadScene("PlaceDoorT");
    }

    // if exit tutorial -> start menu scene
    public void ExitTutorial()
    {
        SceneManager.LoadScene("Menu");
    }
}