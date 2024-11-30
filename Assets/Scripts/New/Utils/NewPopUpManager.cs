/*
The NewPopUpManager script is responsible for dynamically creating and managing popups

Key Features:
- Customizable content: Updates header, body text, and button labels dynamically
- Centralized popup control: Ensures only one popup is active at a time

This script controls popup creation and usage across multiple scenes
*/

using System;
using UnityEngine;
using TMPro;
using MixedReality.Toolkit.UX;

public class NewPopUpManager : MonoBehaviour
{
    // Singleton instance of NewPopUpManager for global access
    public static NewPopUpManager Instance;

    [SerializeField] private GameObject popupPrefab; // popup prefab
    private GameObject currentPopup; // current active popup

    // singleton instance and makes sue the PopUpManager persists across scenes
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /* Creates and displays the pop up
       Parameters:
       - headerText: title of the popup
       - mainText: body content of the popup
       - leftButtonText: label for the left button
       - rightButtonText: label for the right button
       - leftButtonCallback: action to execute when the left button is clicked
       - rightButtonCallback: action to execute when the right button is clicked
    */
    public void ShowPopup(
        string headerText,
        string mainText,
        string leftButtonText,
        string rightButtonText,
        Action leftButtonCallback,
        Action rightButtonCallback)
    {
        // ensure only one is active
        if (currentPopup != null)
        {
            Destroy(currentPopup);
        }

        // instantiate the popup prefab
        currentPopup = Instantiate(popupPrefab, transform);

        // fill title
        var header = currentPopup.transform.Find("Canvas/Header").GetComponent<TextMeshProUGUI>();
        if (header != null)
        {
            header.text = headerText;
        }
        else
        {
            Debug.LogError("Header not found");
        }

        // fill body
        var mainTextComponent = currentPopup.transform.Find("Canvas/Main Text").GetComponent<TextMeshProUGUI>();
        if (mainTextComponent != null)
        {
            mainTextComponent.text = mainText;
        }
        else
        {
            Debug.LogError("Main Text not foun");
        }

        // fill left button
        var leftButton = currentPopup.transform.Find("Canvas/Horizontal/Left/Frontplate/AnimatedContent/Text")
            .GetComponent<TextMeshProUGUI>();
        if (leftButton != null)
        {
            leftButton.text = leftButtonText;

            // action
            var leftButtonObject = currentPopup.transform.Find("Canvas/Horizontal/Left").gameObject;
            var leftButtonComponent = leftButtonObject.GetComponent<PressableButton>();
            if (leftButtonComponent != null)
            {
                leftButtonComponent.OnClicked.RemoveAllListeners(); 
                leftButtonComponent.OnClicked.AddListener(() =>
                {
                    leftButtonCallback?.Invoke();
                    ClosePopup();
                });
            }
            else
            {
                Debug.LogError("Left button component not found!");
            }
        }
        else
        {
            Debug.LogError("Left Button not found");
        }

        // fill right button
        var rightButton = currentPopup.transform.Find("Canvas/Horizontal/Right/Frontplate/AnimatedContent/Text")
            .GetComponent<TextMeshProUGUI>();
        if (rightButton != null)
        {
            rightButton.text = rightButtonText;

            // action
            var rightButtonObject = currentPopup.transform.Find("Canvas/Horizontal/Right").gameObject;
            var rightButtonComponent = rightButtonObject.GetComponent<PressableButton>();
            if (rightButtonComponent != null)
            {
                rightButtonComponent.OnClicked.RemoveAllListeners();
                rightButtonComponent.OnClicked.AddListener(() =>
                {
                    rightButtonCallback?.Invoke();
                    ClosePopup();
                });
            }
            else
            {
                Debug.LogError("Right button component not found");
            }
        }
        else
        {
            Debug.LogError("Right Button not found");
        }
    }

    // Enures no popup stays active after the user interaction is complete
    public void ClosePopup()
    {
        if (currentPopup != null)
        {
            Destroy(currentPopup);
            currentPopup = null;
        }
    }
}
