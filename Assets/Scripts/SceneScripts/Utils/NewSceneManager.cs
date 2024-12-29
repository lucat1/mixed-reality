/*
The SceneManager script is responsible for managing transitions between scenes.

Key Features:
- Handles scene loading: Provides a method to load scenes by name.
- Manages tutorial state: Tracks whether the tutorial is active using a global boolean.
- Manages object visibility: Automatically hides all objects in a scene and selectively activates specified objects.

This script serves as a central control point for scene transitions and navigation logic
*/

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public class NewSceneManager : MonoBehaviour
{
    public static NewSceneManager Instance; // singleton instance of SceneManager for global access
    public bool TutorialActive { get; private set; } = false; // boolean to track if the tutorial is active (read-only from outside SceneManager)
    public bool ChallengeActive { get; private set; } = false; // boolean to track if the challenge is active (read-only from outside SceneManager)
    public string PreviousScene { get; private set; } = null;
    public string Scene { get; private set; } = null;

    // sets up the singleton instance and ensures the SceneManager persists across scenes
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Hide all objects and show the initial scene
            GoTo("Login", new List<string> {"LoginPanel"});
            //GoTo(new List<string> { "PlacementSceneCanvas", "PlacementPanel" });
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // hide all objects in the loaded scene
    private void HideAllObjects()
    {
        foreach (GameObject obj in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects())
        {
            // Skip essential objects
            if (obj.name.Contains("Scene"))
            {
                obj.SetActive(false);
                foreach (Transform child in obj.transform)
                {
                    child.gameObject.SetActive(false);
                }
            }
        }

        Debug.Log("[SceneManager] All objects in the scene are hidden");
    }

    // activate a specific GameObject by its name
    private void ShowObject(string objectName)
    {
        Assert.IsFalse(string.IsNullOrEmpty(objectName));
        Debug.Log($"[SceneManager] Showing '{objectName}'");
        GameObject obj = FindInScene(objectName);
        if (obj != null)
            obj.SetActive(true);
        else
            Debug.LogError($"[SceneManager] Object '{objectName}' not found in the scene");
    }

    public void ShowObjects(List<string> objectNames) {
        foreach(var obj in objectNames)
            ShowObject(obj);

        Timer.Instance.Action($"Show[{Scene}+{string.Join("+", objectNames)}]");
    }

    // deactivate a specific GameObject by its name
    public void HideObject(string objectName)
    {
        if (!string.IsNullOrEmpty(objectName))
        {
            GameObject obj = FindInScene(objectName);
            if (obj != null)
            {
                obj.SetActive(false);
                //Debug.Log($"Object '{objectName}' is inactive");
            }
            else
            {
                Debug.LogWarning($"[SceneManager] Object '{objectName}' not found in the scene");
            }
        }
    }

    // look for object in scene (even if inactive)
    public GameObject FindInScene(string objectName)
    {
        foreach (GameObject rootObj in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects())
        {
            GameObject foundObj = Search(rootObj, objectName);
            if (foundObj != null)
            {
                return foundObj;
            }
        }
        return null;
    }

    // recursive function to search for object (even if nested)
    public GameObject Search(GameObject parent, string objectName)
    {
        if (parent.name == objectName)
        {
            return parent;
        }
        foreach (Transform child in parent.transform)
        {
            GameObject foundObj = Search(child.gameObject, objectName);
            if (foundObj != null)
            {
                return foundObj;
            }
        }

        return null;
    }

    public string SceneObjectName() {
        Assert.IsTrue(Scene != "");
        return Scene + "SceneCanvas";
    }

    // this is the same as loadScene except it does not change scene - it deactivates everything except from what is in objsToShow list
    public void GoTo(string scene, List<string> objsToShow)
    {
        HideAllObjects();
        if(Scene != scene) {
            PreviousScene = Scene;
            Scene = scene;
            ShowObject(SceneObjectName());
            Timer.Instance.Action($"LoadScene[{scene}]");
        }
        foreach (string obj in objsToShow)
        {
            ShowObject(obj);
        }
    }

    // update TutorialActive flag (to True)
    public void StartTutorial()
    {
        Timer.Instance.Action("StartTutorial");
        TutorialActive = true;
    }

    // update TutorialActive flag (to False)
    public void EndTutorial()
    {
        Timer.Instance.Action("FinishTutorial");
        TutorialActive = false;
    }

    // update ChallengeActive flag (to True)
    public void StartChallenge()
    {
        Timer.Instance.Action("StartChallenge");
        ChallengeActive = true;
    }

    // update ChallengeActive flag (to False)
    public void EndChallenge()
    {
        Timer.Instance.Action("FinishChallenge");
        ChallengeActive = false;
    }
}