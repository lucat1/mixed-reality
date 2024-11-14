using UnityEngine;
using UnityEngine.SceneManagement;

public class WelcomeScene : MonoBehaviour
{
    // if 'Yes, start Tutorial'
    public void GoToTutorial()
    {
        //SceneManager.LoadScene("TutorialScene");
    }

    // if 'No, Go to Menu'
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}