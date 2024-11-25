using UnityEngine;
using UnityEngine.SceneManagement;

public class PlaceDoorTutorial : MonoBehaviour
{
    public GameObject step3; // popup to explain what step3 is
    public GameObject placeDoorMenu; // menu to help the user place the door
    public GameObject step3Place; // first part of step3: click on the place door button
    public GameObject step3AnchorPoints; // first part of step3: click on the place door button
    public GameObject step4; // popup to explain what step4 is (how menu steps works)
    public GameObject step5; // popup to explain what step5 is (how miniature works)
    public GameObject move; // popup to make the user move closer to the scene
    public GameObject end; // end of tutorial pop up
    public GameObject challenge; // start challenge 
    public GameObject challengeEnd; // finish challenge
    //public GameObject steps; //steps
    public GameObject completed; // step2 completed

    // 0 - start configuration -> manage what should be displayed when scene is opened
    public void Start()
    {
        step3.SetActive(true);
        placeDoorMenu.SetActive(false);
        step3Place.SetActive(false);

        // GET TIME
        if(TimeTracker.Instance)
            TimeTracker.Instance.StartAction("place door tutorial|intro message");

    }

    // 1 - if step3 && user clicks 'continue' -> load place door menu && step3 place popup
    public void GoToPlace()
    {
        step3.SetActive(false);
        placeDoorMenu.SetActive(true);
        step3Place.SetActive(true);

        // GET TIME
        if(TimeTracker.Instance){
            TimeTracker.Instance.EndAction();
            TimeTracker.Instance.StartAction("place door tutorial|place door");
        }
    }

    // 2 - if step3Place && user clicks 'place door' -> load step3AnchorPoints
    public void GoToAnchorPoints()
    {
        step3Place.SetActive(false);
        step3AnchorPoints.SetActive(true);
    }

    // 4 - if door placed && user clicks 'confirm' -> go to step4
    public void GoToStep4()
    {
        step4.SetActive(true);
        completed.SetActive(false);
        //steps.SetActive(true);

        // GET TIME
        if(TimeTracker.Instance){
            TimeTracker.Instance.EndAction();
            TimeTracker.Instance.StartAction("place door tutorial|navigate steps");
        }
    }

    public void Complete()
    {
        completed.SetActive(true);
        step3AnchorPoints.SetActive(false);
    }

    public void HideComplete()
    {
        completed.SetActive(false);
    }

    // 5 - if step4 && user clicks 'continue' -> go to move popup
    public void HideStep4()
    {
        step4.SetActive(false);
    }

    public void ShowMove()
    {
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
    // 7 - if step5 && user clicks 'continue' -> hide step5
    public void GoToChallenge()
    {
        end.SetActive(false);
        challenge.SetActive(true);

        // GET TIME
        if(TimeTracker.Instance)
            TimeTracker.Instance.EndAction();
    }

    // 8 - if challengepopup && user clicks 'decline challenge' -> hide challenge pop up
    public void Decline()
    {
        challenge.SetActive(false);
    }
    
    // 9 - if challengepopup && user clicks 'accept challenge' -> hide challenge pop up & start timing
    public void Accept()
    {    
        SceneManager.LoadScene("Menu");
    }

    // 10 - if challenge finished -> show finished challenge pop up
    public void finishChallenge()
    {    
        
        // GET TIME end navigate steps timer and set challenge to false
        if(TimeTracker.Instance.challengeOn){
            challengeEnd.SetActive(true);
            Debug.Log("finished with logs");
            if(TimeTracker.Instance){
                TimeTracker.Instance.EndAction();
                TimeTracker.Instance.challengeOn = false;
            }
        }
    }
    // if exit tutorial -> start menu scene
    public void ExitTutorial()
    {
        SceneManager.LoadScene("Menu");
    }
}