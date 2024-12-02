using System;
using System.Collections.Generic;
using MixedReality.Toolkit.UX;
using TMPro;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[Serializable]
class NTasks {
    public List<NEntry> tasks;
}

[Serializable]
class NEntry {
    public int id;
    public int priority;
    public string train_number;
    public string door_number;
    public string problem;
}

public class MenuManager : MonoBehaviour
{
    public GameObject listContainer;
    public GameObject listGrid;
    public GameObject listEntry;

    public string sceneName;

    public TextAsset jsonFile;

    // Start is called before the first frame update
    void Start()
    {
        
        if (NewSceneManager.Instance.TutorialActive)
        {
            BuildStartPopUp();

        }
        else
        {
        BuildMenu();
        }
    }

    private void BuildMenu(){

        Console.WriteLine(jsonFile.ToString());
        NTasks tasks = JsonUtility.FromJson<NTasks>(jsonFile.ToString());
        foreach (var entry in tasks.tasks)
        {
            Console.WriteLine("aooo", entry);
        }

        Debug.Log("root of the UI we're building is", gameObject);
        // We assume that the script is attached to a UIContainer, which always
        // has a ManipulatorContainer as a child
        Transform manipulationContainer = transform.GetChild(0);
        Debug.Log("ciaoo");
        print(manipulationContainer);
        Assert.AreNotEqual(manipulationContainer, null);
        GameObject sec = Instantiate(listContainer, new Vector3(0, 0, 0), Quaternion.identity, manipulationContainer);
        // Force local transform to be relative to the parent
        sec.transform.localPosition = new Vector3(0, 0, 0);
        // We assume that a listContainer GameObject always has a first child that is the title
        TextMeshProUGUI titleText = sec.GetComponentInChildren<TextMeshProUGUI>();
        Assert.AreNotEqual(titleText, null);
        titleText.SetText("Tasks");

        GameObject grid = Instantiate(listGrid, new Vector3(0, 0, 0), Quaternion.identity, sec.transform);
        grid.transform.localPosition = new Vector3(0, 0, 0);
        foreach (NEntry entry in tasks.tasks) {
            GameObject itm = Instantiate(listEntry, new Vector3(0, 0, 0), Quaternion.identity, grid.transform);
            addItemAction(itm);

            // Force local transform to be relative to the parent
            itm.transform.localPosition = new Vector3(0, 0, 0);
            TextMeshProUGUI[] texts = itm.GetComponentsInChildren<TextMeshProUGUI>();
            Assert.AreNotEqual(texts, null);
            texts[0].SetText(entry.problem);
            texts[1].SetText(entry.train_number);
            texts[2].SetText(entry.door_number);


            // Encode priority color
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
                        background.color = new Color(1f, 0.65f, 0f);
                        break;
                    case 3:
                        background.color = new Color(0f, 0.8f, 0f);
                        break;
                }
            }
            else
            {
                Debug.Log("Ignored ");
            }
            }
        }
    }

    private void BuildStartPopUp(){
            NewSceneManager.Instance.HideObject("ManipulationBar");
            NewSceneManager.Instance.HideObject("SettingsButton");
            NewPopUpManager.Instance.ShowBigPopUp(
            "Step 1: Choose a Maintenance Task",
            "Each row represents a maintenance task that has to be done. Each task is identified by: \nThe train and coach where it must be performed \nThe specific door to target \nA brief description of the problem \nTry to choose one maintenance task to continue with the tutorial",
            "Continue", 
            () =>
            {
                Debug.Log("Yass");
                NewSceneManager.Instance.ShowObject("ManipulationBar");
                NewSceneManager.Instance.ShowObject("SettingsButton");
                BuildMenu();
            }
            );
    }

    private void BuildFinishStep1(){
        NewSceneManager.Instance.HideObject("MenuPanel");
        NewPopUpManager.Instance.ShowSinglePopup(
            "Step 1 successfully completed!",
            "Click \"Continue\" to proceed with the tutorial.",
             "Continue", 
            () =>
            {
             Debug.Log("opt1");
             NewSceneManager.Instance.GoTo(new List<string> { "MenuSceneCanvas", "MenuPanel" });

            }
            );

    }
    private void addItemAction(GameObject item) {
        PressableButton pb = item.GetComponent<PressableButton>();
        pb.OnClicked.AddListener(LoadScene);
    }

    void LoadScene() {

        if (NewSceneManager.Instance.TutorialActive)
        {
            BuildFinishStep1();

        }
        else
        {
        Debug.Log("opt2");
        NewSceneManager.Instance.GoTo(new List<string> { "MenuSceneCanvas", "MenuPanel" });

        }
    }
}
