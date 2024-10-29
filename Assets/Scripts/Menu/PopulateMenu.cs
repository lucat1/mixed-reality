using System;
using System.Collections.Generic;
using MixedReality.Toolkit.UX;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
class Item {
    public String name;

    public Item(String name) => this.name = name;
}

class Section {
    public String title;
    public List<Item> items;

    public Section(String tl, List<Item> itms) {
        title = tl;
        items = itms;
    }
}

public class PopulateMenu : MonoBehaviour
{
    public GameObject listContainer;
    public GameObject listGrid;
    public GameObject listEntry;

    public string sceneName;

    private List<Section> sections = new List<Section>() {
        new("Section 1", new List<Item>() {
            new("Task 1"),
            new("Task 2")
        }),
    };

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("root of the UI we're building is", gameObject);
        // We assume that the script is attached to a UIContainer, which always
        // has a ManipulatorContainer as a child
        Transform manipulationContainer = transform.GetChild(0);
        Assert.AreNotEqual(manipulationContainer, null);
        foreach (Section section in sections) {
            GameObject sec = Instantiate(listContainer, new Vector3(0, 0, 0), Quaternion.identity, manipulationContainer);
            // Force local transform to be relative to the parent
            sec.transform.localPosition = new Vector3(0, 0, 0);
            // We assume that a listContainer GameObject always has a first child that is the title
            TextMeshProUGUI titleText = sec.GetComponentInChildren<TextMeshProUGUI>();
            Assert.AreNotEqual(titleText, null);
            titleText.SetText(section.title);

            GameObject grid = Instantiate(listGrid, new Vector3(0, 0, 0), Quaternion.identity, sec.transform);
            grid.transform.localPosition = new Vector3(0, 0, 0);
            foreach (Item item in section.items) {
                GameObject itm = Instantiate(listEntry, new Vector3(0, 0, 0), Quaternion.identity, grid.transform);
                addItemAction(itm);

                // Force local transform to be relative to the parent
                itm.transform.localPosition = new Vector3(0, 0, 0);
                TextMeshProUGUI itemText = itm.GetComponentInChildren<TextMeshProUGUI>();
                Assert.AreNotEqual(itemText, null);
                itemText.SetText(item.name);
            }
        }
    }

    private void addItemAction(GameObject item) {
        PressableButton pb = item.GetComponent<PressableButton>();
        pb.OnClicked.AddListener(LoadScene);
    }

    void LoadScene() {
        SceneManager.LoadScene(sceneName);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
