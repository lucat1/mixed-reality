/*
The LoginManager script is responsible for handling the login.

Key Features:
- Fills the username and password fields with correct credentials when the login panel is activated
- Handles the login process: validates credentials and transitions to the tutorial popup if correct

*/

using UnityEngine;
using UnityEngine.UI;
using MixedReality.Toolkit.UX;
using UnityEngine.Events;


public class LoginManager : MonoBehaviour
{
    // Correct credentials (hardcoded for now)
    private string correctUsername = "SBBuser";
    private string correctPassword = "SBBpassword";

    private MRTKTMPInputField usernameInput; // username input field
    private MRTKTMPInputField passwordInput; // password inpu field
    private PressableButton loginButton; // button for login 

    private void Start()
    {
        print("SI INIZIA START LOGIN");
        GameObject usernameInputObject = NewSceneManager.Instance.Search(gameObject, "UsernameInput");
        GameObject passwordInputObject = NewSceneManager.Instance.Search(gameObject, "PasswordInput");

        if (usernameInputObject != null)
        {
            usernameInput = usernameInputObject.GetComponentInChildren<MRTKTMPInputField>();
            usernameInput.text = correctUsername;
        }
        else
        {
            Debug.LogError("UsernameInput not found in hierarchy!");
        }

        if (passwordInputObject != null)
        {
            passwordInput = passwordInputObject.GetComponentInChildren<MRTKTMPInputField>();
            passwordInput.text = correctPassword;
        }
        else
        {
            Debug.LogError("PasswordInput not found in hierarchy!");
        }

        // HELP: target viene null ma dovrebbe esere l'oggetto a cui e legato lo script
        LoginManager loginManager = this;
        print(loginManager);

        loginButton = NewSceneManager.Instance.Search(gameObject,"LoginButton").GetComponent<PressableButton>();

        // Map the LoginCheck method to the login button's click event
        if (loginButton != null)
        {
            loginButton.OnClicked.AddListener(LoginCheck);
            Debug.Log($"Number of listeners on OnClicked: {loginButton.OnClicked.GetPersistentEventCount()}");
            for (int i = 0; i < loginButton.OnClicked.GetPersistentEventCount(); i++)
                {
                    var target = loginButton.OnClicked.GetPersistentTarget(i);
                    var method = loginButton.OnClicked.GetPersistentMethodName(i);

                    Debug.Log($"Listener {i}: Target = {target}, Method = {method}");
                }

        }
        else
        {
            Debug.LogError("LoginButton not found!");
        }
    }

    /// Checks the credentials -> transitions to the tutorial popup if correct
    public void LoginCheck()
    {
        // Get input values (already filled)
        string enteredUsername = usernameInput.text;
        string enteredPassword = passwordInput.text;

        // Check credentials
        if (enteredUsername == correctUsername && enteredPassword == correctPassword)
        {
            Debug.Log("Login successful");
            NewSceneManager.Instance.HideObject("LoginPanel");
            NewSceneManager.Instance.ShowObject("StartTutorialPopUp");
        }
        else
        {
            Debug.LogWarning("Login failed");
        }
    }
}
