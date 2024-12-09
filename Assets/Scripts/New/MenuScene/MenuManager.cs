/*
The MenuManager script is responsible for managing the tasks menu.

Key Features:
- Dynamically builds the menu based on tasks from a JSON file.
- Displays a tutorial popup if the tutorial mode is active.
- Allows the user to select a maintenance task and proceed to the next step.
*/

using System;
using System.Collections.Generic;
using MixedReality.Toolkit.UX;
using TMPro;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using System;
using System.Collections;
using System.Collections.Generic;
using MixedReality.Toolkit;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.Assertions;
using System.Linq;

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
    public string door_number;
    public string problem;
}

public class MenuManager : MonoBehaviour
{
    // Prefabs for UI components
    public GameObject listContainer; // The container for the task list
    public GameObject listGrid;      // The grid layout for task entries
    public GameObject listEntry;     // The individual task entry prefab

    // JSON file containing task data
    public TextAsset jsonFile;

    public bool built = false;

    private void Start()
    {
        print("ENTRATO START");
        // Check if tutorial
        if (NewSceneManager.Instance.TutorialActive)
        {
            print("POPUPTUTORIAL");
            BuildStep1PopUp();
        }
    }

    private IEnumerator ShowPopupSequence()
    {
        NewSceneManager.Instance.HideObject("ManipulationContainer");
        bool firstPopupDone = false;

        // First Popup
        NewPopUpManager.Instance.ShowSinglePopup(
            "Welcome to the Challenge!",
            "This challenge is made of 4 tasks to complete in order to win. \nClick \"Continue\" to proceed to the first one.",
             "Continue", 
            () =>
            {
                firstPopupDone = true;
            }
            );

        // Wait until the first popup is closed
        yield return new WaitUntil(() => firstPopupDone);

        // Second Popup
         NewPopUpManager.Instance.ShowSinglePopup(
            "First Task",
            "The first task is about picking the maintenance task with train number \"001-5\".",
            "Continue", 
            () =>
            {
                DestroyMenu();
                NewSceneManager.Instance.ShowObject("ManipulationContainer");
                BuildMenu();
            }
            );
            
        }
    private IEnumerator WaitForPopupToClose(Action callback)
    {
        yield return new WaitUntil(() => !NewPopUpManager.Instance.IsPopupActive);
        callback.Invoke();
    }

    private void OnEnable()
    {
        if (NewSceneManager.Instance)
        {
            if (NewPopUpManager.Instance.IsPopupActive)
            {
                Debug.Log("[OnEnable] Popup is already active, waiting for closure.");
                StartCoroutine(WaitForPopupToClose(() =>
                {
                    ActivatePopupLogic();
                }));
            }
            else
            {
                ActivatePopupLogic();
            }
        }
    }

