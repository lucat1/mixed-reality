using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class DataLoader : MonoBehaviour
{
    public List<string> smallComponentsList = new List<string>();

    private List<string> LoadStringsFromFile(string path)
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
    // Start is called before the first frame update to load all the data
    void Start()
    {
        // load small components list from txt file
        string filePathSmallComponetns = Path.Combine(Application.streamingAssetsPath, "small_components.txt");
        smallComponentsList = LoadStringsFromFile(filePathSmallComponetns);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
