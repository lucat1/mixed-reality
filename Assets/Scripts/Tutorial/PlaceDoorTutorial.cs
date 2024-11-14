using UnityEngine;
using UnityEngine.SceneManagement;

public class PlaceDoorTutorial : MonoBehaviour
{
    public GameObject StartPopUp;

    // start configuration
    public void Start()
    {
        StartPopUp.SetActive(true);
    }





    // if exit tutorial -> start menu scene
    public void ExitTutorial()
    {
        SceneManager.LoadScene("Menu");
    }
}