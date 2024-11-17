using UnityEngine;
using UnityEngine.SceneManagement;

public class PlaceDoorTutorial : MonoBehaviour
{
    public GameObject step3; // popup to explain what step3 is
    public GameObject placeDoorMenu; // menu to help the user place the door
    public GameObject step3Place; // first part of step3: click on the place door button

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

    // if exit tutorial -> start menu scene
    public void ExitTutorial()
    {
        SceneManager.LoadScene("Menu");
    }
}