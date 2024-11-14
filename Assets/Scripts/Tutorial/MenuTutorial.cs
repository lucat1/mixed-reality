using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuTutorial : MonoBehaviour
{
    public GameObject StartPopUp;
    public GameObject ManipulatorPopUp;
    public GameObject MenuT;
    public GameObject ManipulatorCheck;

    public void Start()
    {
        ManipulatorPopUp.SetActive(false);
        MenuT.SetActive(false);
        ManipulatorCheck.SetActive(false);
        StartPopUp.SetActive(true);
    }
    public void LoadManipulatorPopUp()
    {
        StartPopUp.SetActive(false);
        ManipulatorPopUp.SetActive(true);
        ManipulatorCheck.SetActive(true);

    }

    public void LoadMenu()
    {
        ManipulatorPopUp.SetActive(false);
        ManipulatorCheck.SetActive(false);
        MenuT.SetActive(true);
    }

    public void ExitTutorial()
    {
        SceneManager.LoadScene("Menu");
    }



}