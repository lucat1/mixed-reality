/*
The MenuManager script is responsible for managing the tasks menu.

Key Features:
- Dynamically builds the menu based on tasks from a JSON file.
- Displays a tutorial popup if the tutorial mode is active.
- Allows the user to select a maintenance task and proceed to the next step.
*/

using System;
using System.Collections;
using System.Collections.Generic;
using MixedReality.Toolkit.UX;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

[Serializable]
class NTasks
{
    public List<NEntry> tasks;
}

[Serializable]
class NEntry
{
    public int id;
    public int priority;
    public string train_number;
    public string coach_number;
    public string door_number;
    public string problem;
}

public class MenuManager : MonoBehaviour
{
    // Prefabs for UI components
    public GameObject listContainer; // The container for the task list
    public GameObject listGrid;      // The grid layout for task entries
    public GameObject listEntry;     // The individual task entry prefab
    public TextAsset jsonFile; // JSON file containing task data

    // start not needed -> can use only onEnable
    private void Start()
    {
    }

    /* called each time the menu is enabled, this happens in 3 cases:
        1. if user chooses tutorial -> call tutorial popups
        2. during the challenge -> call challenge popups
        3. on normal execution -> no popups
    */
    private void OnEnable()
    {
        if (NewSceneManager.Instance)
        {
            if (NewPopUpManager.Instance.IsPopupActive) // needed to avoid errors
            {
                // wait until all popups are closed
                StartCoroutine(WaitForPopupToClose(() =>
                {
                    ActivatePopupLogic();
                }));
            }
            else
            {
                ActivatePopupLogic();
            }}
        
    }

    private void ActivatePopupLogic()
    {
        if (NewSceneManager.Instance.TutorialActive) // 1. tutorial
        {
            BuildStep1TutorialPopUp();
        }
        else if (NewSceneManager.Instance.ChallengeActive) // 2. challenge
        {
            StartCoroutine(ShowPopupSequence());
        }
        else 
        {
            BuildMenu();
        }
    }

    // make one popup appear after another popup (wait until closed)
    private IEnumerator ShowPopupSequence()
    {
        NewSceneManager.Instance.HideObject("ManipulationContainer");
        bool firstPopupDone = false;

        // Popup #1
        NewPopUpManager.Instance.ShowSinglePopup(
            "WelcomeChallenge",
            "Welcome to the Challenge!",
            "This challenge is made of 4 tasks to complete in order to win. \nClick \"Continue\" to proceed to the first one.",
             "Continue", 
            () =>
            {
                firstPopupDone = true;
            }
            );

        // Wait
        yield return new WaitUntil(() => firstPopupDone);

        // Popup #2
         NewPopUpManager.Instance.ShowSinglePopup(
            "ChooseMaintenanceChallenge",
            "First Task",
            "The first task is about picking the maintenance task with train number \"001-5\".",
            "Continue", 
            () =>
            {
                BuildMenu();
            }
            );
            
        }

    // wait for popup to be closed in order not to have errors with new popup
    private IEnumerator WaitForPopupToClose(Action callback)
    {
        yield return new WaitUntil(() => !NewPopUpManager.Instance.IsPopupActive);
        callback.Invoke();
    }


    // builds the menu dynamically from the JSON file
    private void BuildMenu()
    {
        DestroyMenu();
        Debug.Log("Loading task data from JSON file...");
        // Parse the JSON file into a list of tasks
        NTasks tasks = JsonUtility.FromJson<NTasks>(jsonFile.ToString());

        // Log tasks for debugging
        foreach (var entry in tasks.tasks)
        {
            Debug.Log($"Task: {entry.problem}, Train: {entry.train_number}, Door: {entry.door_number}, Priority: {entry.priority}");
        }

        Debug.Log("Building the menu UI...");
        // Get the ManipulationContainer (root for UI elements)
        Transform manipulationContainer = transform.GetChild(0);
        Assert.IsNotNull(manipulationContainer, "ManipulationContainer is missing!");

        // Create the list container
        // GameObject sec = Instantiate(listContainer, Vector3.zero, Quaternion.identity, manipulationContainer);
        GameObject sec = Instantiate(listContainer, Vector3.zero, Quaternion.identity, transform);
        sec.name = "Menu";
        sec.transform.localPosition = Vector3.zero;

        // Set the title of the task list
        TextMeshProUGUI titleText = sec.GetComponentInChildren<TextMeshProUGUI>();
        Assert.IsNotNull(titleText, "Title Text is missing in ListContainer!");
        titleText.SetText("Tasks");

        // Create the grid for task entries
        GameObject grid = Instantiate(listGrid, Vector3.zero, Quaternion.identity, sec.transform);
        grid.transform.localPosition = Vector3.zero;

        // Populate the grid with tasks
        for (int i = 0; i < tasks.tasks.Count; i++)
        {
            NEntry entry = tasks.tasks[i];
            GameObject itm = Instantiate(listEntry, Vector3.zero, Quaternion.identity, grid.transform);
            itm.transform.localPosition = Vector3.zero;

            // in the challenge differentiate between correct && wrong pick
            if (i == 1 && NewSceneManager.Instance.ChallengeActive) // correct pick
            {
                AddSpecialItemActionSecond(itm);
            }
            else if(i != 1 && NewSceneManager.Instance.ChallengeActive) // wrong pick
            {
                AddSpecialItemActionGeneral(itm);
            }
            else
            {
                AddItemAction(itm);
            }

            // Set task details in the UI
            TextMeshProUGUI[] texts = itm.GetComponentsInChildren<TextMeshProUGUI>();
            Assert.IsNotNull(texts, "Task entry UI is missing Text components!");
            texts[0].SetText(entry.train_number);
            texts[1].SetText(entry.coach_number);
            texts[2].SetText(entry.door_number);
            texts[3].SetText(entry.problem);
            texts[4].SetText(entry.priority.ToString());
            switch (entry.priority)
            {
                case 1:
                    texts[4].color = Color.red; 
                    break;
                case 2:
                    texts[4].color = new Color(1f, 0.65f, 0f); 
                    break;
                case 3:
                    texts[4].color = new Color(0f, 0.8f, 0f);
                    break;
                default:
                    texts[4].color = Color.black;
                    break;
            }
        }
    }

