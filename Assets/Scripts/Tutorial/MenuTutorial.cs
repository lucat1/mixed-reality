using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuTutorial : MonoBehaviour
{
    public GameObject StartPopUp;
    public GameObject ManipulatorPopUp;
    public GameObject MenuT;
    public GameObject ManipulatorCheck;
    public GameObject Step1;

    public GameObject Step2Overview;
    public GameObject Step2Row;
    public GameObject Step2Priority;
    public GameObject Step2Pick;

    public GameObject CorrectP;
    public GameObject WrongP;

    // start configuration
    public void Start()
    {
        ManipulatorPopUp.SetActive(false);
        MenuT.SetActive(false);
        ManipulatorCheck.SetActive(false);
        StartPopUp.SetActive(true);
        Step1.SetActive(false);
        Step2Overview.SetActive(false);
        Step2Row.SetActive(false);
        Step2Priority.SetActive(false);
        Step2Pick.SetActive(false);
    }

    // 1 - if tutorial -> step 1 try manipulator
    public void LoadManipulatorPopUp()
    {
        StartPopUp.SetActive(false);
        ManipulatorPopUp.SetActive(true);
        ManipulatorCheck.SetActive(true);

    }

    // 2 - if skip step 1 -> go to step 2
    public void LoadMenu()
    {
        ManipulatorPopUp.SetActive(false);
        ManipulatorCheck.SetActive(false);
        Step2Overview.SetActive(true);
        MenuT.SetActive(true);
    }

    // 3 - if tried manipulator and worked -> in ManipulationCheck.cs
    // 4 - after 3 a popup is open if continue -> go to step2
    public void Step1Completed()
    {
        Step1.SetActive(false);
        MenuT.SetActive(true);
        Step2Overview.SetActive(true);

    }

    // 5 - if step2 overview continue -> row popup
    public void GoToRow()
    {
        Step2Overview.SetActive(false);
        Step2Row.SetActive(true);

    }

    // 6 - if step2 row continue -> priority popup
    public void GoToPriority()
    {
        Step2Row.SetActive(false);
        Step2Priority.SetActive(true);

    }

    // 7 - if step2 priority continue -> pick popup
    public void GoToPick()
    {
        Step2Priority.SetActive(false);
        Step2Pick.SetActive(true);

    }

    // 8 - if step2 priority continue -> hide popup
    public void HidePick()
    {
        Step2Pick.SetActive(false);

    }

    // 9 - if correct pick -> correct pick pop up
    public void CorrectPick()
    {
        CorrectP.SetActive(true);

    }

    // 10 - if worng pick -> wrong pick pop up
    public void WrongPick()
    {
        WrongP.SetActive(true);

    }

    // 11 - if worng pick && Try again -> hide wrong pick pop up
    public void HideWrongPick()
    {
        WrongP.SetActive(false);

    }

    // 12 - if correct pick && continue -> start plae door scene tutorial
    public void GoToPlaceDoorT()
    {
        SceneManager.LoadScene("PlaceDoorTutorial");
    }

    // if exit tutorial -> start menu scene
    public void ExitTutorial()
    {
        SceneManager.LoadScene("Menu");
    }



}