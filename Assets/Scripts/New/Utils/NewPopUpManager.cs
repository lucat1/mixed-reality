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
    public static NewPopUpManager Instance; // singleton instance of NewPopUpManager for global access
    [SerializeField] private GameObject singleButtonBrefab; // popup prefab with one button
    [SerializeField] private GameObject doubleButtonPrefab; // popup prefab with 2 buttons
    private GameObject currentPopup; // current active popup
    public bool IsPopupActive => currentPopup != null; // flag for current active popup

    // singleton instance and makes sure the PopUpManager persists across scenes
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

    /* Creates and displays the double pop up (aka popup with two buttons)
       Parameters:
       - headerText: title of the popup
       - mainText: body content of the popup
       - leftButtonText: label for the left button
       - rightButtonText: label for the right button
       - leftButtonCallback: action to execute when the left button is clicked
       - rightButtonCallback: action to execute when the right button is clicked
    */
    public void ShowDoublePopup(
        string popupId,
        string headerText,
        string mainText,
        string leftButtonText,
        string rightButtonText,
        Action leftButtonCallback,
        Action rightButtonCallback)
    {
        Timer.Instance.Action($"Popup[{popupId}]");
        // only one is active
        if (currentPopup != null)
        {
            Destroy(currentPopup);
        }

        // instantiate the popup prefab
        currentPopup = Instantiate(doubleButtonPrefab, transform);

        // fill title
        var header = currentPopup.transform.Find("Canvas/Header").GetComponent<TextMeshProUGUI>();
        if (header != null)
        {
            header.text = headerText;
        }
        else
        {
            Debug.LogError("[PopUpManager] Header not found");
        }

        // fill body
        var mainTextComponent = currentPopup.transform.Find("Canvas/Main Text").GetComponent<TextMeshProUGUI>();
        if (mainTextComponent != null)
        {
            mainTextComponent.text = mainText;
        }
        else
        {
            Debug.LogError("[PopUpManager] Main Text not foun");
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
                Debug.LogError("[PopUpManager] Left button component not found!");
            }
        }
        else
        {
            Debug.LogError("[PopUpManager] Left Button not found");
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
                Debug.LogError("[PopUpManager] Right button component not found");
            }
        }
        else
        {
            Debug.LogError("[PopUpManager] Right Button not found");
        }
    }

    /* Creates and displays the single pop up (aka single button)
       Parameters:
       - headerText: title of the popup
       - mainText: body content of the popup
       - leftButtonText: label for the left button
       - rightButtonText: label for the right button
       - leftButtonCallback: action to execute when the left button is clicked
       - rightButtonCallback: action to execute when the right button is clicked
    */
    public void ShowSinglePopup(
        string popupId,
        string headerText,
        string mainText,
        string buttonText,
        Action buttonCallback)
    {
        Timer.Instance.Action($"Popup[{popupId}]");
        // only one is active
        if (currentPopup != null)
        {
            ClosePopup();
        }

        print("BLA");

        // instantiate the popup prefab
        currentPopup = Instantiate(singleButtonBrefab, transform);

        // fill title
        var header = currentPopup.transform.Find("Canvas/Header").GetComponent<TextMeshProUGUI>();
        if (header != null)
        {
            header.text = headerText;
        }
        else
        {
            Debug.LogError("[PopUpManager] Header not found");
        }

        // fill body
        var mainTextComponent = currentPopup.transform.Find("Canvas/Main Text").GetComponent<TextMeshProUGUI>();
        if (mainTextComponent != null)
        {
            mainTextComponent.text = mainText;
        }
        else
        {
            Debug.LogError("[PopUpManager] Main Text not foun");
        }

        // fill button
        var button = currentPopup.transform.Find("Canvas/Horizontal/Positive/Frontplate/AnimatedContent/Text")
            .GetComponent<TextMeshProUGUI>();
        if (button != null)
        {
            button.text = buttonText;

            // action
            var buttonobject = currentPopup.transform.Find("Canvas/Horizontal/Positive").gameObject;
            var buttonComponent = buttonobject.GetComponent<PressableButton>();
            if (buttonComponent != null)
            {
                buttonComponent.OnClicked.RemoveAllListeners(); 
                buttonComponent.OnClicked.AddListener(() =>
                {
                    ClosePopup();
                    buttonCallback?.Invoke();
                    
                });
            }
            else
            {
                Debug.LogError("[PopUpManager] button component not found!");
            }
        }
        else
        {
            Debug.LogError("[PopUpManager] Button not found");
        }

    }

    /* Creates and displays the big single pop up (aka single button but bigger size to fit text)
       Parameters:
       - headerText: title of the popup
       - mainText: body content of the popup
       - leftButtonText: label for the left button
       - rightButtonText: label for the right button
       - leftButtonCallback: action to execute when the left button is clicked
       - rightButtonCallback: action to execute when the right button is clicked
    */
    public void ShowBigPopUp(
        string popupId,
        string headerText,
        string mainText,
        string buttonText,
        Action buttonCallback)
    {
        Timer.Instance.Action($"Popup[{popupId}]");
        // only one is active
        if (currentPopup != null)
        {
            Destroy(currentPopup);
        }

        // instantiate the popup prefab
        currentPopup = Instantiate(singleButtonBrefab, transform);

        // fill title
        var header = currentPopup.transform.Find("Canvas/Header").GetComponent<TextMeshProUGUI>();
        if (header != null)
        {
            header.text = headerText;
        }
        else
        {
            Debug.LogError("[PopUpManager] Header not found");
        }

        // fill body
        var mainTextComponent = currentPopup.transform.Find("Canvas/Main Text").GetComponent<TextMeshProUGUI>();
        if (mainTextComponent != null)
        {
            mainTextComponent.text = mainText;
        }
        else
        {
            Debug.LogError("[PopUpManager] Main Text not foun");
        }

        // fill button
        var button = currentPopup.transform.Find("Canvas/Horizontal/Positive/Frontplate/AnimatedContent/Text")
            .GetComponent<TextMeshProUGUI>();
        if (button != null)
        {
            button.text = buttonText;

            // action
            var buttonobject = currentPopup.transform.Find("Canvas/Horizontal/Positive").gameObject;
            var buttonComponent = buttonobject.GetComponent<PressableButton>();
            if (buttonComponent != null)
            {
                buttonComponent.OnClicked.RemoveAllListeners(); 
                buttonComponent.OnClicked.AddListener(() =>
                {
                    buttonCallback?.Invoke();
                    ClosePopup();
                });
            }
            else
            {
                Debug.LogError("[PopUpManager] Button component not found!");
            }

        // scale the backplate
        Transform backplateTransform = currentPopup.transform.Find("Canvas/UX.Slate.ContentBackplate");

        if (backplateTransform != null)
        {
            // scale of Y to 170
            Vector3 newScale = backplateTransform.localScale;
            newScale.y = 170f;
            backplateTransform.localScale = newScale;

            Debug.Log("[PopUpManager] Adjusted UX.Slate.ContentBackplate scale to: " + backplateTransform.localScale);
        }
        else
        {
            Debug.LogError("[PopUpManager] UX.Slate.ContentBackplate not found in popup prefab!");
        }
        }
        else
        {
            Debug.LogError("[PopUpManager] Button not found");
        }

    }

    // no popup stays active after the user interaction is complete
    public void ClosePopup()
    {
        if (currentPopup != null)
        {
            Destroy(currentPopup);
            currentPopup = null;
        }
    }
}
