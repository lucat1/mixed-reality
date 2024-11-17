using UnityEngine;
using UnityEngine.SceneManagement;

public class PlaceDoorTutorial : MonoBehaviour
{
    public GameObject step3; // popup to explain what step3 is
    public GameObject placeDoorMenu; // menu to help the user place the door
    public GameObject step3Place; // first part of step3: click on the place door button
    public GameObject step3AnchorPoints; // second part of step3: guide user to place door
    public GameObject blueBall; // blue ball for the door rendering
    public GameObject step4; // popup to explain what step4 is (how menu steps works)
    public GameObject step5; // popup to explain what step5 is (how miniature works)
    public GameObject move; // popup to make the user move closer to the scene
    public GameObject end; // end of tutorial pop up

    // 0 - start configuration -> manage what should be displayed when scene is opened
    public void Start()
    {
        step3.SetActive(true);
        placeDoorMenu.SetActive(false);
        step3Place.SetActive(false);
    }

    // 1 - if step3 && user clicks 'continue' -> load place door menu && step3 place popup
    public void GoToPlace()
    {
        step3.SetActive(false);
        placeDoorMenu.SetActive(true);
        step3Place.SetActive(true);
    }

    // 2 - if step3Place && user clicks 'place door' -> load step3AnchorPoints
    public void GoToAnchorPoints()
    {
        step3Place.SetActive(false);
    }

    // 3 - if user starts to move the blue ball -> remove step3AnchorPoints
    public void OnManipulationStarted()
    {
        step3AnchorPoints.SetActive(false);
    }

    // 4 - if door placed && user clicks 'confirm' -> go to step4
    public void GoToStep4()
    {
        step4.SetActive(true);
    }

    // 5 - if step4 && user clicks 'continue' -> go to move popup
    public void HideStep4()
    {
        step4.SetActive(false);
        move.SetActive(true);
    }

    // 6 - if movepopup && user clicks 'continue' -> hide move pop up
    public void HideMove()
    {
        move.SetActive(false);
    }

    // 7 - if step5 && user clicks 'continue' -> hide step5
    public void HideStep5()
    {
        step5.SetActive(false);
        end.SetActive(true);
    }

    // if exit tutorial -> start menu scene
    public void ExitTutorial()
    {
        SceneManager.LoadScene("Menu");
    }
}