    // this is needed in order to have buttons behave differently in tutorial/challenge/normal execution
    private void DestroyMenu()
    {
        Debug.Log("Destroying the menu UI...");
        
        // Get the ManipulationContainer (root for UI elements)
        Transform menu = transform.Find("Menu");
        if(menu != null)
            Destroy(menu.gameObject);
        else
            Debug.LogWarning("[MenuManager] menu not found, thus not deleting.");
    }

    // builds the initial tutorial popup that explains the menu structure
    private void BuildStep1TutorialPopUp()
    {
        NewSceneManager.Instance.HideObject("ManipulationBar");
        NewPopUpManager.Instance.ShowBigPopUp(
            "ChooseMaintenanceTutorial",
            "Step 1: Choose a Maintenance Task",
            "Each row represents a maintenance task that has to be done. Each task is identified by: \n" +
            "- The train and coach where it must be performed.\n" +
            "- The specific door to target.\n" +
            "- A brief description of the problem.\n\n" +
            "Try to choose one maintenance task to continue with the tutorial.",
            "Continue",
            () =>
            {
                Debug.Log("Continuing tutorial...");
                NewSceneManager.Instance.ShowObjects(new () {"ManipulationBar"});
                BuildMenu();
            }
        );
    }

    // Builds the popup shown after completing Step 1 in the tutorial
    private void BuildFinishStep1TutorialPopUp()
    {
        Debug.Log("Building the finish step 1 popup...");
        NewSceneManager.Instance.HideObject("MenuPanel");
        NewPopUpManager.Instance.ShowSinglePopup(
            "FinishChooseMaintenanceTutorial",
            "Step 1 successfully completed!",
            "Click \"Continue\" to proceed with the tutorial.",
            "Continue",
            () =>
            {
                NewSceneManager.Instance.GoTo("Placement", new () { "PlacementPanel" });
            }
        );
    }

    //Adds a click action to a task entry
    private void AddItemAction(GameObject item)
    {
        PressableButton pb = item.GetComponent<PressableButton>();
        Assert.IsNotNull(pb, "PressableButton is missing on task entry!");
        pb.OnClicked.AddListener(Pick);
    }
    private void AddSpecialItemActionSecond(GameObject item)
    {
        PressableButton pb = item.GetComponentInChildren<PressableButton>();
        Assert.IsNotNull(pb, "PressableButton is missing on task entry!");
        pb.OnClicked.AddListener(CorrectPick);
    }
     private void AddSpecialItemActionGeneral(GameObject item)
    {
        PressableButton pb = item.GetComponentInChildren<PressableButton>();
        Assert.IsNotNull(pb, "PressableButton is missing on task entry!");
        pb.OnClicked.AddListener(WongPick);
    }

    // Loads the next scene when a task entry is clicked
    private void Pick()
    {
        if (NewSceneManager.Instance.TutorialActive) // if tutorial: finish popup + load placement scene
        {
            BuildFinishStep1TutorialPopUp();
        }
        else if (!NewSceneManager.Instance.ChallengeActive) // else: load placement scene
        {
            NewSceneManager.Instance.HideObject("MenuSceneCanvas");
            NewSceneManager.Instance.GoTo("Placement", new List<string> { "PlacementPanel", "PlaceDoor" });
        }
    }

    // case challenge: correct pick for the challenge, popup + load placement scene
    private void CorrectPick()
    {
        NewSceneManager.Instance.HideObject("MenuSceneCanvas");
        NewPopUpManager.Instance.ShowSinglePopup(
            "FinishChooseMaintenanceChallenge",
            "First Task Completed!",
            "First task completed successfully! Now the second task regards placing the door correctly. Click continue to try!",
            "Continue",
            () =>
            {
                NewSceneManager.Instance.GoTo("Placement", new List<string> { "PlacementPanel", "PlaceDoor" });
            }
        );
    }
    // case challenge: wrong pick for the challenge, try again
    private void WongPick()
    {
        NewSceneManager.Instance.HideObject("ManipulationContainer");
        NewPopUpManager.Instance.ShowSinglePopup(
            "WrongChooseMaintenanceChallenge",
            "Watch Out!",
            "The task required you to choose the task with train number '001-5'. Try again!",
            "Try Again",
            () =>
            {
                NewSceneManager.Instance.ShowObjects(new (){"ManipulationContainer"});
            }
        );
    }
}