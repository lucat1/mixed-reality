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

        // fill user inputs (for semplicity)
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

        // map login button
        loginButton = NewSceneManager.Instance.Search(gameObject,"LoginButton").GetComponent<PressableButton>();
        if (loginButton != null)
        {
            loginButton.OnClicked.RemoveAllListeners();
            loginButton.OnClicked.AddListener(() =>
                {
                    LoginCheck();
                });

        }
        else
        {
            Debug.LogError("LoginButton not found!");
        }
    }

    // checks the credentials -> transitions to the tutorial popup if correct
    public void LoginCheck()
    {
        // get input values (already filled)
        string enteredUsername = usernameInput.text;
        string enteredPassword = passwordInput.text;

        // check credentials
        if (enteredUsername == correctUsername && enteredPassword == correctPassword)
        {
            Debug.Log("Login successful");
            NewSceneManager.Instance.HideObject("LoginPanel");
            NewPopUpManager.Instance.ShowPopup(
            "Start Tutorial",
            "Do you want to start the tutorial?",
            "No", 
            "Yessssssss", 
            () =>
            {
                Debug.Log("Tutorial started");
            },
            () =>
            {
                Debug.Log("Go to Menu");
            }
        );
        }
        else
        {
            Debug.LogWarning("Login failed");
        }
    }
}
