using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class WelcomeScene : MonoBehaviour
{
    
    public GameObject loginPopUp; // enable the user to log in
    public GameObject welcomePopUp; // once the user logs in welcome him to the app
    public GameObject errorPopup; // if the user inserted wrong username/password
    // NB: the only credentials that works are u:=sbbuser, pwd:=sbbpassword
    public TMP_InputField usernameInput; // username input field
    public TMP_InputField passwordInput; // password input field
    private string correctUsername = "Sbbuser"; // hardcoded test username
    private string correctPassword = "Sbbpassword"; // hardcoded test password
    
    // 0 - start configuration -> manage what should be displayed when scene is opened
    public void Start()
    {
        loginPopUp.SetActive(true);
        errorPopup.SetActive(false);
        welcomePopUp.SetActive(false);

        // GET TIME
        TimeTracker.Instance.StartAction("login|login the user");

    }

    // 1 - check if correct credentials when login button is clicked
    public void LoginCheck()
    {
        // get the input values
        string enteredUsername = usernameInput.text;
        string enteredPassword = passwordInput.text;

        // check credentials
        if (enteredUsername == correctUsername && enteredPassword == correctPassword)
        {
            Debug.Log("Login successful!");
            // if login successful -> welcome user to the app
            loginPopUp.SetActive(false);
            welcomePopUp.SetActive(true);

            // GET TIME
            TimeTracker.Instance.EndAction();
        }
        else
        {
            Debug.Log("Login failed. Invalid username or password.");
            // if login failed -> open error popup
            errorPopup.SetActive(true);
        }
    }

    // 2 - if login failed && user clicks 'try again' on error pop up -> back to LoginPopUp 
    public void TryAgain()
    {
        loginPopUp.SetActive(true);
        errorPopup.SetActive(false);
        usernameInput.text = "";
        passwordInput.text = "";
    }
    
    // if WelcomePopUp %% user clicks 'Yes, go to the tutorial' -> load tutorial scene for menu tasks
    public void GoToTutorial()
    {
        SceneManager.LoadScene("MenuTutorial");
    }

    // if WelcomePopUp %% user clicks 'No, go to the menu' -> load real scene for menu tasks
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}