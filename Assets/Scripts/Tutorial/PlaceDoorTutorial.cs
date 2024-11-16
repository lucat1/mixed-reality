using UnityEngine;
using UnityEngine.SceneManagement;

public class PlaceDoorTutorial : MonoBehaviour
{
    public GameObject StartPopUp;
    public GameObject PlaceDoor;
    public GameObject PlaceButtonPopUp;

    // 0 - start configuration
    public void Start()
    {
        StartPopUp.SetActive(true);
        PlaceDoor.SetActive(false);
        PlaceButtonPopUp.SetActive(false);
    }

    // 1 - if continue -> load scene and place button popup
    public void FistPopUp()
    {
        StartPopUp.SetActive(false);
        PlaceDoor.SetActive(true);
        PlaceButtonPopUp.SetActive(true);
    }

    // 2 - if button 'place door' is clicked -> open door guide popup
    public void DoorGuidePopUp()
    {
        PlaceButtonPopUp.SetActive(false);
    }




    // if exit tutorial -> start menu scene
    public void ExitTutorial()
    {
        SceneManager.LoadScene("Menu");
    }
}