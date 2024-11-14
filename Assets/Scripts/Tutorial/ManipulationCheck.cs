using UnityEngine;
using UnityEngine.SceneManagement;

public class ManipulationCheck : MonoBehaviour
{
    public GameObject ManipulatorPopUp;
    public GameObject MenuT;
    public GameObject ManipulatorCheck;
    private bool hasMoved = false;
    public void OnManipulationStarted()
    {
        hasMoved = true;
    }

    public void OnManipulationEnded()
    {
        if (hasMoved)
        {
            LoadMenu();
        }
    }

    private void LoadMenu()
    {
        ManipulatorPopUp.SetActive(false);
        ManipulatorCheck.SetActive(false);
        MenuT.SetActive(true);
    }
}