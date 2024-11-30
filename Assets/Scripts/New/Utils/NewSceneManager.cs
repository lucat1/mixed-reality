/*
The SceneManager script is responsible for managing transitions between scenes.

Key Features:
- Handles scene loading: Provides a method to load scenes by name.
- Manages tutorial state: Tracks whether the tutorial is active using a global boolean.
- Manages object visibility: Automatically hides all objects in a scene and selectively activates specified objects.

This script serves as a central control point for scene transitions and navigation logic
*/

using System.Collections.Generic;
using UnityEngine;

public class NewSceneManager : MonoBehaviour
{
    // singleton instance of SceneManager for global access
    public static NewSceneManager Instance;

    // boolean to track if the tutorial is active (read-only from outside SceneManager)
    public bool TutorialActive { get; private set; } = false;

    // sets up the singleton instance and ensures the SceneManager persists across scenes
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // all objects are hidden at the start
            HideAllObjects();

            // activate the specific object in the first scene
            print("SHOWW");
            ShowObject("LoginSceneCanvas");
            ShowObject("LoginPanel");  
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /* Loads a scene by name, hides all objects in the scene and activates a specified list of object names
       Parameters: 
        - sceneName: The name of the scene to load
        - objectNamesToActivate: The list of object names to activate after the scene is loaded 
    */
    public void NewLoadScene(string sceneName, List<string> objectNamesToActivate)
    {
        Debug.Log($"Loading scene: {sceneName}");
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        HideAllObjects(); 

        if (objectNamesToActivate != null)
        {
            foreach (var objectName in objectNamesToActivate)
            {
                ShowObject(objectName);
            }
        }

        Debug.Log($"{sceneName} loading completed");
    }

    // hide all objects in the loaded scene
    private void HideAllObjects()
    {
        foreach (GameObject obj in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects())
        {
            // Skip essential objects
            if (!obj.name.Contains("Scene"))
            {
                Debug.Log($"Skipping: {obj.name}");
            }
            else{
                obj.SetActive(false);
                foreach (Transform child in obj.transform)
                {
                    child.gameObject.SetActive(false);
                }
            }
        }

        Debug.Log("All objects in the scene are hidden");
    }

    // activate a specific GameObject by its name
    public void ShowObject(string objectName)
    {
        if (!string.IsNullOrEmpty(objectName))
        {
            print("stringa ce");
            GameObject obj = FindInScene(objectName);
            print(obj);
            if (obj != null)
            {
                obj.SetActive(true);
                Debug.Log($"Object '{objectName}' is active");
            }
            else
            {
                Debug.LogWarning($"Object '{objectName}' not found in the scene");
            }
        }
    }

    // deactivates a specific GameObject by its name
    public void HideObject(string objectName)
    {
        if (!string.IsNullOrEmpty(objectName))
        {
            GameObject obj = FindInScene(objectName);
            if (obj != null)
            {
                obj.SetActive(false);
                Debug.Log($"Object '{objectName}' is inactive");
            }
            else
            {
                Debug.LogWarning($"Object '{objectName}' not found in the scene");
            }
        }
    }

    // look for object in scene (even if inactive)
    private GameObject FindInScene(string objectName)
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


}
