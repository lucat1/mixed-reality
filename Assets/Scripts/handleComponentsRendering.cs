using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class handleComponentsRendering : MonoBehaviour
{
    public GameObject Door;

    private GameObject recursiveFind(Transform parent, string target) {
        Queue<Transform> q = new ();
        foreach(Transform c in parent)
            q.Enqueue(c);

        while(q.Count > 0) {
            var child = q.Dequeue();
            if(child.name == target)
                return child.gameObject;
            
            foreach(Transform cc in child)
                q.Enqueue(cc);
        }

        return null;
    }

    List<string> LoadStringsFromFile(string path)
    {
        List<string> strings = new List<string>();

        if (File.Exists(path))
        {
            // Read all lines from the file
            string[] lines = File.ReadAllLines(path);
            strings.AddRange(lines);
        }
        else
        {
            Debug.LogError("File not found at: " + path);
        }

        return strings;
    }

    // Start is called before the first frame update
    // On start we delete the small components
    void Start()
    {
        
        string filePath = Path.Combine(Application.streamingAssetsPath, "small_components.txt");
        List<string> smallComponentsList = LoadStringsFromFile(filePath);
        
        int componentsEreased = 0;
        foreach(string component in smallComponentsList)
        {
            Debug.Log(component);
            GameObject componentObj = recursiveFind(Door.transform, component);
            if(componentObj)
            {
                componentObj.SetActive(false);
                componentsEreased++;
            }
        }

        Debug.Log("Small component ereased: " + componentsEreased + "/" + smallComponentsList.Count);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