    private void ActivatePopupLogic()
    {
        if (NewSceneManager.Instance.TutorialActive)
        {

        }
        else if (NewSceneManager.Instance.ChallengeActive)
        {
            StartCoroutine(ShowPopupSequence());
        }
        else if (!built)
        {
            BuildMenu();
        }
        else if (built){
            DestroyMenu();
            BuildMenu();
        }
    }


    
    //Builds the menu dynamically from the JSON file.
    private void BuildMenu()
    {
        built=true;
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
        GameObject sec = Instantiate(listContainer, Vector3.zero, Quaternion.identity, manipulationContainer);
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

            // Set the action based on the index
            if (i == 1 && NewSceneManager.Instance.ChallengeActive)
            {
                print("si entrato");
                AddSpecialItemActionSecond(itm);
            }
            else if(i != 1 && NewSceneManager.Instance.ChallengeActive)
            {
                print("si entrato");
                AddSpecialItemActionGeneral(itm);
            }
            else
            {
                AddItemAction(itm);
            }

            // Set task details in the UI
            TextMeshProUGUI[] texts = itm.GetComponentsInChildren<TextMeshProUGUI>();
            Assert.IsNotNull(texts, "Task entry UI is missing Text components!");
            texts[0].SetText(entry.problem);
            texts[1].SetText(entry.train_number);
            texts[2].SetText(entry.door_number);

            // Set priority color
            Image[] backgrounds = itm.GetComponentsInChildren<Image>(true);
            foreach (Image background in backgrounds)
            {
                if (background.name == "PriorityColor")
                {
                    switch (entry.priority)
                    {
                        case 1:
                            background.color = Color.red;
                            break;
                        case 2:
                            background.color = new Color(1f, 0.65f, 0f); // Orange
                            break;
                        case 3:
                            background.color = new Color(0f, 0.8f, 0f); // Green
                            break;
                    }
                }
            }
        }
    }

    private void DestroyMenu()
{
    Debug.Log("Destroying the menu UI...");
    
    // Get the ManipulationContainer (root for UI elements)
    Transform manipulationContainer = transform.GetChild(0);
    Assert.IsNotNull(manipulationContainer, "ManipulationContainer is missing!");

    // Find the ListContainer (menu container)
    Transform listContainerTransform = manipulationContainer.Find(listContainer.name + "(Clone)");
    if (listContainerTransform != null)
    {
        // Destroy all instantiated child objects
        Destroy(listContainerTransform.gameObject);
        Debug.Log("Menu successfully destroyed.");
    }
    else
    {
        Debug.LogWarning("Menu not found to destroy.");
    }
}

    // Builds the initial tutorial popup that explains the menu structure.
    private void BuildStep1PopUp()
    {
        Debug.Log("Building the tutorial popup...");
        NewSceneManager.Instance.HideObject("ManipulationBar");
        NewPopUpManager.Instance.ShowBigPopUp(
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
                NewSceneManager.Instance.ShowObject("ManipulationBar");
                BuildMenu();
            }
        );
    }

    // Builds the popup shown after completing Step 1 in the tutorial
    private void BuildFinishStep1PopUp()
    {
        Debug.Log("Building the finish step 1 popup...");
        NewSceneManager.Instance.HideObject("MenuPanel");
        NewPopUpManager.Instance.ShowSinglePopup(
            "Step 1 successfully completed!",
            "Click \"Continue\" to proceed with the tutorial.",
            "Continue",
            () =>
            {
                NewSceneManager.Instance.GoTo(new List<string> { "PlacementSceneCanvas", "PlacementPanel" });
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
        if (NewSceneManager.Instance.TutorialActive)
        {
            BuildFinishStep1PopUp();
        }
        else if (!NewSceneManager.Instance.ChallengeActive)
        {
            NewSceneManager.Instance.HideObject("MenuSceneCanvas");
            NewSceneManager.Instance.GoTo(new List<string> { "PlacementSceneCanvas", "PlacementPanel", "PlaceDoor" });
        }
    }

    private void CorrectPick()
    {
        NewSceneManager.Instance.HideObject("MenuSceneCanvas");
        NewPopUpManager.Instance.ShowSinglePopup(
        "First Task Completed!",
        "First task completed successfully! Now the second task regards placing the door correctly. Click continue to try!",
        "Continue",
        () =>
        {
            Debug.Log("Proceeding to the second task.");
            // Add logic to proceed to the next step, e.g., go to the placement scene
            NewSceneManager.Instance.GoTo(new List<string> { "PlacementSceneCanvas", "PlacementPanel", "PlaceDoor" });
        }
        );

        
    }
    private void WongPick()
    {
        NewSceneManager.Instance.HideObject("ManipulationContainer");
        NewPopUpManager.Instance.ShowSinglePopup(
        "Watch Out!",
        "The task required you to choose the task with train number '001-5'. Try again!",
        "Try Again",
        () =>
        {
            Debug.Log("Retrying task selection.");
            NewSceneManager.Instance.ShowObject("ManipulationContainer");
           
        }
    );

        
    }
}